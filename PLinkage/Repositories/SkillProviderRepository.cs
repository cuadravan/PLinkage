using Newtonsoft.Json;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.Repositories
{
    public class SkillProviderRepository : IRepository<SkillProvider>
    {
        private readonly string _filePath;
        private List<SkillProvider> _data;

        public SkillProviderRepository()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFolderPath = Path.GetFullPath(Path.Combine(projectPath, @"..\..\..\..\..\json"));
            // Ensure the directory exists
            Directory.CreateDirectory(jsonFolderPath);

            _filePath = Path.Combine(jsonFolderPath, "SkillProvider.txt");

            // Ensure file exists, if not, create it with an empty array
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }

            // Try to deserialize
            try
            {
                var json = File.ReadAllText(_filePath);
                _data = JsonConvert.DeserializeObject<List<SkillProvider>>(json) ?? new List<SkillProvider>();
            }
            catch
            {
                _data = new List<SkillProvider>(); // fallback on corruption
            }
        }

        public Task<List<SkillProvider>> GetAllAsync() => Task.FromResult(_data);

        public Task<SkillProvider?> GetByIdAsync(Guid id) =>
            Task.FromResult(_data.FirstOrDefault(u => u.UserId == id));

        public Task AddAsync(SkillProvider entity)
        {
            _data.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(SkillProvider entity)
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

        public async Task Reload()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _data = JsonConvert.DeserializeObject<List<SkillProvider>>(json) ?? new();
            }
        }
    }
}
