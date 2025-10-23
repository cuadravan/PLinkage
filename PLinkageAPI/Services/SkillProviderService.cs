using MongoDB.Driver;
using PLinkageShared.Enums;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using PLinkageAPI.ValueObject;
using PLinkageShared.DTOs;
using PLinkageShared.ApiResponse;
using AutoMapper;

namespace PLinkageAPI.Services
{
    public class SkillProviderService : ISkillProviderService
    {
        private readonly IRepository<SkillProvider> _skillProviderRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly IMapper _mapper;

        public SkillProviderService(IRepository<SkillProvider> repository, IRepository<Project> projectRepository, IMapper mapper)
        {
            _skillProviderRepository = repository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<SkillProviderDto?>> GetSpecificSkillProviderAsync(Guid skillProviderId)
        {
            var skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);

            if (skillProvider == null)
                return ApiResponse<SkillProviderDto?>.Fail($"Skill provider with ID {skillProviderId} not found.");

            var educationDtos = _mapper.Map<List<EducationDto>>(skillProvider.Educations);
            var skillDtos = _mapper.Map<List<SkillDto>>(skillProvider.Skills);

            var skillProviderDto = new SkillProviderDto
            {
                UserId = skillProvider.UserId,
                UserName = skillProvider.UserFirstName + " " + skillProvider.UserLastName,
                UserFirstName = skillProvider.UserFirstName,
                UserLastName = skillProvider.UserLastName,
                UserPhone = skillProvider.UserPhone,
                UserLocation = skillProvider.UserLocation.ToString(),
                UserEmail = skillProvider.UserEmail,
                UserBirthDate = skillProvider.UserBirthDate,
                UserGender = skillProvider.UserGender,
                UserRole = skillProvider.UserRole,
                UserStatus = skillProvider.UserStatus,
                JoinedOn = skillProvider.JoinedOn,
                UserRating = skillProvider.UserRating.ToString("F2"),
                Educations = educationDtos,
                Skills = skillDtos
            };
            var projects = await _projectRepository.GetByIdsAsync(skillProvider.EmployedProjects);

            foreach (var project in projects)
            {
                var member = project.ProjectMembers.FirstOrDefault(pm => pm.MemberId == skillProviderId);
                skillProviderDto.ProfileProjects.Add(new SkillProviderProfileProjectsDto
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectStatus = project.ProjectStatus.ToString(),
                    TimeFrame = member.TimeFrame,
                    Rate = member.Rate

                });
            }

            return ApiResponse<SkillProviderDto?>.Ok(skillProviderDto, "Skill provider fetched successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateSkillProviderAsync(Guid skillProviderId, UserProfileUpdateDto updateDto)
        {
            try
            {
                var skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);
                if (skillProvider == null)
                    return ApiResponse<bool>.Fail($"Skill provider with ID {skillProviderId} not found.");

                skillProvider.UpdateProfile(updateDto);
                await _skillProviderRepository.UpdateAsync(skillProvider);

                return ApiResponse<bool>.Ok(true, "Skill provider updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error updating skill provider: {ex.Message}");
            }
        }

        // -------------- EDUCATIONS -----------------

        public async Task<ApiResponse<bool>> AddEducationAsync(Guid skillProviderId, Education education)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            sp.AddEducation(education);
            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Education added successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateEducationAsync(Guid skillProviderId, int index, Education updatedEducation)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            if (!sp.UpdateEducation(index, updatedEducation))
                return ApiResponse<bool>.Fail("Education index invalid or update failed.");

            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Education updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteEducationAsync(Guid skillProviderId, int index)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            if (!sp.DeleteEducation(index))
                return ApiResponse<bool>.Fail("Education index invalid or delete failed.");

            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Education deleted successfully.");
        }

        // -------------- SKILLS -----------------

        public async Task<ApiResponse<bool>> AddSkillAsync(Guid skillProviderId, Skill skill)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            sp.AddSkill(skill);
            await _skillProviderRepository.UpdateAsync(sp);

            return ApiResponse<bool>.Ok(true, "Skill added successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateSkillAsync(Guid skillProviderId, int index, Skill skill)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            if (!sp.UpdateSkill(index, skill))
                return ApiResponse<bool>.Fail("Skill index invalid or update failed.");

            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Skill updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteSkillAsync(Guid skillProviderId, int index)
        {
            var sp = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (sp == null)
                return ApiResponse<bool>.Fail("Skill provider not found.");

            if (!sp.DeleteSkill(index))
                return ApiResponse<bool>.Fail("Skill index invalid or delete failed.");

            await _skillProviderRepository.UpdateAsync(sp);
            return ApiResponse<bool>.Ok(true, "Skill deleted successfully.");
        }

        private static readonly Dictionary<string, int> ProximityRanges = new()
            {
                { "Nearby (<= 10km)", 10 },
                { "Within Urban (<= 20km)", 20 },
                { "Extended (<= 50km)", 50 }
            };

        public async Task<ApiResponse<IEnumerable<SkillProviderCardDto>>> GetFilteredSkillProvidersAsync(
    string proximity,
    CebuLocation? location,
    string status,
    bool? isEmployed) // 🌟 NEW PARAMETER
        {
            try
            {
                // 1. Initial validation (unchanged)
                if (proximity != "All" && proximity != "By Specific Location" && !location.HasValue)
                {
                    return ApiResponse<IEnumerable<SkillProviderCardDto>>.Fail("A location must be provided for proximity searches.");
                }

                ProximityRanges.TryGetValue(proximity, out var range);

                var nearbyLocationsWithDistance = new Dictionary<CebuLocation, double>();
                if (range > 0 && location.HasValue)
                {
                    nearbyLocationsWithDistance = GetNearbyLocationsWithDistance(location.Value, range);
                }

                var builder = Builders<SkillProvider>.Filter;
                var filter = builder.Empty;

                // 2. Filter by User Status (unchanged)
                if (status != "All")
                {
                    filter &= builder.Eq(sp => sp.UserStatus, status);
                }

                // 3. Filter by Location (unchanged)
                if (proximity == "By Specific Location" && location.HasValue)
                {
                    filter &= builder.Eq(sp => sp.UserLocation, location.Value);
                }
                else if (nearbyLocationsWithDistance.Any())
                {
                    filter &= builder.In(sp => sp.UserLocation, nearbyLocationsWithDistance.Keys.Cast<CebuLocation?>());
                }

                // 🌟 4. Filter by Employment Status
                if (isEmployed.HasValue)
                {
                    // First, get the IDs of all currently Active projects
                    var activeProjectsFilter = Builders<Project>.Filter.Eq(p => p.ProjectStatus, ProjectStatus.Active);
                    // Assuming you have access to a Project Repository (_projectRepository)
                    var activeProjects = await _projectRepository.FindAsync(activeProjectsFilter);

                    var activeProjectIds = activeProjects?.Select(p => p.ProjectId).ToList() ?? new List<Guid>();

                    if (isEmployed.Value)
                    {
                        // Filter for skill providers who have AT LEAST ONE employed project ID that is currently active.
                        filter &= builder.AnyIn(sp => sp.EmployedProjects, activeProjectIds);
                    }
                    else
                    {
                        // Filter for skill providers who have NO employed project IDs that are currently active.
                        // This is complex in MongoDB. The best approach is often to find ALL providers
                        // and then filter the results in C# if MongoDB complexity is too high, 
                        // OR use $nin if the collection is guaranteed to contain only the active ones, 
                        // which is not the case here.

                        // A more performant approach: Find all providers who ARE employed (as above), 
                        // and then exclude them. We will use a $nin query here, assuming EmployedProjects 
                        // contains the project IDs you are checking against.

                        // NOTE: This assumes EmployedProjects only holds IDs relevant to *potential* employment. 
                        // The true MongoDB way to implement NOT EMPLOYED is complex. For simplicity, 
                        // we'll apply the filter to the active projects list.

                        filter &= builder.Not(builder.AnyIn(sp => sp.EmployedProjects, activeProjectIds));
                    }
                }
                // ------------------------------------

                var skillProviders = await _skillProviderRepository.FindAsync(filter);

                // ... rest of the method (mapping to DTOs) ...

                // 5. Check for null/empty results (unchanged)
                if (skillProviders == null || !skillProviders.Any())
                {
                    return ApiResponse<IEnumerable<SkillProviderCardDto>>.Fail("No skill providers found matching the criteria.");
                }

                // 6. Mapping to DTOs (unchanged)
                var skillProviderCardDtos = skillProviders.Select(sp =>
                {
                    // ... mapping logic remains the same ...
                    string locationString = sp.UserLocation?.ToString() ?? "Location not set";

                    if (location.HasValue && sp.UserLocation.HasValue)
                    {
                        if (nearbyLocationsWithDistance.TryGetValue(sp.UserLocation.Value, out double distance))
                        {
                            locationString = $"{sp.UserLocation.Value}, {distance:F0} km away";
                        }
                        else if (!nearbyLocationsWithDistance.Any())
                        {
                            var baseLoc = Location.From(location.Value);
                            var providerLoc = Location.From(sp.UserLocation.Value);
                            distance = baseLoc.DistanceTo(providerLoc);
                            locationString = $"{sp.UserLocation.Value}, {distance:F0} km away";
                        }
                    }

                    return new SkillProviderCardDto
                    {
                        UserId = sp.UserId,
                        UserName = sp.UserFirstName + " " + sp.UserLastName,
                        UserRating = sp.UserRating.ToString("F2") + " ☆",
                        Location = locationString,
                        Education = sp.Educations != null && sp.Educations.Count > 0
                            ? sp.Educations[0].CourseName + " at " + sp.Educations[0].SchoolAttended
                            : string.Empty,
                        Skills = sp.Skills?
                            .Select(skill => skill.SkillName)
                            .Take(5)
                            .ToList() ?? new List<string>()
                    };
                });

                return ApiResponse<IEnumerable<SkillProviderCardDto>>.Ok(skillProviderCardDtos, "Filtered skill providers fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SkillProviderCardDto>>.Fail($"Error fetching skill providers: {ex.Message}");
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
