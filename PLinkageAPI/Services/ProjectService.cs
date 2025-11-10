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
        private readonly IRepository<SkillProvider> _skillProviderRepository;
        private readonly IMongoClient _mongoClient;

        public ProjectService(IMongoClient mongoClient, IRepository<Project> projectRepository, IRepository<ProjectOwner> projectOwnerRepository, IRepository<SkillProvider> skillProviderRepository)
        {
            _projectRepository = projectRepository;
            _projectOwnerRepository = projectOwnerRepository;
            _skillProviderRepository = skillProviderRepository;
            _mongoClient = mongoClient;
        }

        public async Task<ApiResponse<Guid>> AddProjectAsync(ProjectCreationDto projectCreationDto)
        {
            ProjectOwner? projectOwner = await _projectOwnerRepository.GetByIdAsync(projectCreationDto.ProjectOwnerId);
            if (projectOwner == null)
                return ApiResponse<Guid>.Fail($"Project Owner with ID {projectCreationDto.ProjectOwnerId} not found.");

            var projectId = Guid.NewGuid();
            projectOwner.AddProject(projectId);

            Project newProject = new Project
            {
                ProjectId = projectId,
                ProjectOwnerId = projectCreationDto.ProjectOwnerId,
                ProjectName = projectCreationDto.ProjectName,
                ProjectLocation = projectCreationDto.ProjectLocation,
                ProjectDescription = projectCreationDto.ProjectDescription,
                ProjectStartDate = projectCreationDto.ProjectStartDate,
                ProjectEndDate = projectCreationDto.ProjectEndDate,
                ProjectStatus = projectCreationDto.ProjectStatus,
                ProjectSkillsRequired = projectCreationDto.ProjectSkillsRequired,
                ProjectPriority = projectCreationDto.ProjectPriority,
                ProjectResourcesNeeded = projectCreationDto.ProjectResourcesNeeded,
                ProjectResourcesAvailable = projectCreationDto.ProjectResourcesNeeded,
                ProjectDateCreated = projectCreationDto.ProjectDateCreated,
                ProjectDateUpdated = projectCreationDto.ProjectDateUpdated
            };

            await _projectOwnerRepository.UpdateAsync(projectOwner);
            await _projectRepository.AddAsync(newProject);

            return ApiResponse<Guid>.Ok(projectId, "Project successfully created.");
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

        public async Task<ApiResponse<ProjectDto>> GetSpecificProjectAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return ApiResponse<ProjectDto>.Fail($"Project with ID {projectId} not found.");

            var projectOwner = await _projectOwnerRepository.GetByIdAsync(project.ProjectOwnerId);
            if (projectOwner == null)
                return ApiResponse<ProjectDto>.Fail($"Project owner with ID {project.ProjectOwnerId} not found.");

            var projectMembersDto = new List<ProjectMemberDetailDto>();

            foreach(var projectMember in project.ProjectMembers)
            {
                projectMembersDto.Add(new ProjectMemberDetailDto
                {
                    Email = projectMember.Email,
                    IsResigning = projectMember.IsResigning,
                    MemberId = projectMember.MemberId,
                    Rate = projectMember.Rate,
                    TimeFrame = projectMember.TimeFrame,
                    UserFirstName = projectMember.UserFirstName,
                    UserLastName = projectMember.UserLastName,
                    ResignationReason = projectMember.ResignationReason,
                    UserName = projectMember.UserFirstName + " " + projectMember.UserLastName
                });
            }

            // Determine the earlier and later dates to ensure a non-negative duration
            DateTime earlierDate = (project.ProjectStartDate < project.ProjectEndDate) ? project.ProjectStartDate : project.ProjectEndDate;
            DateTime laterDate = (project.ProjectStartDate < project.ProjectEndDate) ? project.ProjectEndDate : project.ProjectStartDate;

            // Calculate the difference, which is a TimeSpan object
            TimeSpan duration = laterDate.Subtract(earlierDate);

            // Get the total number of whole days in the duration
            int days = duration.Days;

            string projectDuration = days.ToString() + " days";

            var projectDto = new ProjectDto
            {
                ProjectId = project.ProjectId,
                ProjectOwnerId = project.ProjectOwnerId,
                ProjectOwnerName = projectOwner.UserFirstName + " " + projectOwner.UserLastName,
                ProjectName = project.ProjectName,
                ProjectLocation = project.ProjectLocation.ToString(),
                ProjectDescription = project.ProjectDescription,
                ProjectStartDate = project.ProjectStartDate,
                ProjectEndDate = project.ProjectEndDate,
                ProjectStatus = project.ProjectStatus.ToString(),
                ProjectStatusPicker = project.ProjectStatus,
                ProjectSkillsRequired = project.ProjectSkillsRequired,
                ProjectPriority = project.ProjectPriority,
                ProjectResourcesNeeded = project.ProjectResourcesNeeded,
                ProjectResourcesAvailable = project.ProjectResourcesAvailable,
                ProjectDateCreated = project.ProjectDateCreated,
                ProjectDateUpdated = project.ProjectDateUpdated,
                ProjectMembers = projectMembersDto,
                ProjectDuration = projectDuration
            };

            return ApiResponse<ProjectDto>.Ok(projectDto);
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
                    ProjectId = project.ProjectId,
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

        public async Task<ApiResponse<bool>> RequestResignation(RequestResignationDto requestResignationDto)
        {
            var project = await _projectRepository.GetByIdAsync(requestResignationDto.ProjectId);
            if (project == null)
                return ApiResponse<bool>.Fail($"Project with ID {requestResignationDto.ProjectId} not found.");

            var process = project.RequestResignationByMember(requestResignationDto.SkillProviderId, requestResignationDto.Reason);
            if(!process)
                return ApiResponse<bool>.Fail($"Skill provider with ID {requestResignationDto.SkillProviderId} not found.");

            await _projectRepository.UpdateAsync(project);
            return ApiResponse<bool>.Ok(true);
        }

        public async Task<ApiResponse<bool>> ProcessResignation(ProcessResignationDto processResignationDto)
        {
            var project = await _projectRepository.GetByIdAsync(processResignationDto.ProjectId);
            if (project == null)
            {
                return ApiResponse<bool>.Fail($"Project with ID {processResignationDto.ProjectId} not found. Cannot process any resignations.");
            }

            var skillProvider = await _skillProviderRepository.GetByIdAsync(processResignationDto.SkillProviderId);

            if(skillProvider == null)
            {
                return ApiResponse<bool>.Fail($"Skill provider with ID {processResignationDto.SkillProviderId} not found. Cannot process any resignations.");
            }

            skillProvider.ResignProject(processResignationDto.ProjectId);
            project.ProcessResignationOfMember(processResignationDto.SkillProviderId, processResignationDto.ApproveResignation);

            using (var session = await _mongoClient.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    var tasks = new List<Task>();

                    tasks.Add(_projectRepository.UpdateAsync(project, session));
                    tasks.Add(_skillProviderRepository.UpdateAsync(skillProvider, session));
                    await Task.WhenAll(tasks);
                    await session.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    return ApiResponse<bool>.Fail($"A critical error occurred while saving changes: {ex.Message}");
                }
            }
            return ApiResponse<bool>.Ok(true);
        }

        public async Task<ApiResponse<bool>> RateSkillProviders(RateSkillProviderDto rateSkillProviderDto)
        {
            if (rateSkillProviderDto.rateSkillProviderIndividualDtos == null || rateSkillProviderDto.rateSkillProviderIndividualDtos.Count == 0)
            {
                return ApiResponse<bool>.Fail("No rating requests were provided for processing.");
            }

            var errors = new List<string>();
            var skillProvidersToUpdate = new List<SkillProvider>();

            var skillProviderIdsToRate = rateSkillProviderDto.rateSkillProviderIndividualDtos
                .Select(dto => dto.SkillProviderId)
                .Distinct()
                .ToList();

            if (skillProviderIdsToRate.Any())
            {
                var fetchedSkillProviders = await _skillProviderRepository.GetByIdsAsync(skillProviderIdsToRate);
                var skillProviderMap = fetchedSkillProviders.ToDictionary(sp => sp.UserId, sp => sp);

                foreach (var dto in rateSkillProviderDto.rateSkillProviderIndividualDtos)
                {
                    if(skillProviderMap.TryGetValue(dto.SkillProviderId, out var skillProvider))
                    {
                        skillProvider.RateSkillProvider(dto.SkillProviderRating);
                        skillProvidersToUpdate.Add(skillProvider);
                    }
                    else
                    {
                        errors.Add($"Skill provider with ID {dto.SkillProviderId} was not found in database.");
                    }
                }
            }
            try
            {
                foreach (var skillProvider in skillProvidersToUpdate)
                {
                    await _skillProviderRepository.UpdateAsync(skillProvider);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"A critical error occurred while saving changes: {ex.Message}");
            }

            if (errors.Any())
            {
                return ApiResponse<bool>.Fail($"Successfully processed {rateSkillProviderDto.rateSkillProviderIndividualDtos.Count - errors.Count} resignations, but skipped {errors.Count} due to errors. Details: {string.Join("; ", errors.Take(3))}...");
            }

            return ApiResponse<bool>.Ok(true);
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
