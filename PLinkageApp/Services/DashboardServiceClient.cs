using System.Net.Http; // Add this using for HttpClient
using System.Threading.Tasks;
using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.Services
{
    public interface IDashboardServiceClient
    {
        Task<ApiResponse<SkillProviderDashboardDto>> GetSkillProviderDashboardAsync(Guid skillProviderId);
    }

    public class DashboardServiceClient : BaseApiClient, IDashboardServiceClient
    {
        public DashboardServiceClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<ApiResponse<SkillProviderDashboardDto>> GetSkillProviderDashboardAsync(Guid skillProviderId)
        {
            string url = $"api/dashboard/skillprovider/{skillProviderId}";
            return await GetAsync<SkillProviderDashboardDto>(url);
        }
    }
}