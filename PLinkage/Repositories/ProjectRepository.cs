using Newtonsoft.Json;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.Repositories
{
    public class ProjectRepository : IRepository<Project>
    {
        private readonly string _filePath;
        private List<Project> _data;

        public ProjectRepository()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFolderPath = Path.GetFullPath(Path.Combine(projectPath, @"..\..\..\..\..\json"));
            _filePath = Path.Combine(jsonFolderPath, "Projects.txt");

            _data = File.Exists(_filePath)
                ? JsonConvert.DeserializeObject<List<Project>>(File.ReadAllText(_filePath)) ?? new()
                : new();
        }

        public Task<List<Project>> GetAllAsync() => Task.FromResult(_data);

        public Task<Project?> GetByIdAsync(Guid id) =>
            Task.FromResult(_data.FirstOrDefault(p => p.ProjectId == id));

        public Task AddAsync(Project entity)
        {
            _data.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Project entity)
        {
            var index = _data.FindIndex(p => p.ProjectId == entity.ProjectId);
            if (index >= 0) _data[index] = entity;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var entity = _data.FirstOrDefault(p => p.ProjectId == id);
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
