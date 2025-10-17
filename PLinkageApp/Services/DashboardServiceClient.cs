using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageApp.Interfaces;

namespace PLinkageApp.Services
{
    public class DashboardServiceClient : BaseApiClient, IDashboardServiceClient
    {
        public DashboardServiceClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<ApiResponse<SkillProviderDashboardDto>> GetSkillProviderDashboardAsync(Guid skillProviderId)
        {
            string url = $"api/dashboard/skillprovider/{skillProviderId}";
            return await GetAsync<SkillProviderDashboardDto>(url);
        }

        public async Task<ApiResponse<ProjectOwnerDashboardDto>> GetProjectOwnerDashboardAsync(Guid projectOwnerId)
        {
            string url = $"api/dashboard/projectowner/{projectOwnerId}";
            return await GetAsync<ProjectOwnerDashboardDto>(url);
        }

        public async Task<ApiResponse<AdminDashboardDto>> GetAdminDashboardAsync(Guid adminId)
        {
            string url = $"api/dashboard/admin/{adminId}";
            return await GetAsync<AdminDashboardDto>(url);
        }
    }
}