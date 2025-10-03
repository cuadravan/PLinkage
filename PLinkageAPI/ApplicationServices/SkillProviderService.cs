using PLinkageShared.Enums;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using PLinkageAPI.ValueObject;
using PLinkageAPI.Specifications;

namespace PLinkageAPI.ApplicationServices
{
    public class SkillProviderService : ISkillProviderService
    {
        private readonly ISkillProviderRepository _repository;

        public SkillProviderService(ISkillProviderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SkillProvider>> GetFilteredProvidersAsync(
            string proximity,
            CebuLocation? location,
            string status)
        {
            // 1. Business Logic: Determine the distance threshold
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

            var spec = new SkillProviderByStatusProximityLocationSpecification(
                proximity,
                location,
                status,
                nearbyLocations
            );

            return await _repository.FindAsync(spec);
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