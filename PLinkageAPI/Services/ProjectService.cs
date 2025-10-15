using MongoDB.Driver;
using PLinkageAPI.Entities;
using PLinkageAPI.Interfaces;
using PLinkageAPI.ValueObject;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<ProjectOwner> _projectOwnerRepository;

        public ProjectService(IRepository<Project> projectRepository, IRepository<ProjectOwner> projectOwnerRepository)
        {
            _projectRepository = projectRepository;
            _projectOwnerRepository = projectOwnerRepository;
        }

        public async Task<ApiResponse<bool>> AddProjectAsync(ProjectDto projectCreationDto)
        {
            ProjectOwner? projectOwner = await _projectOwnerRepository.GetByIdAsync(projectCreationDto.ProjectOwnerId);
            if (projectOwner == null)
                return ApiResponse<bool>.Fail($"Project Owner with ID {projectCreationDto.ProjectOwnerId} not found.");

            projectOwner.AddProject(projectCreationDto.ProjectId);

            Project newProject = new Project
            {
                ProjectId = projectCreationDto.ProjectId,
                ProjectOwnerId = projectCreationDto.ProjectOwnerId,
                ProjectName = projectCreationDto.ProjectName,
                ProjectLocation = projectCreationDto.ProjectLocation,
                ProjectDescription = projectCreationDto.ProjectDescription,
                ProjectStartDate = projectCreationDto.ProjectStartDate,
                ProjectEndDate = projectCreationDto.ProjectEndDate,
                ProjectStatus = projectCreationDto.ProjectStatus,
                ProjectSkillsRequired = projectCreationDto.ProjectSkillsRequired,
                ProjectMembers = projectCreationDto.ProjectMembers,
                ProjectPriority = projectCreationDto.ProjectPriority,
                ProjectResourcesNeeded = projectCreationDto.ProjectResourcesNeeded,
                ProjectResourcesAvailable = projectCreationDto.ProjectResourcesNeeded,
                ProjectDateCreated = projectCreationDto.ProjectDateCreated,
                ProjectDateUpdated = projectCreationDto.ProjectDateUpdated
            };

            await _projectOwnerRepository.UpdateAsync(projectOwner);
            await _projectRepository.AddAsync(newProject);

            return ApiResponse<bool>.Ok(true, "Project successfully created.");
        }

        public async Task<ApiResponse<bool>> UpdateProjectAsync(ProjectUpdateDto projectUpdateDto)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectUpdateDto.ProjectId);
            if (project == null)
                return ApiResponse<bool>.Fail($"Project with ID {projectUpdateDto.ProjectId} not found.");

            project.UpdateProject(projectUpdateDto);
            await _projectRepository.UpdateAsync(project);

            return ApiResponse<bool>.Ok(true, "Project updated successfully.");
        }

        public async Task<ApiResponse<Project>> GetSpecificProjectAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return ApiResponse<Project>.Fail($"Project with ID {projectId} not found.");

            return ApiResponse<Project>.Ok(project);
        }

        private static readonly Dictionary<string, int> ProximityRanges = new()
            {
                { "Nearby (<= 10km)", 10 },
                { "Within Urban (<= 20km)", 20 },
                { "Extended (<= 50km)", 50 }
            };

        public async Task<ApiResponse<IEnumerable<ProjectCardDto>>> GetFilteredProjectsAsync(
            string proximity,
            CebuLocation? location,
            string status)
        {
            if (proximity != "All" && proximity != "By Specific Location" && !location.HasValue)
            {
                return ApiResponse<IEnumerable<ProjectCardDto>>.Fail("A location must be provided for proximity searches.");
            }

            ProximityRanges.TryGetValue(proximity, out var range);

            var nearbyLocationsWithDistance = new Dictionary<CebuLocation, double>();
            if (range > 0 && location.HasValue)
            {
                nearbyLocationsWithDistance = GetNearbyLocationsWithDistance(location.Value, range);
            }

            var builder = Builders<Project>.Filter;
            var filter = builder.Empty;

            if (status != "All" && Enum.TryParse<ProjectStatus>(status, true, out var projectStatus))
            {
                filter &= builder.Eq(p => p.ProjectStatus, projectStatus);
            }

            if (proximity == "By Specific Location" && location.HasValue)
            {
                filter &= builder.Eq(p => p.ProjectLocation, location.Value);
            }
            else if (nearbyLocationsWithDistance.Any())
            {
                filter &= builder.In(p => p.ProjectLocation, nearbyLocationsWithDistance.Keys.Cast<CebuLocation?>());
            }

            var projects = await _projectRepository.FindAsync(filter);

            if (projects == null || !projects.Any())
            {
                return ApiResponse<IEnumerable<ProjectCardDto>>.Fail("No projects found matching the criteria.");
            }

            var projectCardDtos = projects.Select(project =>
            {
                string locationString = project.ProjectLocation?.ToString() ?? "Location not set";

                if (location.HasValue && project.ProjectLocation.HasValue)
                {
                    if (nearbyLocationsWithDistance.TryGetValue(project.ProjectLocation.Value, out double distance))
                    {
                        locationString = $"{project.ProjectLocation.Value}, {distance:F0} km away";
                    }
                    else if (!nearbyLocationsWithDistance.Any())
                    {
                        var baseLoc = Location.From(location.Value);
                        var projectLoc = Location.From(project.ProjectLocation.Value);
                        distance = baseLoc.DistanceTo(projectLoc);
                        locationString = $"{project.ProjectLocation.Value}, {distance:F0} km away";
                    }
                }

                return new ProjectCardDto
                {
                    Title = project.ProjectName,
                    Slots = $"{project.ProjectResourcesAvailable} slot/s",
                    Location = locationString,
                    Description = project.ProjectDescription,
                    Skills = project.ProjectSkillsRequired?
    .Take(5)
    .ToList() ?? new List<string>()
                };
            });

            return ApiResponse<IEnumerable<ProjectCardDto>>.Ok(projectCardDtos);
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
