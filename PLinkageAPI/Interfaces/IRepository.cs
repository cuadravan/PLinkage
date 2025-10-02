using MongoDB.Driver;
using System.Linq.Expressions;

namespace PLinkageAPI.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> FilterAsync(FilterDefinition<T> filter);
        Task<bool> ExistsAsync(Guid id);
    }
}
