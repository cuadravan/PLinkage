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

        public ProjectService(IRepository<Project> projectRepository, IRepository<ProjectOwner> projectOwnerRepository, IRepository<SkillProvider> skillProviderRepository)
        {
            _projectRepository = projectRepository;
            _projectOwnerRepository = projectOwnerRepository;
            _skillProviderRepository = skillProviderRepository;
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
            var member = project.ProjectMembers.FirstOrDefault(pm => pm.MemberId == requestResignationDto.SkillProviderId);
            if (member == null)
                return ApiResponse<bool>.Fail($"Skill provider with ID {requestResignationDto.SkillProviderId} not found.");
            member.IsResigning = true;
            member.ResignationReason = requestResignationDto.Reason;
            await _projectRepository.UpdateAsync(project);

            return ApiResponse<bool>.Ok(true);
        }

        public async Task<ApiResponse<bool>> ProcessResignation(ProcessResignationDto processResignationDto)
        {
            if (processResignationDto.processResignationIndividualDtos == null || processResignationDto.processResignationIndividualDtos.Count == 0)
            {
                return ApiResponse<bool>.Fail("No resignation requests were provided for processing.");
            }

            var errors = new List<string>();

            var project = await _projectRepository.GetByIdAsync(processResignationDto.ProjectId);
            if (project == null)
            {
                return ApiResponse<bool>.Fail($"Project with ID {processResignationDto.ProjectId} not found. Cannot process any resignations.");
            }

            var approvedSkillProviderIds = processResignationDto.processResignationIndividualDtos
                .Where(dto => dto.ApproveResignation)
                .Select(dto => dto.SkillProviderId)
                .Distinct()
                .ToList();

            var skillProvidersToUpdate = new List<SkillProvider>();

            if (approvedSkillProviderIds.Any())
            {
                var fetchedSkillProviders = await _skillProviderRepository.GetByIdsAsync(approvedSkillProviderIds);
                var skillProviderMap = fetchedSkillProviders.ToDictionary(sp => sp.UserId, sp => sp);

                foreach (var dto in processResignationDto.processResignationIndividualDtos)
                {
                    var member = project.ProjectMembers.FirstOrDefault(pm => pm.MemberId == dto.SkillProviderId);

                    if (member == null)
                    {
                        errors.Add($"Skill provider ID {dto.SkillProviderId} not found in project's member list. Skipping.");
                        continue;
                    }

                    if (dto.ApproveResignation)
                    {
                        if (skillProviderMap.TryGetValue(dto.SkillProviderId, out var skillProvider))
                        {
                            project.ProjectMembers.Remove(member);
                            skillProvider.EmployedProjects.Remove(processResignationDto.ProjectId);
                            skillProvidersToUpdate.Add(skillProvider);
                        }
                        else
                        {
                            errors.Add($"Skill provider ID {dto.SkillProviderId} not found in database. Cannot approve removal.");
                        }
                    }
                    else
                    {
                        member.IsResigning = false;
                        member.ResignationReason = string.Empty;
                    }
                }
            }
            else
            {
                foreach (var dto in processResignationDto.processResignationIndividualDtos)
                {
                    var member = project.ProjectMembers.FirstOrDefault(pm => pm.MemberId == dto.SkillProviderId);

                    if (member == null)
                    {
                        errors.Add($"Skill provider ID {dto.SkillProviderId} not found in project's member list. Skipping.");
                        continue;
                    }

                    member.IsResigning = false;
                    member.ResignationReason = string.Empty;
                }
            }

            try
            {
                await _projectRepository.UpdateAsync(project);

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
                return ApiResponse<bool>.Fail($"Successfully processed {processResignationDto.processResignationIndividualDtos.Count - errors.Count} resignations for Project ID {processResignationDto.ProjectId}, but skipped {errors.Count} due to errors. Details: {string.Join("; ", errors.Take(3))}...");
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
                        skillProvider.UserRatingCount += 1;
                        skillProvider.UserRatingTotal += dto.SkillProviderRating;
                        skillProvider.UserRating = skillProvider.UserRatingTotal / skillProvider.UserRatingCount;
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
