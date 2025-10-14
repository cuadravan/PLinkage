using PLinkageAPI.Entities;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageAPI.Interfaces
{
    public interface IProjectService
    {
        Task<ApiResponse<Project>> GetSpecificProjectAsync(Guid projectId);

        Task<ApiResponse<IEnumerable<Project>>> GetFilteredProjectsAsync(
            string proximity,
            CebuLocation? location,
            string status);

        Task<ApiResponse<bool>> AddProjectAsync(ProjectDto projectDto);
        Task<ApiResponse<bool>> UpdateProjectAsync(ProjectUpdateDto projectUpdateDto);
    }
}
