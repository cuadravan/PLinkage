using Newtonsoft.Json;
using PLinkage.Domain.Interfaces;
using PLinkage.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace PLinkage.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _messageFilePath;

        public MessageRepository()
        {
            // Get project base path
            string _projectPath = AppDomain.CurrentDomain.BaseDirectory;

            // Get the full path to the JSON folder
            string _jsonFolderPath = Path.GetFullPath(Path.Combine(_projectPath, @"..\..\..\..\..\json"));

            // Combine folder path with the file name for messages
            _messageFilePath = Path.Combine(_jsonFolderPath, "Messages.txt");
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            if (File.Exists(_messageFilePath))
            {
                var json = await File.ReadAllTextAsync(_messageFilePath);
                return JsonConvert.DeserializeObject<List<Message>>(json) ?? new List<Message>();
            }
            return new List<Message>();
        }

        public async Task<Message?> GetMessageByIdAsync(Guid messageId)
        {
            var messages = await GetAllMessagesAsync();
            return messages.Find(m => m.MessageId == messageId);
        }

        public async Task AddMessageAsync(Message message)
        {
            var messages = await GetAllMessagesAsync();
            messages.Add(message);
            await SaveToFileAsync(messages);
        }

        public async Task UpdateMessageAsync(Message message)
        {
            var messages = await GetAllMessagesAsync();
            var existingMessage = messages.Find(m => m.MessageId == message.MessageId);
            if (existingMessage != null)
            {
                messages.Remove(existingMessage);
                messages.Add(message);
                await SaveToFileAsync(messages);
            }
        }

        public async Task DeleteMessageAsync(Guid messageId)
        {
            var messages = await GetAllMessagesAsync();
            var message = messages.Find(m => m.MessageId == messageId);
            if (message != null)
            {
                messages.Remove(message);
                await SaveToFileAsync(messages);
            }
        }

        private async Task SaveToFileAsync(List<Message> messages)
        {
            var json = JsonConvert.SerializeObject(messages, Newtonsoft.Json.Formatting.Indented);
            await File.WriteAllTextAsync(_messageFilePath, json);
        }
    }
}
