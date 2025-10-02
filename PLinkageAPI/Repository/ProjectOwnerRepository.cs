using MongoDB.Driver;
using PLinkageAPI.Models;
using System.Reflection;
using System.Linq.Expressions;

namespace PLinkageAPI.Repository
{
    public class ProjectOwnerRepository
    {
        private readonly IMongoCollection<ProjectOwner> _projectOwners;

        public ProjectOwnerRepository(IMongoDatabase database)
        {
            // Use the collection name as the class name by convention
            _projectOwners = database.GetCollection<ProjectOwner>("ProjectOwner");
        }

        // Get all documents
        public async Task<List<ProjectOwner>> GetAllAsync()
        {
            return await _projectOwners.Find(_ => true).ToListAsync();
        }

        // Get by ID
        public async Task<ProjectOwner?> GetByIdAsync(Guid id)
        {
            var filter = Builders<ProjectOwner>.Filter.Eq(p => p.UserId, id);
            return await _projectOwners.Find(filter).FirstOrDefaultAsync();
        }

        // Add a new document
        public async Task AddAsync(ProjectOwner projectOwner)
        {
            await _projectOwners.InsertOneAsync(projectOwner);
        }

        // Update an existing document
        public async Task UpdateAsync(ProjectOwner projectOwner)
        {
            var filter = Builders<ProjectOwner>.Filter.Eq(p => p.UserId, projectOwner.UserId);
            await _projectOwners.ReplaceOneAsync(filter, projectOwner);
        }

        // Delete a document by ID
        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<ProjectOwner>.Filter.Eq(p => p.UserId, id);
            await _projectOwners.DeleteOneAsync(filter);
        }

        // Generic filtering
        public async Task<List<ProjectOwner>> FilterAsync(Expression<Func<ProjectOwner, bool>> predicate)
        {
            return await _projectOwners.Find(predicate).ToListAsync();
        }

        // Bundled filtering
        public async Task<List<ProjectOwner>> FilterAsync(FilterDefinition<ProjectOwner> filter)
        {
            return await _projectOwners.Find(filter).ToListAsync();
        }

        // Optional: check if exists
        public async Task<bool> ExistsAsync(Guid id)
        {
            var filter = Builders<ProjectOwner>.Filter.Eq(p => p.UserId, id);
            return await _projectOwners.Find(filter).AnyAsync();
        }
    }
}
