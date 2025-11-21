using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Views;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;

namespace PLinkageApp.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        public AppShellViewModel(ISessionService sessionService, INavigationService navigationService)
        {
            _sessionService = sessionService;
            _navigationService = navigationService;
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

        [ObservableProperty]
        private bool isLoggedIn;

        [ObservableProperty]
        private string userRoleMessage;

        public void UpdateRoleProperties()
        {
            var role = _sessionService.GetCurrentUserRole();
            var username = _sessionService.GetCurrentUserName();
            
            IsAdmin = role == UserRole.Admin;
            IsProjectOwner = role == UserRole.ProjectOwner;
            IsSkillProvider = role == UserRole.SkillProvider;
            IsNotLoggedIn = role == null;
            IsLoggedIn = !IsNotLoggedIn;
            if (IsNotLoggedIn)
            {
                WelcomeMessage = "Welcome to PLinkage!";
                UserRoleMessage = string.Empty;
            }
            else
            {
                var userType = _sessionService.GetCurrentUserRole();
                WelcomeMessage = $"Welcome to PLinkage! {username}";
                UserRoleMessage = userType.ToString();
            }

        }

        [RelayCommand]
        public async Task Logout()
        {
            _sessionService.ClearSession();
            UpdateRoleProperties();
            if (Application.Current.MainPage is AppShellWindows shell)
            {
                shell.ClearFlyout();
            }
            await _navigationService.NavigateAndClearStackAsync("LoginView");
        }
    }
}
