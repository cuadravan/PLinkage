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

        public async Task<ApiResponse<IEnumerable<ProjectCardDto>>> GetFilteredProjectsAsync(
            string proximity,
            CebuLocation? location,
            string status)
        {
            if (proximity != "All" && proximity != "By Specific Location" && !location.HasValue)
            {
                return ApiResponse<IEnumerable<ProjectCardDto>>.Fail("A location must be provided for proximity searches.");
            }

            int range = proximity switch
            {
                "Nearby (<= 10km)" => 10,
                "Within Urban (<= 20km)" => 20,
                "Extended (<= 50km)" => 50,
                _ => 0
            };

            List<CebuLocation?> nearbyLocations = new();

            if (range > 0 && location.HasValue)
                nearbyLocations = NearCebuLocations(location.Value, range).ToList();

            var builder = Builders<Project>.Filter;
            var filter = builder.Empty;
            ProjectStatus projectStatus = ProjectStatus.Active;

            if (status != "All")
            {
                projectStatus = status switch
                {
                    "Active" => ProjectStatus.Active,
                    "Completed" => ProjectStatus.Completed,
                    "Deactivated" => ProjectStatus.Deactivated,
                    _ => ProjectStatus.Active
                };
                filter &= builder.Eq(sp => sp.ProjectStatus, projectStatus);
            }

            if (proximity == "By Specific Location" && location.HasValue)
                filter &= builder.Eq(sp => sp.ProjectLocation, location.Value);
            else if (proximity != "All" && location.HasValue && nearbyLocations.Any())
                filter &= builder.In(sp => sp.ProjectLocation, nearbyLocations);

            var projects = await _projectRepository.FindAsync(filter);

            if (projects == null || !projects.Any())
                return ApiResponse<IEnumerable<ProjectCardDto>>.Fail("No projects found matching the criteria.");

            Location? baseLoc = location.HasValue ? Location.From(location.Value) : null;

            var projectCardDtos = new List<ProjectCardDto>();
            foreach (var project in projects)
            {
                string locationString = project.ProjectLocation?.ToString() ?? "Location not set";

                if (baseLoc != null && project.ProjectLocation.HasValue)
                {
                    var projectLoc = Location.From(project.ProjectLocation.Value);

                    double distance = baseLoc.DistanceTo(projectLoc);

                    string formattedDistance = distance.ToString("F0");

                    locationString = $"{project.ProjectLocation.Value}, {formattedDistance} km away";
                }

                var projectCardDtoTemp = new ProjectCardDto
                {
                    Title = project.ProjectName,
                    Slots = project.ProjectResourcesAvailable.ToString() + " slot/s",
                    Location = locationString, 
                    Description = project.ProjectDescription,
                    Skills = project.ProjectSkillsRequired
                };

                projectCardDtos.Add(projectCardDtoTemp);
            }

            return ApiResponse<IEnumerable<ProjectCardDto>>.Ok(projectCardDtos);
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
