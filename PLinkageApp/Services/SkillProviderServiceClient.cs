using System.Net.Http; // Add this using for HttpClient
using System.Threading.Tasks;
using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using PLinkageApp.Interfaces;

namespace PLinkageApp.Services
{
    public class SkillProviderServiceClient : BaseApiClient, ISkillProviderServiceClient
    {
        public SkillProviderServiceClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<ApiResponse<IEnumerable<SkillProviderCardDto>>> GetFilteredSkillProvidersAsync(string proximity, CebuLocation? location, string status, bool? isEmployed)
        {
            string url = "api/skillprovider";

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

            if (isEmployed.HasValue)
            {
                queryParams.Add($"isEmployed={isEmployed}");
            }

            string finalUrl = $"{url}?{string.Join("&", queryParams)}";

            return await GetAsync<IEnumerable<SkillProviderCardDto>>(finalUrl);
        }

        public async Task<ApiResponse<SkillProviderDto>> GetSpecificAsync(Guid skillProviderId)
        {
            return await GetAsync<SkillProviderDto>($"api/skillprovider/{skillProviderId}");
        }
    }
}