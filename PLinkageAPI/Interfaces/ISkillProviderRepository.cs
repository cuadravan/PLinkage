using PLinkageAPI.Models;
using PLinkageAPI.Interfaces; // Assuming ISpecification is here
using System.Linq.Expressions;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLinkageAPI.Interfaces
{
    // Note: If you have a generic repository interface, this could inherit from it.
    public interface ISkillProviderRepository
    {
        // CRUD Operations
        Task<List<SkillProvider>> GetAllAsync();
        
        Task<SkillProvider?> GetByIdAsync(Guid id);
        
        Task AddAsync(SkillProvider skillProvider);
        
        Task UpdateAsync(SkillProvider skillProvider);
        
        Task DeleteAsync(Guid id);

        // Query/Filtering Operations
        Task<IEnumerable<SkillProvider>> FindAsync(ISpecification<SkillProvider> specification);

        // Utility
        Task<bool> ExistsAsync(Guid id);

        // RECOMMENDED: Add the Specification Pattern method signature
        // Task<List<SkillProvider>> FindAsync(ISpecification<SkillProvider> specification);
    }
}