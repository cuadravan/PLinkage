using MongoDB.Driver;
using PLinkageAPI.Models;
using System.Linq.Expressions;

namespace PLinkageAPI.Repository
{
    public class SkillProviderRepository
    {
        private readonly IMongoCollection<SkillProvider> _skillProviders;

        public SkillProviderRepository(IMongoDatabase database)
        {
            // Use the collection name as the class name by convention
            _skillProviders = database.GetCollection<SkillProvider>("SkillProvider");
        }

        // Get all documents
        public async Task<List<SkillProvider>> GetAllAsync()
        {
            return await _skillProviders.Find(_ => true).ToListAsync();
        }

        // Get by ID
        public async Task<SkillProvider?> GetByIdAsync(Guid id)
        {
            var filter = Builders<SkillProvider>.Filter.Eq(p => p.UserId, id);
            return await _skillProviders.Find(filter).FirstOrDefaultAsync();
        }

        // Add a new document
        public async Task AddAsync(SkillProvider skillProvider)
        {
            await _skillProviders.InsertOneAsync(skillProvider);
        }

        // Update an existing document
        public async Task UpdateAsync(SkillProvider skillProvider)
        {
            var filter = Builders<SkillProvider>.Filter.Eq(p => p.UserId, skillProvider.UserId);
            await _skillProviders.ReplaceOneAsync(filter, skillProvider);
        }

        // Delete a document by ID
        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<SkillProvider>.Filter.Eq(p => p.UserId, id);
            await _skillProviders.DeleteOneAsync(filter);
        }

        // Generic filtering
        public async Task<List<SkillProvider>> FilterAsync(Expression<Func<SkillProvider, bool>> predicate)
        {
            return await _skillProviders.Find(predicate).ToListAsync();
        }

        //Bundled filtering
        public async Task<List<SkillProvider>> FilterAsync(FilterDefinition<SkillProvider> filter)
        {
            return await _skillProviders.Find(filter).ToListAsync();
        }

        // Optional: check if exists
        public async Task<bool> ExistsAsync(Guid id)
        {
            var filter = Builders<SkillProvider>.Filter.Eq(p => p.UserId, id);
            return await _skillProviders.Find(filter).AnyAsync();
        }
    }
}
