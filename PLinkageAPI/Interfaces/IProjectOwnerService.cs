using PLinkageAPI.Entities;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageAPI.Interfaces
{
    public interface IProjectOwnerService
    {
        Task<ApiResponse<ProjectOwnerDto?>> GetSpecificProjectOwnerAsync(Guid projectOwnerId);

        Task<ApiResponse<IEnumerable<ProjectOwnerCardDto>>> GetFilteredProjectOwnerAsync(
            string proximity,
            CebuLocation? location,
            string status); 

        Task<ApiResponse<string>> UpdateProjectOwnerAsync(Guid projectOwnerId, UserProfileUpdateDto projectOwnerUpdateDto);

        Task<ApiResponse<List<ResignationItemDto>>> GetResignations(Guid projectOwnerId);
    }
}
