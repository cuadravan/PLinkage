using PLinkageAPI.Entities;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageAPI.Interfaces
{
    public interface ISkillProviderService
    {
        Task<ApiResponse<SkillProvider>> GetSpecificSkillProviderAsync(Guid skillProviderId);
        Task<ApiResponse<bool>> UpdateSkillProviderAsync(Guid skillProviderId, UserProfileUpdateDto skillProviderUpdateDto);

        // ---------- Educations ----------
        Task<ApiResponse<bool>> AddEducationAsync(Guid skillProviderId, Education educationToAdd);
        Task<ApiResponse<bool>> UpdateEducationAsync(Guid skillProviderId, int indexToUpdate, Education educationToUpdate);
        Task<ApiResponse<bool>> DeleteEducationAsync(Guid skillProviderId, int indexToDelete);

        // ---------- Skills ----------
        Task<ApiResponse<bool>> AddSkillAsync(Guid skillProviderId, Skill skillToAdd);
        Task<ApiResponse<bool>> UpdateSkillAsync(Guid skillProviderId, int indexToUpdate, Skill skillToUpdate);
        Task<ApiResponse<bool>> DeleteSkillAsync(Guid skillProviderId, int indexToDelete);

        // ---------- Filters ----------
        Task<ApiResponse<IEnumerable<SkillProviderCardDto>>> GetFilteredSkillProvidersAsync(
            string proximity,
            CebuLocation? location,
            string status);
    }
}
