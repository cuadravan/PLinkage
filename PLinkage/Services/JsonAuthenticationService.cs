using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.Services
{
    public class JsonAuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;

        public JsonAuthenticationService(IUnitOfWork unitOfWork, ISessionService sessionService)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
        }

        public async Task<IUser?> LoginAsync(string email, string password)
        {
            await _unitOfWork.ReloadAsync();

            var skillProviders = await _unitOfWork.SkillProvider.GetAllAsync();
            var skillProvider = skillProviders
                .FirstOrDefault(u => u.UserEmail == email && u.UserPassword == password);

            if (skillProvider != null)
            {
                _sessionService.SetCurrentUser(skillProvider);
                return skillProvider;
            }

            // Try ProjectOwner
            var projectOwners = await _unitOfWork.ProjectOwner.GetAllAsync();
            var projectOwner = projectOwners
                .FirstOrDefault(u => u.UserEmail == email && u.UserPassword == password);

            if (projectOwner != null)
            {
                _sessionService.SetCurrentUser(projectOwner);
                return projectOwner;
            }

            // Try Admin
            var admins = await _unitOfWork.Admin.GetAllAsync();
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
