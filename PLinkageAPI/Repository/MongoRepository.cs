using MongoDB.Driver;
using PLinkageAPI.Interfaces;
using System.Reflection;

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

        public async Task<List<T>> GetAllAsync(IClientSessionHandle? session = null) =>
            session != null
                ? await _collection.Find(session, _ => true).ToListAsync()
                : await _collection.Find(_ => true).ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id, IClientSessionHandle? session = null) =>
            session != null
                ? await _collection.Find(session, IdFilter(id)).FirstOrDefaultAsync()
                : await _collection.Find(IdFilter(id)).FirstOrDefaultAsync();

        public async Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids, IClientSessionHandle? session = null)
        {
            if (_idProperty == null)
                throw new InvalidOperationException($"No suitable ID property found on {typeof(T).Name}");

            if (ids == null || !ids.Any())
                return new List<T>();

            var filter = Builders<T>.Filter.In(_idProperty.Name, ids);
            return session != null
                ? await _collection.Find(session, filter).ToListAsync()
                : await _collection.Find(filter).ToListAsync();
        }

        public async Task AddAsync(T entity, IClientSessionHandle? session = null) =>
           await (session != null
                ? _collection.InsertOneAsync(session, entity)
                : _collection.InsertOneAsync(entity));

        public async Task UpdateAsync(T entity, IClientSessionHandle? session = null)
        {
            if (_idProperty == null)
                throw new InvalidOperationException($"No suitable ID property found on {typeof(T).Name}");

            var idValue = (Guid)_idProperty.GetValue(entity)!;
            var filter = IdFilter(idValue);

            if (session != null)
                await _collection.ReplaceOneAsync(session, filter, entity);
            else
                await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id, IClientSessionHandle? session = null) =>
    await (session != null
        ? _collection.DeleteOneAsync(session, IdFilter(id))
        : _collection.DeleteOneAsync(IdFilter(id)));

        public async Task<IReadOnlyList<T>> FindAsync(FilterDefinition<T> filter, IClientSessionHandle? session = null) =>
            session != null
                ? await _collection.Find(session, filter).ToListAsync()
                : await _collection.Find(filter).ToListAsync();

        public async Task<bool> ExistsAsync(Guid id, IClientSessionHandle? session = null) =>
            session != null
                ? await _collection.Find(session, IdFilter(id)).AnyAsync()
                : await _collection.Find(IdFilter(id)).AnyAsync();
    }
}   