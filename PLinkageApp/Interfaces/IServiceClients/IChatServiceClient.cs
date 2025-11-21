using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.Interfaces
{
    public interface IChatServiceClient
    {
        Task<ApiResponse<IEnumerable<ChatSummaryDto>>> GetChatSummariesAsync(Guid userId);
        Task<ApiResponse<IEnumerable<ChatMessageDto>>> GetChatMessagesAsync(Guid chatId, Guid viewingUserId);
        Task<ApiResponse<ChatMessageDto>> SendMessageAsync(Guid userId, SendMessageDto sendMessageDto);
        Task<ApiResponse<Guid>> GetChatIdAsync(Guid senderId, Guid receiverId);
    }
}
