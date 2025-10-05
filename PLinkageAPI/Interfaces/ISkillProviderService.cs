using PLinkageAPI.Entities;
using PLinkageShared.Enums;

namespace PLinkageAPI.Interfaces
{
    public interface ISkillProviderService
    {
        Task<SkillProvider?> GetSpecificSkillProviderAsync(Guid skillProviderId);
        Task<bool> AddEducationAsync(Guid skillProviderId, Education educationToAdd);
        Task<bool> UpdateEducationAsync(Guid skillProviderId, int indexToUpdate, Education educationToUpdate);
        Task<bool> DeleteEducationAsync(Guid skillProviderId, int indexToDelete);
        Task<bool> AddSkillAsync(Guid skillProviderId, Skill skillToAdd);
        Task<bool> UpdateSkillAsync(Guid skillProviderId, int indexToUpdate, Skill skillToUpdate);
        Task<bool> DeleteSkillAsync(Guid skillProviderId, int indexToDelete);
        Task<IEnumerable<SkillProvider>> GetFilteredSkillProvidersAsync(
            string proximity,
            CebuLocation? location,
            string status);
    }
}