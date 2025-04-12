using Newtonsoft.Json;
using PLinkage.Interfaces;
using System.Reflection;

namespace PLinkage.Repositories
{
    public class JsonRepository<T> : IRepository<T> where T : class
    {
        private readonly string _filePath;
        private List<T> _data;

        public JsonRepository(string fileName)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFolderPath = Path.GetFullPath(Path.Combine(projectPath, @"..\..\..\..\..\json"));

            Directory.CreateDirectory(jsonFolderPath);

            _filePath = Path.Combine(jsonFolderPath, $"{fileName}.txt");

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                _data = JsonConvert.DeserializeObject<List<T>>(json) ?? new();
            }
            catch
            {
                _data = new();
            }
        }

        public Task<List<T>> GetAllAsync() => Task.FromResult(_data);

        public Task<T?> GetByIdAsync(Guid id)
        {
            var prop = typeof(T).GetProperty("UserId") ?? typeof(T).GetProperty("Id");
            var entity = _data.FirstOrDefault(item => (Guid?)prop?.GetValue(item) == id);
            return Task.FromResult(entity);
        }

        public Task AddAsync(T entity)
        {
            _data.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity)
        {
            var prop = typeof(T).GetProperty("UserId") ?? typeof(T).GetProperty("Id");
            if (prop == null) return Task.CompletedTask;

            var id = (Guid?)prop.GetValue(entity);
            var index = _data.FindIndex(item => (Guid?)prop.GetValue(item) == id);
            if (index >= 0) _data[index] = entity;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var prop = typeof(T).GetProperty("UserId") ?? typeof(T).GetProperty("Id");
            var entity = _data.FirstOrDefault(item => (Guid?)prop?.GetValue(item) == id);
            if (entity != null) _data.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            var json = JsonConvert.SerializeObject(_data, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public Task Reload()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _data = JsonConvert.DeserializeObject<List<T>>(json) ?? new();
            }

            return Task.CompletedTask;
        }
    }
}
