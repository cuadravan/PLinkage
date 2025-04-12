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
            // Ensure the directory exists
            Directory.CreateDirectory(jsonFolderPath);

            _filePath = Path.Combine(jsonFolderPath, "Projects.txt");

            // Ensure file exists, if not, create it with an empty array
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }

            // Try to deserialize
            try
            {
                var json = File.ReadAllText(_filePath);
                _data = JsonConvert.DeserializeObject<List<Project>>(json) ?? new List<Project>();
            }
            catch
            {
                _data = new List<Project>(); // fallback on corruption
            }
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

        public async Task Reload()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _data = JsonConvert.DeserializeObject<List<Project>>(json) ?? new();
            }
        }
    }
}
