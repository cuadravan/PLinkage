using System.Net.Http; // Add this using for HttpClient
using System.Threading.Tasks;
using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageApp.Services
{
    public interface IProjectServiceClient
    {
        Task<ApiResponse<IEnumerable<ProjectCardDto>>> GetFilteredProjectsAsync(string proximity, CebuLocation? location, string status);
    }

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
    }
}