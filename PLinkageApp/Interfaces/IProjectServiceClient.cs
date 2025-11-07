using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageApp.Interfaces
{
    public interface IProjectServiceClient
    {
        Task<ApiResponse<IEnumerable<ProjectCardDto>>> GetFilteredProjectsAsync(string proximity, CebuLocation? location, string status);
        Task<ApiResponse<ProjectDto>> GetSpecificAsync(Guid projectId);

        Task<ApiResponse<Guid>> AddProjectAsync(ProjectCreationDto projectCreationDto);
        Task<ApiResponse<bool>> UpdateProjectAsync(ProjectUpdateDto projectUpdateDto);

        Task<ApiResponse<bool>> RequestResignationAsync(RequestResignationDto requestResignationDto);

        Task<ApiResponse<bool>> ProcessResignationAsync(ProcessResignationDto processResignationDto);

        Task<ApiResponse<bool>> RateSkillProvidersAsync(RateSkillProviderDto rateSkillProviderDto);
    }

}
