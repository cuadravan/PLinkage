using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.Interfaces
{
    public interface IDashboardServiceClient
    {
        Task<ApiResponse<SkillProviderDashboardDto>> GetSkillProviderDashboardAsync(Guid skillProviderId);
        Task<ApiResponse<ProjectOwnerDashboardDto>> GetProjectOwnerDashboardAsync(Guid projectOwnerId);
        Task<ApiResponse<AdminDashboardDto>> GetAdminDashboardAsync(Guid adminId);
    }
}
