using PLinkageAPI.Entities;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageAPI.Interfaces
{
    public interface IProjectOwnerService
    {
        Task<ApiResponse<ProjectOwner?>> GetSpecificProjectOwnerAsync(Guid projectOwnerId);

        Task<ApiResponse<IEnumerable<ProjectOwner>>> GetFilteredProjectOwnerAsync(
            string proximity,
            CebuLocation? location,
            string status); 

        Task<ApiResponse<string>> UpdateProjectOwnerAsync(Guid projectOwnerId, UserProfileUpdateDto projectOwnerUpdateDto);
    }
}
