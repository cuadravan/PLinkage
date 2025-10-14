using MongoDB.Driver;
using PLinkageAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace PLinkageAPI.Repository
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;
        private readonly PropertyInfo? _idProperty;

        public MongoRepository(IMongoDatabase database)
        {
            var collectionName = typeof(T).Name;
            _collection = database.GetCollection<T>(collectionName);

            // Find a property of type Guid to use as the ID
            _idProperty = typeof(T).GetProperties()
                                   .FirstOrDefault(p => p.PropertyType == typeof(Guid)
                                                     && (p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)
                                                      || p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase)));
        }

        private FilterDefinition<T> IdFilter(Guid id)
        {
            if (_idProperty == null)
                throw new InvalidOperationException($"No suitable ID property found on {typeof(T).Name}");

            return Builders<T>.Filter.Eq(_idProperty.Name, id);
        }

        public async Task<List<T>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id) =>
            await _collection.Find(IdFilter(id)).FirstOrDefaultAsync();

        public async Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            if (_idProperty == null)
                throw new InvalidOperationException($"No suitable ID property found on {typeof(T).Name}");

            if (ids == null || !ids.Any())
                return new List<T>();

            var filter = Builders<T>.Filter.In(_idProperty.Name, ids);
            return await _collection.Find(filter).ToListAsync();
        }


        public async Task AddAsync(T entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(T entity)
        {
            if (_idProperty == null)
                throw new InvalidOperationException($"No suitable ID property found on {typeof(T).Name}");

            var idValue = (Guid)_idProperty.GetValue(entity)!;
            var filter = IdFilter(idValue);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id) =>
            await _collection.DeleteOneAsync(IdFilter(id));

        public async Task<IReadOnlyList<T>> FindAsync(FilterDefinition<T> filter) =>
            await _collection.Find(filter).ToListAsync();

        public async Task<bool> ExistsAsync(Guid id) =>
            await _collection.Find(IdFilter(id)).AnyAsync();
    }
}
