// File: PLinkageAPI.Services/SkillProviderService.cs

using PLinkageAPI.Interfaces;
using PLinkageAPI.Models;
using PLinkageAPI.Specifications;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLinkageAPI.Services
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
            // 1. Business Logic: Determine the list of nearby locations
            List<CebuLocation?> nearbyLocations = new List<CebuLocation?>();
            int range = 0;

            if (proximity == "Nearby (<= 10km)") range = 10;
            else if (proximity == "Within Urban (<= 20km)") range = 20;
            else if (proximity == "Extended (<= 50km)") range = 50;

            if (range > 0 && location.HasValue)
            {
                // Execute the location calculation
                nearbyLocations = this.nearCebuLocations(location.Value, range).ToList();
            }

            // 2. Instantiate the Specification using the calculated data
            var spec = new SkillProviderByStatusProximityLocationSpecification(
                proximity,
                location,
                status,
                nearbyLocations // Pass the calculated result to the specification
            );

            // 3. Call the Repository with the Specification
            return await _repository.FindAsync(spec);
        }

        // =========================================================================
        // Private Geographical Calculation Methods (Moved from Specification)
        // These methods perform business-level calculation and belong here.
        // =========================================================================

        private List<CebuLocation?> nearCebuLocations(CebuLocation location, int threshold)
        {
            List<CebuLocation?> nearbyLocations = new List<CebuLocation?>();
            // Assuming CebuLocationCoordinates.Map is accessible (or injected)
            var (lat1, lon1) = CebuLocationCoordinates.Map[location];

            foreach (var loc in CebuLocationCoordinates.Map)
            {
                var (lat2, lon2) = loc.Value;
                double distance = CalculateDistance(lat1, lon1, lat2, lon2);
                if (distance <= threshold)
                {
                    nearbyLocations.Add(loc.Key);
                }
            }
            return nearbyLocations;
        }

        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers
            lat1 = ToRadians(lat1);
            lon1 = ToRadians(lon1);
            lat2 = ToRadians(lat2);
            lon2 = ToRadians(lon2);

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double angleInDegrees)
        {
            return angleInDegrees * (Math.PI / 180);
        }
    }
}