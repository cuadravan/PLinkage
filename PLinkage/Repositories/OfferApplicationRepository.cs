using Newtonsoft.Json;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.Repositories
{
    public class OfferApplicationRepository : IRepository<OfferApplication>
    {
        private readonly string _filePath;
        private List<OfferApplication> _data;

        public OfferApplicationRepository()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFolderPath = Path.GetFullPath(Path.Combine(projectPath, @"..\..\..\..\..\json"));
            // Ensure the directory exists
            Directory.CreateDirectory(jsonFolderPath);

            _filePath = Path.Combine(jsonFolderPath, "OfferApplications.txt");

            // Ensure file exists, if not, create it with an empty array
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }

            // Try to deserialize
            try
            {
                var json = File.ReadAllText(_filePath);
                _data = JsonConvert.DeserializeObject<List<OfferApplication>>(json) ?? new List<OfferApplication>();
            }
            catch
            {
                _data = new List<OfferApplication>(); // fallback on corruption
            }
        }

        public Task<List<OfferApplication>> GetAllAsync()
        {
            return Task.FromResult(_data);
        }

        public Task<OfferApplication?> GetByIdAsync(Guid id)
        {
            var item = _data.FirstOrDefault(o => o.OfferApplicationId == id);
            return Task.FromResult(item);
        }

        public Task AddAsync(OfferApplication entity)
        {
            _data.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(OfferApplication entity)
        {
            var index = _data.FindIndex(o => o.OfferApplicationId == entity.OfferApplicationId);
            if (index >= 0)
                _data[index] = entity;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var item = _data.FirstOrDefault(o => o.OfferApplicationId == id);
            if (item != null)
                _data.Remove(item);

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
                _data = JsonConvert.DeserializeObject<List<OfferApplication>>(json) ?? new();
            }
        }
    }
}
