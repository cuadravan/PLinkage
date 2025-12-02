using PLinkageAPI.Entities;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageAPI.Interfaces;
using PLinkageShared.Enums;

namespace PLinkageAPI.Services
{
    public class DashboardService: IDashboardService
    {
        private readonly IRepository<SkillProvider> _skillProviderRepository;
        private readonly IRepository<ProjectOwner> _projectOwnerRepository;
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<OfferApplication> _offerApplicationRepository;

        public DashboardService(IRepository<SkillProvider> skillProviderRepository, IRepository<ProjectOwner> projectOwnerRepository, IRepository<Admin> adminRepository, IRepository<Project> projectRepository, IRepository<OfferApplication> offerApplicationRepository)
        {
            _skillProviderRepository = skillProviderRepository;
            _projectOwnerRepository = projectOwnerRepository;
            _adminRepository = adminRepository;
            _projectRepository = projectRepository;
            _offerApplicationRepository = offerApplicationRepository;
        }

        public async Task<ApiResponse<ProjectOwnerDashboardDto>> GetProjectOwnerDashboardAsync(Guid projectOwnerId)
        {
            var projectOwner = await _projectOwnerRepository.GetByIdAsync(projectOwnerId);
            if (projectOwner == null)
                return ApiResponse<ProjectOwnerDashboardDto>.Fail("Requested project owner ID not found.");

            // Fetch projects and offers in bulk
            var projectsTask = _projectRepository.GetByIdsAsync(projectOwner.OwnedProjectId);
            var offersTask = _offerApplicationRepository.GetByIdsAsync(projectOwner.OfferApplicationId);

            await Task.WhenAll(projectsTask, offersTask);

            var projects = projectsTask.Result;
            var offers = offersTask.Result;

            var activeProjectsCount = projects.Count(p => p.ProjectStatus == ProjectStatus.Active);

            var pendingSentOffersCount = offers.Count(o =>
                o.SenderId == projectOwnerId && o.OfferApplicationStatus == "Pending");

            var receivedApplicationCount = offers.Count(o =>
                o.ReceiverId == projectOwnerId && o.OfferApplicationStatus == "Pending");

            // This counts the total number of members
            var resignationCount = projects.SelectMany(p => p.ProjectMembers) //Flatten all project members list into one
                                           .Count(m => m.IsResigning);

            var negotiationCount = offers.Count(o => o.IsNegotiating == true);

            var dto = new ProjectOwnerDashboardDto
            {
                PendingSentOffers = pendingSentOffersCount,
                ReceivedApplications = receivedApplicationCount,
                ActiveProjects = activeProjectsCount,
                ReportedResignations = resignationCount,
                ReportedNegotiations = negotiationCount
            };

            return ApiResponse<ProjectOwnerDashboardDto>.Ok(dto);
        }

        public async Task<ApiResponse<SkillProviderDashboardDto>> GetSkillProviderDashboardAsync(Guid skillProviderId)
        {
            var skillProvider = await _skillProviderRepository.GetByIdAsync(skillProviderId);
            if (skillProvider == null)
                return ApiResponse<SkillProviderDashboardDto>.Fail("Requested skill provider ID not found.");

            var projectsTask = _projectRepository.GetByIdsAsync(skillProvider.EmployedProjects);
            var offersTask = _offerApplicationRepository.GetByIdsAsync(skillProvider.OfferApplicationId);

            await Task.WhenAll(projectsTask, offersTask);

            var projects = projectsTask.Result;
            var offers = offersTask.Result;

            var activeProjectsCount = projects.Count(p => p.ProjectStatus == ProjectStatus.Active);
            var pendingSentApplicationsCount = offers.Count(o =>
                o.SenderId == skillProviderId && o.OfferApplicationStatus == "Pending");
            var receivedOfferCount = offers.Count(o =>
                o.ReceiverId == skillProviderId && o.OfferApplicationStatus == "Pending");

            var dto = new SkillProviderDashboardDto
            {
                PendingSentApplications = pendingSentApplicationsCount,
                ReceivedOffers = receivedOfferCount,
                ActiveProjects = activeProjectsCount
            };

            return ApiResponse<SkillProviderDashboardDto>.Ok(dto);
        }

        public async Task<ApiResponse<AdminDashboardDto>> GetAdminDashboardAsync(Guid adminId)
        {
            var admin = await _adminRepository.GetByIdAsync(adminId);
            if (admin == null)
                return ApiResponse<AdminDashboardDto>.Fail("Requested admin ID not found.");

            var projectsTask = _projectRepository.GetAllAsync();
            var skillProvidersTask = _skillProviderRepository.GetAllAsync();

            await Task.WhenAll(projectsTask, skillProvidersTask);

            var projects = projectsTask.Result;
            var skillProviders = skillProvidersTask.Result;

            var activeProjectCount = projects.Count(p => p.ProjectStatus == ProjectStatus.Active);
            var completedProjectCount = projects.Count(p => p.ProjectStatus == ProjectStatus.Completed);

            var activeSkillProviders = skillProviders
                .Where(sp => sp.UserStatus == "Active")
                .ToList();

            var employedSkillProviderCount = activeSkillProviders.Count(sp =>
                sp.EmployedProjects?.Any(id =>
                    projects.Any(p => p.ProjectId == id && p.ProjectStatus == ProjectStatus.Active)) == true);

            double employmentRatio = activeSkillProviders.Count > 0
                ? (double)employedSkillProviderCount / activeSkillProviders.Count
                : 0;

            var dto = new AdminDashboardDto
            {
                OverallActiveProjects = activeProjectCount,
                OverallCompleteProjects = completedProjectCount,
                EmploymentRatio = employmentRatio
            };

            return ApiResponse<AdminDashboardDto>.Ok(dto);
        }


    }
}
