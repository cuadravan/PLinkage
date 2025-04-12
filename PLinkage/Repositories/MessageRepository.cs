using Newtonsoft.Json;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.Repositories
{
    public class MessageRepository : IRepository<Message>
    {
        private readonly string _filePath;
        private List<Message> _data;

        public MessageRepository()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFolderPath = Path.GetFullPath(Path.Combine(projectPath, @"..\..\..\..\..\json"));
            _filePath = Path.Combine(jsonFolderPath, "Messages.txt");

            _data = File.Exists(_filePath)
                ? JsonConvert.DeserializeObject<List<Message>>(File.ReadAllText(_filePath)) ?? new()
                : new();
        }

        public Task<List<Message>> GetAllAsync() => Task.FromResult(_data);

        public Task<Message?> GetByIdAsync(Guid id) =>
            Task.FromResult(_data.FirstOrDefault(m => m.MessageId == id));

        public Task AddAsync(Message entity)
        {
            _data.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Message entity)
        {
            var index = _data.FindIndex(m => m.MessageId == entity.MessageId);
            if (index >= 0) _data[index] = entity;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var entity = _data.FirstOrDefault(m => m.MessageId == id);
            if (entity != null) _data.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            var json = JsonConvert.SerializeObject(_data, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
