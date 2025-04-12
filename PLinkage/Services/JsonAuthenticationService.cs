using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.Services
{
    public class JsonAuthenticationService : IAuthenticationService
    {
        private readonly IRepository<SkillProvider> _skillProviderRepo;
        private readonly IRepository<ProjectOwner> _projectOwnerRepo;
        private readonly IRepository<Admin> _adminRepo;
        private readonly ISessionService _sessionService;

        public JsonAuthenticationService(
            IRepository<SkillProvider> skillProviderRepo,
            IRepository<ProjectOwner> projectOwnerRepo,
            IRepository<Admin> adminRepo,
            ISessionService sessionService)
        {
            _skillProviderRepo = skillProviderRepo;
            _projectOwnerRepo = projectOwnerRepo;
            _adminRepo = adminRepo;
            _sessionService = sessionService;
        }

        public async Task<IUser?> LoginAsync(string email, string password)
        {
            // Try SkillProvider
            var skillProviders = await _skillProviderRepo.GetAllAsync();
            var skillProvider = skillProviders
                .FirstOrDefault(u => u.UserEmail == email && u.UserPassword == password);

            if (skillProvider != null)
            {
                _sessionService.SetCurrentUser(skillProvider);
                return skillProvider;
            }

            // Try ProjectOwner
            var projectOwners = await _projectOwnerRepo.GetAllAsync();
            var projectOwner = projectOwners
                .FirstOrDefault(u => u.UserEmail == email && u.UserPassword == password);

            if (projectOwner != null)
            {
                _sessionService.SetCurrentUser(projectOwner);
                return projectOwner;
            }

            // Try Admin
            var admins = await _adminRepo.GetAllAsync();
            var admin = admins
                .FirstOrDefault(u => u.UserEmail == email && u.UserPassword == password);

            if (admin != null)
            {
                _sessionService.SetCurrentUser(admin);
                return admin;
            }

            // No match
            return null;
        }

        public Task LogoutAsync()
        {
            _sessionService.ClearSession();
            return Task.CompletedTask;
        }

        public bool IsUserLoggedIn() => _sessionService.GetCurrentUser() != null;
    }
}
