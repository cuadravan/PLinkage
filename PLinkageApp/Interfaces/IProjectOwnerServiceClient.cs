using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.Interfaces
{
    public interface IProjectOwnerServiceClient
    {
        Task<ApiResponse<IEnumerable<ProjectOwnerCardDto>>> GetFilteredProjectOwnersAsync(string proximity, CebuLocation? location, string status);
        Task<ApiResponse<ProjectOwnerDto>> GetSpecificAsync(Guid projectOwnerId);
    }

}
