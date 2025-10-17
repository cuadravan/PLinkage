using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using PLinkageApp.Interfaces;

namespace PLinkageApp.Services
{
    public class ProjectOwnerServiceClient : BaseApiClient, IProjectOwnerServiceClient
    {
        public ProjectOwnerServiceClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<ApiResponse<IEnumerable<ProjectOwnerCardDto>>> GetFilteredProjectOwnersAsync(string proximity, CebuLocation? location, string status)
        {
            string url = "api/projectowner";

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

            return await GetAsync<IEnumerable<ProjectOwnerCardDto>>(finalUrl);
        }
    }
}