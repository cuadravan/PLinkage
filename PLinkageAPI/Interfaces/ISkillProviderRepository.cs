using PLinkageAPI.Entities;

namespace PLinkageAPI.Interfaces
{
    public interface ISkillProviderRepository
    {
        Task<List<SkillProvider>> GetAllAsync();
        
        Task<SkillProvider?> GetByIdAsync(Guid id);
        
        Task AddAsync(SkillProvider skillProvider);
        
        Task UpdateAsync(SkillProvider skillProvider);
        
        Task DeleteAsync(Guid id);

        Task<IEnumerable<SkillProvider>> FindAsync(ISpecification<SkillProvider> specification);

        Task<bool> ExistsAsync(Guid id);
    }
}