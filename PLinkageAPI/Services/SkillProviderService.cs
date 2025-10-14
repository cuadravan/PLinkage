using MongoDB.Driver;
using PLinkageShared.Enums;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using PLinkageAPI.ValueObject;
using PLinkageShared.DTOs;
using PLinkageShared.ApiResponse;

namespace PLinkageAPI.Services
{
    public class SkillProviderService : ISkillProviderService
    {
        private readonly IRepository<SkillProvider> _skillProviderRepository;

        public SkillProviderService(IRepository<SkillProvider> repository)
        {
            _skillProviderRepository = repository;
        }

        public async Task<ApiResponse<SkillProvider?>> GetSpecificSkillProviderAsync(Guid skillProviderId)
        {
            var skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);

            if (skillProvider == null)
                return ApiResponse<SkillProvider?>.Fail($"Skill provider with ID {skillProviderId} not found.");

            return ApiResponse<SkillProvider?>.Ok(skillProvider, "Skill provider fetched successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateSkillProviderAsync(Guid skillProviderId, UserProfileUpdateDto updateDto)
        {
            try
            {
                var skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);
                if (skillProvider == null)
                    return ApiResponse<bool>.Fail($"Skill provider with ID {skillProviderId} not found.");

                skillProvider.UpdateProfile(updateDto);
                await _skillProviderRepository.UpdateAsync(skillProvider);

                return ApiResponse<bool>.Ok(true, "Skill provider updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error updating skill provider: {ex.Message}");
            }
        }

        // -------------- EDUCATIONS -----------------

        public async Task<ApiResponse<bool>> AddEducationAsync(Guid skillProviderId, Education education)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            sp.AddEducation(education);
            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Education added successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateEducationAsync(Guid skillProviderId, int index, Education updatedEducation)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            if (!sp.UpdateEducation(index, updatedEducation))
                return ApiResponse<bool>.Fail("Education index invalid or update failed.");

            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Education updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteEducationAsync(Guid skillProviderId, int index)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            if (!sp.DeleteEducation(index))
                return ApiResponse<bool>.Fail("Education index invalid or delete failed.");

            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Education deleted successfully.");
        }

        // -------------- SKILLS -----------------

        public async Task<ApiResponse<bool>> AddSkillAsync(Guid skillProviderId, Skill skill)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            sp.AddSkill(skill);
            await _skillProviderRepository.UpdateAsync(sp);

            return ApiResponse<bool>.Ok(true, "Skill added successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateSkillAsync(Guid skillProviderId, int index, Skill skill)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            if (!sp.UpdateSkill(index, skill))
                return ApiResponse<bool>.Fail("Skill index invalid or update failed.");

            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Skill updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteSkillAsync(Guid skillProviderId, int index)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            if (!sp.DeleteSkill(index))
                return ApiResponse<bool>.Fail("Skill index invalid or delete failed.");

            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Skill deleted successfully.");
        }

        // -------------- FILTERING -----------------

        public async Task<ApiResponse<IEnumerable<SkillProvider>>> GetFilteredSkillProvidersAsync(string proximity, CebuLocation? location, string status)
        {
            try
            {
                int range = proximity switch
                {
                    "Nearby (<= 10km)" => 10,
                    "Within Urban (<= 20km)" => 20,
                    "Extended (<= 50km)" => 50,
                    _ => 0
                };

                var builder = Builders<SkillProvider>.Filter;
                var filter = builder.Empty;

                if (status != "All")
                    filter &= builder.Eq(sp => sp.UserStatus, status);

                if (range > 0 && location.HasValue)
                {
                    var nearbyLocations = NearCebuLocations(location.Value, range);
                    filter &= builder.In(sp => sp.UserLocation, nearbyLocations);
                }
                else if (proximity == "By Specific Location" && location.HasValue)
                {
                    filter &= builder.Eq(sp => sp.UserLocation, location.Value);
                }

                var result = await _skillProviderRepository.FindAsync(filter);

                if (result == null || !result.Any())
                    return ApiResponse<IEnumerable<SkillProvider>>.Fail("No skill providers found matching the criteria.");

                return ApiResponse<IEnumerable<SkillProvider>>.Ok(result, "Filtered skill providers fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SkillProvider>>.Fail($"Error fetching skill providers: {ex.Message}");
            }
        }

        private List<CebuLocation?> NearCebuLocations(CebuLocation baseLocation, int threshold)
        {
            var nearby = new List<CebuLocation?>();
            var baseLoc = Location.From(baseLocation);

            foreach (var kvp in CebuLocationCoordinates.Map)
            {
                var otherLoc = Location.From(kvp.Key);
                double distance = baseLoc.DistanceTo(otherLoc);
                if (distance <= threshold)
                    nearby.Add(kvp.Key);
            }

            return nearby;
        }
    }
}
