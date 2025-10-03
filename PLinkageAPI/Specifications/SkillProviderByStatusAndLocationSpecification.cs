using PLinkageAPI.Entities;
using System.Linq.Expressions;
using PLinkageShared.Enums;
using System.Collections.Generic;
using System.Linq;

namespace PLinkageAPI.Specifications
{
    // Assuming you have a class/library that provides expression combining features (like LinqKit)
    // or you use a simple '&&' operator, which often works for simple LINQ.

    public class SkillProviderByStatusProximityLocationSpecification : BaseSpecification<SkillProvider>
    {
        // Constructor now ONLY accepts the result of the complex location calculation, 
        // the required location, and the status.
        public SkillProviderByStatusProximityLocationSpecification(
            string proximity,
            CebuLocation? userLocation,
            string status,
            List<CebuLocation?> nearbyLocations) // <-- The pre-calculated list is essential

            : base(BuildCriteria(proximity, userLocation, status, nearbyLocations))
        {
            // The location and distance calculation MUST happen in the calling Service/Controller.
        }

        // Static method to generate the final combined Expression<Func<T, bool>>
        private static Expression<Func<SkillProvider, bool>> BuildCriteria(
            string proximity,
            CebuLocation? userLocation,
            string status,
            List<CebuLocation?> nearbyLocations)
        {
            // 1. Start with the Status Filter (Assuming 'status' maps to a property like 'CurrentStatus')
            Expression<Func<SkillProvider, bool>> filter = sp => true;

            if (status != "All")
            {
                // Simple filter by status
                filter = sp => sp.UserStatus.Equals(status);
            }

            // 2. Apply Proximity Filter, combined with the Status Filter.
            if (proximity == "By Specific Location" && userLocation.HasValue)
            {
                // AND operation: status filter && location filter
                // Note: If you don't have an external Expression Combiner, using '&&' directly
                // like this is complex. For simplicity, we'll assume a clean override or use '&&'.

                // For demonstration, we'll use a clean filter logic that assumes the first filter (status)
                // is applied implicitly by starting from 'sp => true'.

                if (status != "All")
                {
                    // This is where you need Expression combining logic.
                    // Since that's complicated, the safest approach for your simple setup is sequential:
                    filter = sp => sp.UserStatus.Equals(status) && sp.UserLocation.Equals(userLocation.Value);
                }
                else
                {
                    filter = sp => sp.UserLocation.Equals(userLocation.Value);
                }
            }
            else if (proximity != "All" && userLocation.HasValue)
            {
                // Filter by a list of locations (using Contains for MongoDB $in)
                if (status != "All")
                {
                    filter = sp => sp.UserStatus.Equals(status) && nearbyLocations.Contains(sp.UserLocation);
                }
                else
                {
                    filter = sp => nearbyLocations.Contains(sp.UserLocation);
                }
            }

            // If the filter remains 'sp => true', then all skill providers are returned.

            return filter;
        }
    }
}