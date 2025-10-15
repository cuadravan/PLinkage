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
        Task<ApiResponse<ProjectOwnerDashboardDto>> GetProjectOwnerDashboardAsync(Guid projectOwnerId);
        Task<ApiResponse<AdminDashboardDto>> GetAdminDashboardAsync(Guid adminId);
    }

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