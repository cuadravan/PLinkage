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

        public async Task<ApiResponse<IEnumerable<ProjectOwner>>> GetFilteredProjectOwnerAsync(
            string proximity,
            CebuLocation? location,
            string status)
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

                List<CebuLocation?> nearbyLocations = new();

                if (range > 0 && location.HasValue)
                {
                    nearbyLocations = NearCebuLocations(location.Value, range).ToList();
                }

                var builder = Builders<ProjectOwner>.Filter;
                var filter = builder.Empty;

                if (status != "All")
                    filter &= builder.Eq(sp => sp.UserStatus, status);

                if (proximity == "By Specific Location" && location.HasValue)
                    filter &= builder.Eq(sp => sp.UserLocation, location.Value);
                else if (proximity != "All" && location.HasValue && nearbyLocations.Any())
                    filter &= builder.In(sp => sp.UserLocation, nearbyLocations);

                var filtered = await _projectOwnerRepository.FindAsync(filter);

                if (filtered == null || !filtered.Any())
                    return ApiResponse<IEnumerable<ProjectOwner>>.Fail("No project owner found matching the criteria.");

                return ApiResponse<IEnumerable<ProjectOwner>>.Ok(filtered, "Filtered project owners fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ProjectOwner>>.Fail($"An error occurred while filtering project owners. {ex.Message}");
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

        private List<CebuLocation?> NearCebuLocations(CebuLocation baseLocation, int threshold)
        {
            List<CebuLocation?> nearby = new();

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
