using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageAPI.Interfaces
{
    public interface IDashboardService
    {
        Task<ApiResponse<ProjectOwnerDashboardDto>> GetProjectOwnerDashboardAsync(Guid projectOwnerId);
        Task<ApiResponse<SkillProviderDashboardDto>> GetSkillProviderDashboardAsync(Guid skillProviderId);
        Task<ApiResponse<AdminDashboardDto>> GetAdminDashboardAsync(Guid adminId);
    }
}
