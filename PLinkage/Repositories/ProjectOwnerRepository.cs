using Newtonsoft.Json;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.Repositories
{
    public class ProjectOwnerRepository : IRepository<ProjectOwner>
    {
        private readonly string _filePath;
        private List<ProjectOwner> _data;

        public ProjectOwnerRepository()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFolderPath = Path.GetFullPath(Path.Combine(projectPath, @"..\..\..\..\..\json"));
            _filePath = Path.Combine(jsonFolderPath, "Users.txt");

            _data = File.Exists(_filePath)
                ? JsonConvert.DeserializeObject<List<ProjectOwner>>(File.ReadAllText(_filePath)) ?? new()
                : new();
        }

        public Task<List<ProjectOwner>> GetAllAsync() => Task.FromResult(_data);

        public Task<ProjectOwner?> GetByIdAsync(Guid id) =>
            Task.FromResult(_data.FirstOrDefault(u => u.UserId == id));

        public Task AddAsync(ProjectOwner entity)
        {
            _data.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ProjectOwner entity)
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
