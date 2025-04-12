using CommunityToolkit.Mvvm.ComponentModel;
using PLinkage.Interfaces;

namespace PLinkage.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;

        public AppShellViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
            UpdateRoleProperties();
        }

        [ObservableProperty]
        private string welcomeMessage;

        [ObservableProperty]
        private bool isAdmin;

        [ObservableProperty]
        private bool isProjectOwner;

        [ObservableProperty]
        private bool isSkillProvider;

        [ObservableProperty]
        private bool isNotLoggedIn;

        public void UpdateRoleProperties()
        {
            var role = _sessionService.GetCurrentUser()?.UserRole;

            IsAdmin = role == UserRole.Admin;
            IsProjectOwner = role == UserRole.ProjectOwner;
            IsSkillProvider = role == UserRole.SkillProvider;
            IsNotLoggedIn = role == null;

            if (IsNotLoggedIn)
            {
                WelcomeMessage = "Welcome to PLinkage!";
            }
            else
            {
                var user = _sessionService.GetCurrentUser();
                WelcomeMessage = $"Welcome to PLinkage, {user?.UserFirstName}!";
            }

        }
    }
}
