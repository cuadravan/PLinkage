using PLinkageShared.Enums;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using PLinkageAPI.Specifications;
using System.Linq.Expressions;
using MongoDB.Driver;
using PLinkageAPI.ValueObject;

namespace PLinkageAPI.ApplicationServices
{
    public class SkillProviderService : ISkillProviderService
    {
        private readonly IRepository<SkillProvider> _repository;

        public SkillProviderService(IRepository<SkillProvider> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SkillProvider>> GetFilteredProvidersAsync(string proximity, CebuLocation? location, string status)
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
            return await _repository.FindAsync(filter);
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