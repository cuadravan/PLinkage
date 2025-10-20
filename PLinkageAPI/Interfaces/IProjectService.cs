using PLinkageAPI.Entities;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageAPI.Interfaces
{
    public interface IProjectService
    {
        Task<ApiResponse<Project>> GetSpecificProjectAsync(Guid projectId);

        Task<ApiResponse<IEnumerable<ProjectCardDto>>> GetFilteredProjectsAsync(
            string proximity,
            CebuLocation? location,
            string status);

        Task<ApiResponse<Guid>> AddProjectAsync(ProjectCreationDto projectCreationDto);
        Task<ApiResponse<bool>> UpdateProjectAsync(ProjectUpdateDto projectUpdateDto);

        Task<ApiResponse<bool>> RequestResignation(RequestResignationDto requestResignationDto);

        Task<ApiResponse<bool>> ProcessResignation(ProcessResignationDto processResignationDto);

        Task<ApiResponse<bool>> RateSkillProviders(RateSkillProviderDto rateSkillProviderDto);
    }
}
