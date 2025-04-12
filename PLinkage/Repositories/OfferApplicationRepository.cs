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
            _filePath = Path.Combine(jsonFolderPath, "OfferApplications.txt");

            _data = File.Exists(_filePath)
                ? JsonConvert.DeserializeObject<List<OfferApplication>>(File.ReadAllText(_filePath)) ?? new()
                : new();
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
    }
}
