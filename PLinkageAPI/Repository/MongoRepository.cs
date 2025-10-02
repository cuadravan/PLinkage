using MongoDB.Driver;
using System.Linq.Expressions;

namespace PLinkageAPI.Repository
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;
        private readonly string _idPropertyName;

        public MongoRepository(IMongoDatabase database, string collectionName, string idPropertyName)
        {
            _collection = database.GetCollection<T>(collectionName);
            _idPropertyName = idPropertyName;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(_idPropertyName, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            var idProp = typeof(T).GetProperty(_idPropertyName);
            if (idProp == null)
                throw new InvalidOperationException(
                    $"Type {typeof(T).Name} does not have a property named '{_idPropertyName}'");

            var id = (Guid)idProp.GetValue(entity)!;
            var filter = Builders<T>.Filter.Eq(_idPropertyName, id);

            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(_idPropertyName, id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<List<T>> FilterAsync(FilterDefinition<T> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(_idPropertyName, id);
            return await _collection.Find(filter).AnyAsync();
        }
    }
}
