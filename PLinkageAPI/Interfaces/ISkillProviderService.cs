// File: ILinkageAPI.Interfaces/ISkillProviderService.cs

using PLinkageAPI.Models;
using PLinkageShared.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLinkageAPI.Interfaces
{
    public interface ISkillProviderService
    {
        Task<IEnumerable<SkillProvider>> GetFilteredProvidersAsync(
            string proximity,
            CebuLocation? location,
            string status);

        // Include other service methods as needed (e.g., GetById, Add, etc.)
    }
}