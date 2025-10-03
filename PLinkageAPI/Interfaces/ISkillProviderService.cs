using PLinkageAPI.Entities;
using PLinkageShared.Enums;

namespace PLinkageAPI.Interfaces
{
    public interface ISkillProviderService
    {
        Task<IEnumerable<SkillProvider>> GetFilteredProvidersAsync(
            string proximity,
            CebuLocation? location,
            string status);
    }
}