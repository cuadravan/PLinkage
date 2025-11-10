using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageAPI.Interfaces
{
    public interface IChatService
    {
        Task<ApiResponse<IEnumerable<ChatSummaryDto>>> GetChatSummariesAsync(Guid currentUserId);

        Task<ApiResponse<IEnumerable<ChatMessageDto>>> GetChatMessagesAsync(Guid chatId, Guid currentUserId);

        Task<ApiResponse<ChatMessageDto>> SendMessageAsync(SendMessageDto messageDto, Guid senderId);

        Task<ApiResponse<Guid>> GetChatIdAsync(Guid senderId, Guid receiverId);
    }
}
