using PLinkageApp.Interfaces;
using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.Services
{
    public class ChatServiceClient: BaseApiClient, IChatServiceClient
    {
        public ChatServiceClient(HttpClient httpClient) : base(httpClient) { }
        public async Task<ApiResponse<IEnumerable<ChatSummaryDto>>> GetChatSummariesAsync(Guid userId)
        {
            return await GetAsync<IEnumerable<ChatSummaryDto>>($"api/chat/summaries/{userId}");
        }
        public async Task<ApiResponse<IEnumerable<ChatMessageDto>>> GetChatMessagesAsync(Guid chatId, Guid viewingUserId)
        {
            return await GetAsync<IEnumerable<ChatMessageDto>>($"api/chat/{chatId}/messages/{viewingUserId}");
        }
        public async Task<ApiResponse<ChatMessageDto>> SendMessageAsync(Guid userId, SendMessageDto sendMessageDto)
        {
            return await PostAsync<SendMessageDto, ChatMessageDto>($"api/chat/send/{userId}", sendMessageDto);
        }

    }
}
