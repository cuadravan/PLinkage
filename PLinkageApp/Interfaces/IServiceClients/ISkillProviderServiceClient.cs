using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.Interfaces
{
    public interface ISkillProviderServiceClient
    {
        Task<ApiResponse<IEnumerable<SkillProviderCardDto>>> GetFilteredSkillProvidersAsync(string proximity, CebuLocation? location, string status, bool? isEmployed);
        Task<ApiResponse<SkillProviderDto>> GetSpecificAsync(Guid skillProviderId);

        Task<ApiResponse<bool>> UpdateSkillProviderAsync(Guid skillProviderId, UserProfileUpdateDto skillProviderUpdateDto);
        Task<ApiResponse<bool>> AddEducationAsync(Guid skillProviderId, EducationDto educationToAdd);
        Task<ApiResponse<bool>> UpdateEducationAsync(Guid skillProviderId, int indexToUpdate, EducationDto educationToUpdate);
        Task<ApiResponse<bool>> DeleteEducationAsync(Guid skillProviderId, int indexToDelete);

        Task<ApiResponse<bool>> AddSkillAsync(Guid skillProviderId, SkillDto skillToAdd);
        Task<ApiResponse<bool>> UpdateSkillAsync(Guid skillProviderId, int indexToUpdate, SkillDto skillToUpdate);
        Task<ApiResponse<bool>> DeleteSkillAsync(Guid skillProviderId, int indexToDelete);
    }
}
