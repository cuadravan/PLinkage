using PLinkage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLinkage.Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllMessagesAsync();
        Task<Message?> GetMessageByIdAsync(Guid messageId);
        Task AddMessageAsync(Message message);
        Task UpdateMessageAsync(Message message);
        Task DeleteMessageAsync(Guid messageId);
    }
}
