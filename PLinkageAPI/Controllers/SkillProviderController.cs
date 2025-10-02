using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Models;
using PLinkageAPI.Repository;
using MongoDB.Driver;

namespace PLinkageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillProviderController : ControllerBase
    {
        private readonly SkillProviderRepository _skillProviders;
        public SkillProviderController(SkillProviderRepository skillProviders)
        {
            _skillProviders = skillProviders;
        }
        // GET: api/SkillProvider/filter?proximity=Same Place as Me&location=0
        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string? proximity = "All",
            [FromQuery] CebuLocation? location = null)
        {
            List<CebuLocation?> nearbyLocations = new List<CebuLocation?>();
            if (proximity == "Nearby (<= 10km)" && location != null)
            {
                nearbyLocations = nearCebuLocations(location.Value, 10).ToList();
            }
            else if (proximity == "Within Urban (<= 20km)" && location != null)
            {
                nearbyLocations = nearCebuLocations(location.Value, 20).ToList();
            }
            else if (proximity == "Extended (<= 50km)" && location != null)
            {
                nearbyLocations = nearCebuLocations(location.Value, 50).ToList();
            }

            // Build MongoDB filters sequentially
            // This is the MongoDB Builders<T> syntax for making filters
            var filter = Builders<SkillProvider>.Filter.Empty; // start with no filter
            // Proximity filter
            if (proximity == "Same Place as Me" && location.HasValue)
                filter &= Builders<SkillProvider>.Filter.Eq(sp => sp.UserLocation, location.Value);
            else if (proximity != "All" && location.HasValue)
            {
                filter &= Builders<SkillProvider>.Filter.In(sp => sp.UserLocation, nearbyLocations);
            }
            // Apply filter
            var filteredSkillProviders = await _skillProviders.FilterAsync(filter);
            return Ok(filteredSkillProviders);
        }

        private ICollection<CebuLocation?> nearCebuLocations(CebuLocation location, int threshold)
        {
            List<CebuLocation?> nearbyLocations = new List<CebuLocation?>();
            var (lat1, lon1) = CebuLocationCoordinates.Map[location];
            foreach (var loc in CebuLocationCoordinates.Map)
            {
                var (lat2, lon2) = loc.Value;
                double distance = CalculateDistance(lat1, lon1, lat2, lon2);
                if (distance <= threshold) // within threshold
                {
                    nearbyLocations.Add(loc.Key);
                }
            }
            return nearbyLocations;
        }

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers

            // Convert latitudes and longitudes from degrees to radians
            lat1 = ToRadians(lat1);
            lon1 = ToRadians(lon1);
            lat2 = ToRadians(lat2);
            lon2 = ToRadians(lon2);

            // Calculate the differences in coordinates
            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            // Apply the Haversine formula
            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = R * c; // Distance in kilometers

            return distance;
        }

        private static double ToRadians(double angleInDegrees)
        {
            return angleInDegrees * (Math.PI / 180);
        }
    }
}
