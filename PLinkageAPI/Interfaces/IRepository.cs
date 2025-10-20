using MongoDB.Driver;

namespace PLinkageAPI.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(IClientSessionHandle? session = null);
        Task<T?> GetByIdAsync(Guid id, IClientSessionHandle? session = null);
        Task AddAsync(T entity, IClientSessionHandle? session = null);
        Task UpdateAsync(T entity, IClientSessionHandle? session = null);
        Task DeleteAsync(Guid id, IClientSessionHandle? session = null);
        Task<IReadOnlyList<T>> FindAsync(FilterDefinition<T> filter, IClientSessionHandle? session = null);
        Task<bool> ExistsAsync(Guid id, IClientSessionHandle? session = null);
        Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids, IClientSessionHandle? session = null);
    }
}