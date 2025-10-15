using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PLinkageAPI.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<IReadOnlyList<T>> FindAsync(FilterDefinition<T> filter);
        Task<bool> ExistsAsync(Guid id);
        Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids);

    }
}
