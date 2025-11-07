using PLinkageShared.DTOs;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using System.Linq;
using MongoDB.Driver;
using PLinkageShared.Enums;
using PLinkageShared.ApiResponse;

namespace PLinkageAPI.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<Chat> _chatRepository;
        private readonly IRepository<SkillProvider> _skillProviderRepository;
        private readonly IRepository<ProjectOwner> _projectOwnerRepository;
        private readonly IRepository<Admin> _adminRepository;
        private readonly IMongoClient _mongoClient;

        public ChatService(
            IRepository<Chat> chatRepository,
            IRepository<SkillProvider> skillProviderRepository,
            IRepository<ProjectOwner> projectOwnerRepository,
            IRepository<Admin> adminRepository,
            IMongoClient mongoClient)
        {
            _chatRepository = chatRepository;
            _skillProviderRepository = skillProviderRepository;
            _projectOwnerRepository = projectOwnerRepository;
            _adminRepository = adminRepository;
            _mongoClient = mongoClient;
        }

        public async Task<ApiResponse<IEnumerable<ChatSummaryDto>>> GetChatSummariesAsync(Guid currentUserId)
        {
            try
            {
                IUser? user = null;
                user = await _skillProviderRepository.GetByIdAsync(currentUserId);
                if (user == null)
                    user = await _projectOwnerRepository.GetByIdAsync(currentUserId);
                if (user == null)
                    user = await _adminRepository.GetByIdAsync(currentUserId);

                if (user == null)
                    return ApiResponse<IEnumerable<ChatSummaryDto>>.Fail("User could not be found.");

                var chatIds = user.UserMessagesId;
                var allChats = await _chatRepository.GetByIdsAsync(chatIds);
                var summaries = new List<ChatSummaryDto>();

                foreach (var chat in allChats)
                {
                    if (!chat.MessengerId.Contains(currentUserId))
                        continue;

                    var receiverId = chat.MessengerId.FirstOrDefault(id => id != currentUserId);
                    if (receiverId == Guid.Empty) continue;

                    string fullName = await GetUserNameAsync(receiverId);

                    var latest = chat.Messages.OrderByDescending(m => m.MessageDate).FirstOrDefault();

                    summaries.Add(new ChatSummaryDto
                    {
                        ChatId = chat.ChatId,
                        ReceiverId = receiverId,
                        ReceiverFullName = fullName,
                        MostRecentMessage = latest?.MessageContent ?? "(No message)",
                        MessageDate = latest?.MessageDate ?? DateTime.MinValue
                    });
                }

                var result = summaries.OrderByDescending(s => s.MessageDate);
                return ApiResponse<IEnumerable<ChatSummaryDto>>.Ok(result, "Chat summaries retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ChatSummaryDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<ChatMessageDto>>> GetChatMessagesAsync(Guid chatId, Guid currentUserId)
        {
            using var session = await _mongoClient.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var chat = await _chatRepository.GetByIdAsync(chatId, session);
                if (chat == null)
                {
                    await session.AbortTransactionAsync();
                    return ApiResponse<IEnumerable<ChatMessageDto>>.Fail("Chat not found.");
                }

                bool updated = false;
                foreach (var msg in chat.Messages)
                {
                    if (!msg.IsRead && msg.ReceiverId == currentUserId)
                    {
                        msg.IsRead = true;
                        updated = true;
                    }
                }

                if (updated)
                {
                    await _chatRepository.UpdateAsync(chat, session);
                }

                await session.CommitTransactionAsync();

                var messages = chat.Messages
                    .OrderBy(m => m.MessageDate)
                    .Select(m => new ChatMessageDto
                    {
                        MessageId = m.MessageId,
                        Content = m.MessageContent,
                        Date = m.MessageDate,
                        IsFromCurrentUser = m.SenderId == currentUserId,
                        SenderId = m.SenderId,
                        IsRead = m.IsRead
                    })
                    .ToList();

                return ApiResponse<IEnumerable<ChatMessageDto>>.Ok(messages, "Messages retrieved successfully.");
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                return ApiResponse<IEnumerable<ChatMessageDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<ChatMessageDto>> SendMessageAsync(SendMessageDto messageDto, Guid senderId)
        {
            if (messageDto.ReceiverId == senderId)
                return ApiResponse<ChatMessageDto>.Fail("Cannot send message to yourself.");

            using var session = await _mongoClient.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var filter = Builders<Chat>.Filter.And(
                    Builders<Chat>.Filter.AnyEq(c => c.MessengerId, senderId),
                    Builders<Chat>.Filter.AnyEq(c => c.MessengerId, messageDto.ReceiverId),
                    Builders<Chat>.Filter.Size(c => c.MessengerId, 2)
                );

                var chat = (await _chatRepository.FindAsync(filter, session)).FirstOrDefault();

                bool isNewChat = false;
                if (chat == null)
                {
                    chat = new Chat
                    {
                        ChatId = Guid.NewGuid(),
                        MessengerId = new List<Guid> { senderId, messageDto.ReceiverId }
                    };
                    isNewChat = true;
                }

                var newMessage = new Message
                {
                    MessageId = Guid.NewGuid(),
                    SenderId = senderId,
                    ReceiverId = messageDto.ReceiverId,
                    MessageContent = messageDto.Content,
                    MessageOrder = chat.Messages.Count + 1,
                    MessageDate = DateTime.UtcNow
                };

                chat.Messages.Add(newMessage);

                if (isNewChat)
                {
                    await _chatRepository.AddAsync(chat, session);
                }
                else
                {
                    await _chatRepository.UpdateAsync(chat, session);
                }

                await AddChatIdToUserAsync(senderId, chat.ChatId, session);
                await AddChatIdToUserAsync(messageDto.ReceiverId, chat.ChatId, session);

                await session.CommitTransactionAsync();

                var resultDto = new ChatMessageDto
                {
                    MessageId = newMessage.MessageId,
                    Content = newMessage.MessageContent,
                    Date = newMessage.MessageDate,
                    IsFromCurrentUser = true,
                    SenderId = newMessage.SenderId,
                    IsRead = false,
                    ChatId = chat.ChatId
                };

                return ApiResponse<ChatMessageDto>.Ok(resultDto, "Message sent successfully.");
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                return ApiResponse<ChatMessageDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<Guid>> GetChatIdAsync(Guid senderId, Guid receiverId)
        {
            var filter = Builders<Chat>.Filter.And(
                    Builders<Chat>.Filter.AnyEq(c => c.MessengerId, senderId),
                    Builders<Chat>.Filter.AnyEq(c => c.MessengerId, receiverId),
                    Builders<Chat>.Filter.Size(c => c.MessengerId, 2)
                );

            var chat = (await _chatRepository.FindAsync(filter)).FirstOrDefault();
            
            if(chat == null)
            {
                return ApiResponse<Guid>.Fail("No chat exists for these users yet");
            }
            else
            {
                return ApiResponse<Guid>.Ok(chat.ChatId, "Found chat");
            }
        }

        private async Task<string> GetUserNameAsync(Guid userId)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(userId);
            if (sp != null)
                return $"{sp.UserFirstName} {sp.UserLastName}";

            var po = await _projectOwnerRepository.GetByIdAsync(userId);
            if (po != null)
                return $"{po.UserFirstName} {po.UserLastName}";

            var admin = await _adminRepository.GetByIdAsync(userId);
            if (admin != null)
                return $"{admin.UserFirstName} {admin.UserLastName}";

            return "Unknown User";
        }

        private async Task AddChatIdToUserAsync(Guid userId, Guid chatId, IClientSessionHandle session)
        {
            IUser? user = await _skillProviderRepository.GetByIdAsync(userId, session);
            if (user == null)
                user = await _projectOwnerRepository.GetByIdAsync(userId, session);
            if (user == null)
                user = await _adminRepository.GetByIdAsync(userId, session);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            if (user.AddChat(chatId))
            {
                switch (user.UserRole)
                {
                    case UserRole.SkillProvider:
                        await _skillProviderRepository.UpdateAsync(user as SkillProvider, session);
                        break;
                    case UserRole.ProjectOwner:
                        await _projectOwnerRepository.UpdateAsync(user as ProjectOwner, session);
                        break;
                    case UserRole.Admin:
                        await _adminRepository.UpdateAsync(user as Admin, session);
                        break;
                }
            }
        }
    }
}