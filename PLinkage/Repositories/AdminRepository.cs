using Newtonsoft.Json;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.Repositories
{
    public class AdminRepository : IRepository<Admin>
    {
        private readonly string _filePath;
        private List<Admin> _data;

        public AdminRepository()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFolderPath = Path.GetFullPath(Path.Combine(projectPath, @"..\..\..\..\..\json"));
            _filePath = Path.Combine(jsonFolderPath, "Admin.txt");

            _data = File.Exists(_filePath)
                ? JsonConvert.DeserializeObject<List<Admin>>(File.ReadAllText(_filePath)) ?? new()
                : new();
        }

        public Task<List<Admin>> GetAllAsync() => Task.FromResult(_data);

        public Task<Admin?> GetByIdAsync(Guid id) =>
            Task.FromResult(_data.FirstOrDefault(u => u.UserId == id));

        public Task AddAsync(Admin entity)
        {
            _data.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Admin entity)
        {
            var index = _data.FindIndex(u => u.UserId == entity.UserId);
            if (index >= 0) _data[index] = entity;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var entity = _data.FirstOrDefault(u => u.UserId == id);
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
