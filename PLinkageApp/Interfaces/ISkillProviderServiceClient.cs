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
    public interface ISkillProviderServiceClient
    {
        Task<ApiResponse<IEnumerable<SkillProviderCardDto>>> GetFilteredSkillProvidersAsync(string proximity, CebuLocation? location, string status, bool? isEmployed);
        Task<ApiResponse<SkillProviderDto>> GetSpecificAsync(Guid skillProviderId);
    }
}
