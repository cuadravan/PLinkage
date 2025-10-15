using MongoDB.Driver;
using PLinkageAPI.Entities;
using PLinkageAPI.Interfaces;
using PLinkageAPI.ValueObject;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageAPI.Services
{
    public class ProjectOwnerService : IProjectOwnerService
    {
        private readonly IRepository<ProjectOwner> _projectOwnerRepository;

        public ProjectOwnerService(IRepository<ProjectOwner> repository)
        {
            _projectOwnerRepository = repository;
        }

        public async Task<ApiResponse<ProjectOwner?>> GetSpecificProjectOwnerAsync(Guid projectOwnerId)
        {
            var projectOwner = await _projectOwnerRepository.GetByIdAsync(projectOwnerId);

            if (projectOwner == null)
                return ApiResponse<ProjectOwner?>.Fail("Requested project owner with ID not found.");

            return ApiResponse<ProjectOwner?>.Ok(projectOwner, "Project owner fetched successfully.");
        }

        private static readonly Dictionary<string, int> ProximityRanges = new()
            {
                { "Nearby (<= 10km)", 10 },
                { "Within Urban (<= 20km)", 20 },
                { "Extended (<= 50km)", 50 }
            };

        public async Task<ApiResponse<IEnumerable<ProjectOwnerCardDto>>> GetFilteredProjectOwnerAsync(
    string proximity,
    CebuLocation? location,
    string status)
        {
            try
            {
                if (proximity != "All" && proximity != "By Specific Location" && !location.HasValue)
                {
                    return ApiResponse<IEnumerable<ProjectOwnerCardDto>>.Fail("A location must be provided for proximity searches.");
                }

                ProximityRanges.TryGetValue(proximity, out var range);

                var nearbyLocationsWithDistance = new Dictionary<CebuLocation, double>();
                if (range > 0 && location.HasValue)
                {
                    nearbyLocationsWithDistance = GetNearbyLocationsWithDistance(location.Value, range);
                }

                var builder = Builders<ProjectOwner>.Filter;
                var filter = builder.Empty;

                if (status != "All")
                {
                    filter &= builder.Eq(po => po.UserStatus, status);
                }

                if (proximity == "By Specific Location" && location.HasValue)
                {
                    filter &= builder.Eq(po => po.UserLocation, location.Value);
                }
                else if (nearbyLocationsWithDistance.Any())
                {
                    filter &= builder.In(po => po.UserLocation, nearbyLocationsWithDistance.Keys.Cast<CebuLocation?>());
                }

                var projectOwners = await _projectOwnerRepository.FindAsync(filter);

                if (projectOwners == null || !projectOwners.Any())
                {
                    return ApiResponse<IEnumerable<ProjectOwnerCardDto>>.Fail("No project owners found matching the criteria.");
                }

                var projectOwnerCardDtos = projectOwners.Select(po => new ProjectOwnerCardDto
                {
                    UserName = po.UserFirstName + " " + po.UserLastName,
                    UserStatus = po.UserStatus,
                    ProjectCount = "Has " + po.OwnedProjectId.Count().ToString() + " project/s"
                });

                return ApiResponse<IEnumerable<ProjectOwnerCardDto>>.Ok(projectOwnerCardDtos, "Filtered project owners fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ProjectOwnerCardDto>>.Fail($"An error occurred while filtering project owners: {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> UpdateProjectOwnerAsync(Guid projectOwnerId, UserProfileUpdateDto projectOwnerUpdateDto)
        {
            try
            {
                ProjectOwner? projectOwner = await _projectOwnerRepository.GetByIdAsync(projectOwnerId);

                if (projectOwner == null)
                    return ApiResponse<string>.Fail($"Project owner with ID {projectOwnerId} not found.");

                projectOwner.UpdateProfile(projectOwnerUpdateDto);

                await _projectOwnerRepository.UpdateAsync(projectOwner);
                return ApiResponse<string>.Ok(null, "Project owner updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail($"An error occurred while updating project owner. {ex.Message}");
            }
        }

        private Dictionary<CebuLocation, double> GetNearbyLocationsWithDistance(CebuLocation baseLocation, int threshold)
        {
            var nearby = new Dictionary<CebuLocation, double>();
            var baseLoc = Location.From(baseLocation);

            foreach (var (locationEnum, coordinates) in CebuLocationCoordinates.Map)
            {
                var otherLoc = new Location(coordinates.Latitude, coordinates.Longitude);
                double distance = baseLoc.DistanceTo(otherLoc);
                if (distance <= threshold)
                {
                    nearby[locationEnum] = distance;
                }
            }

            return nearby;
        }
    }
}
