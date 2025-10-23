using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageApp.Interfaces
{
    public interface IProjectServiceClient
    {
        Task<ApiResponse<IEnumerable<ProjectCardDto>>> GetFilteredProjectsAsync(string proximity, CebuLocation? location, string status);
        Task<ApiResponse<ProjectDto>> GetSpecificAsync(Guid projectId);
    }

}
