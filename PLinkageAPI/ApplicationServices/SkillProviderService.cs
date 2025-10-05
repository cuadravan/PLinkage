using PLinkageShared.Enums;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using System.Linq.Expressions;
using MongoDB.Driver;
using PLinkageAPI.ValueObject;
using PLinkageShared.DTOs;

namespace PLinkageAPI.ApplicationServices
{
    public class SkillProviderService : ISkillProviderService
    {
        private readonly IRepository<SkillProvider> _skillProviderRepository;

        public SkillProviderService(IRepository<SkillProvider> repository)
        {
            _skillProviderRepository = repository;
        }

        public async Task<SkillProvider?> GetSpecificSkillProviderAsync(Guid skillProviderId)
        {
            return await _skillProviderRepository.GetByIdAsync(skillProviderId);
        }

        public async Task<bool> UpdateSkillProviderAsync(Guid skillProviderId, SkillProviderUpdateDto skillProviderUpdateDto)
        {
            if (!await _skillProviderRepository.ExistsAsync(skillProviderId))
            {
                return false;
            }
            SkillProvider? skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);

            if (skillProvider == null)
            {
                return false;
            }

            skillProvider.UpdateProfile(skillProviderUpdateDto);

            await _skillProviderRepository.UpdateAsync(skillProvider);
            return true;
        }

        public async Task<bool> AddEducationAsync(Guid skillProviderId, Education educationToAdd)
        {
            if (!await _skillProviderRepository.ExistsAsync(skillProviderId))
            {
                return false;
            }

            SkillProvider? skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);

            if(skillProvider == null)
            {
                return false;
            }

            skillProvider.AddEducation(educationToAdd);

            await _skillProviderRepository.UpdateAsync(skillProvider);
            return true;
        }

        public async Task<bool> UpdateEducationAsync(Guid skillProviderId, int indexToUpdate, Education educationToUpdate)
        {
            if (!await _skillProviderRepository.ExistsAsync(skillProviderId))
            {
                return false;
            }

            SkillProvider? skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);

            if (skillProvider == null)
            {
                return false;
            }

            skillProvider.UpdateEducation(indexToUpdate, educationToUpdate);

            await _skillProviderRepository.UpdateAsync(skillProvider);
            return true;
        }

        public async Task<bool> DeleteEducationAsync(Guid skillProviderId, int indexToDelete)
        {
            if (!await _skillProviderRepository.ExistsAsync(skillProviderId))
            {
                return false;
            }

            SkillProvider? skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);

            if (skillProvider == null)
            {
                return false;
            }

            try { skillProvider.DeleteEducation(indexToDelete); }
            catch(InvalidOperationException ex)
            {
                throw new Exception("Request cannot be fulfilled due to invalid index.");
            }

            await _skillProviderRepository.UpdateAsync(skillProvider);
            return true;
        }

        public async Task<bool> AddSkillAsync(Guid skillProviderId, Skill skillToAdd)
        {
            if (!await _skillProviderRepository.ExistsAsync(skillProviderId))
            {
                return false;
            }

            SkillProvider? skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);

            if (skillProvider == null)
            {
                return false;
            }

            skillProvider.AddSkill(skillToAdd);

            await _skillProviderRepository.UpdateAsync(skillProvider);
            return true;
        }

        public async Task<bool> UpdateSkillAsync(Guid skillProviderId, int indexToUpdate, Skill skillToUpdate)
        {
            if (!await _skillProviderRepository.ExistsAsync(skillProviderId))
            {
                return false;
            }

            SkillProvider? skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);

            if (skillProvider == null)
            {
                return false;
            }

            skillProvider.UpdateSkill(indexToUpdate, skillToUpdate);

            await _skillProviderRepository.UpdateAsync(skillProvider);
            return true;
        }

        public async Task<bool> DeleteSkillAsync(Guid skillProviderId, int indexToDelete)
        {
            if (!await _skillProviderRepository.ExistsAsync(skillProviderId))
            {
                return false;
            }

            SkillProvider? skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);

            if (skillProvider == null)
            {
                return false;
            }

            try { skillProvider.DeleteSkill(indexToDelete); }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Request cannot be fulfilled due to invalid index.");
            }

            await _skillProviderRepository.UpdateAsync(skillProvider);
            return true;
        }

        public async Task<IEnumerable<SkillProvider>> GetFilteredSkillProvidersAsync(string proximity, CebuLocation? location, string status)
        {
            int range = proximity switch
            {
                "Nearby (<= 10km)" => 10,
                "Within Urban (<= 20km)" => 20,
                "Extended (<= 50km)" => 50,
                _ => 0
            };

            List<CebuLocation?> nearbyLocations = new();

            if (range > 0 && location.HasValue)
            {
                nearbyLocations = NearCebuLocations(location.Value, range).ToList();
            }

            var builder = Builders<SkillProvider>.Filter;
            var filter = builder.Empty; // equivalent to 'true'

            // Status filter
            if (status != "All")
            {
                filter &= builder.Eq(sp => sp.UserStatus, status);
            }

            // Proximity filter
            if (proximity == "By Specific Location" && location.HasValue)
            {
                filter &= builder.Eq(sp => sp.UserLocation, location.Value);
            }
            else if (proximity != "All" && location.HasValue && nearbyLocations.Any())
            {
                filter &= builder.In(sp => sp.UserLocation, nearbyLocations);
            }

            // pass pure expression to repository
            return await _skillProviderRepository.FindAsync(filter);
        }


        private List<CebuLocation?> NearCebuLocations(CebuLocation baseLocation, int threshold)
        {
            List<CebuLocation?> nearby = new();

            var baseLoc = Location.From(baseLocation); // Factory method from ValueObject

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