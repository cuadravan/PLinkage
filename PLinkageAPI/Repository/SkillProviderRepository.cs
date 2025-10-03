using MongoDB.Driver;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Specifications;
using PLinkageAPI.Models;
using System.Linq.Expressions;

namespace PLinkageAPI.Repository
{
    public class SkillProviderRepository: ISkillProviderRepository
    {
        private readonly IMongoCollection<SkillProvider> _skillProviders;

        public SkillProviderRepository(IMongoDatabase database)
        {
            // Use the collection name as the class name by convention
            _skillProviders = database.GetCollection<SkillProvider>("SkillProvider");
        }

        // Get all documents.. might remove
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

        public async Task<IEnumerable<SkillProvider>> FindAsync(ISpecification<SkillProvider> specification)
        {
            // 1. Start the query by applying the main filter criteria (the WHERE clause).
            // The MongoDB driver directly translates the C# Expression<Func<T, bool>>.
            IFindFluent<SkillProvider, SkillProvider> query =
                _skillProviders.Find(specification.Criteria);

            // NOTE: OrderBy, Paging (Skip/Limit), and Includes (Aggregation Pipeline) are omitted 
            // as those properties are not currently defined on your ISpecification<T> contract.

            // 2. Execute the final query and return the results.
            // ToListAsync() executes the query against MongoDB.
            return await query.ToListAsync();
        }

        // Optional: check if exists
        public async Task<bool> ExistsAsync(Guid id)
        {
            var filter = Builders<SkillProvider>.Filter.Eq(p => p.UserId, id);
            return await _skillProviders.Find(filter).AnyAsync();
        }
    }
}
