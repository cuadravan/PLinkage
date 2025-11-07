using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using PLinkageApp.Interfaces;

namespace PLinkageApp.Services
{
    public class ProjectServiceClient : BaseApiClient, IProjectServiceClient
    {
        public ProjectServiceClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<ApiResponse<IEnumerable<ProjectCardDto>>> GetFilteredProjectsAsync(string proximity, CebuLocation? location, string status)
        {
            string url = "api/project";

            var queryParams = new List<string>
        {
            $"proximity={proximity}",
            $"status={status}"
        };

            // 3. Add location only if it has a value.
            if (location.HasValue)
            {
                queryParams.Add($"location={(int)location.Value}");
            }

            string finalUrl = $"{url}?{string.Join("&", queryParams)}";

            return await GetAsync<IEnumerable<ProjectCardDto>>(finalUrl);
        }

        public async Task<ApiResponse<ProjectDto>> GetSpecificAsync(Guid projectId)
        {
            return await GetAsync<ProjectDto>($"api/project/{projectId}");
        }

        public async Task<ApiResponse<Guid>> AddProjectAsync(ProjectCreationDto projectCreationDto)
        {
            return await PostAsync<ProjectCreationDto, Guid>("api/project", projectCreationDto);
        }
        public async Task<ApiResponse<bool>> UpdateProjectAsync(ProjectUpdateDto projectUpdateDto)
        {
            return await PatchAsync<ProjectUpdateDto, bool>($"api/project/{projectUpdateDto.ProjectId}", projectUpdateDto);
        }

        public async Task<ApiResponse<bool>> RequestResignationAsync(RequestResignationDto requestResignationDto)
        {
            return await PostAsync<RequestResignationDto, bool>("api/project/requestresignation", requestResignationDto);
        }

        public async Task<ApiResponse<bool>> ProcessResignationAsync(ProcessResignationDto processResignationDto)
        {
            return await PostAsync<ProcessResignationDto, bool>("api/project/processresignation", processResignationDto);
        }

        public async Task<ApiResponse<bool>> RateSkillProvidersAsync(RateSkillProviderDto rateSkillProviderDto)
        {
            return await PatchAsync<RateSkillProviderDto, bool>("api/project/ratings", rateSkillProviderDto);
        }
    }
}