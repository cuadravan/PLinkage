using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkage.Interfaces;
using PLinkage.Views;

namespace PLinkage.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly AppShellViewModel _appShellViewModel;
        private readonly ISessionService _sessionService;

        public LoginViewModel(
            INavigationService navigationService,
            IAuthenticationService authenticationService,
            AppShellViewModel appShellViewModel,
            ISessionService sessionService)
        {
            _navigationService = navigationService;
            _authenticationService = authenticationService;
            _appShellViewModel = appShellViewModel;
            _sessionService = sessionService;
        }

        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private string errorMessage;

        [RelayCommand]
        private async Task Login()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both email and password.";
                return;
            }

            var user = await _authenticationService.LoginAsync(Email, Password);
            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return;
            }

            // Update shell view model so visibility changes immediately
            _appShellViewModel.UpdateRoleProperties();

            ErrorMessage = "Successful login.";

            // Navigate to default home/root page
            if(_sessionService.GetCurrentUserType() == UserRole.SkillProvider)
            {
                await _navigationService.NavigateToAsync(nameof(SkillProviderHomeView)); // replace with your real root
            }
            else if (_sessionService.GetCurrentUserType() == UserRole.ProjectOwner)
            {
                await _navigationService.NavigateToAsync(nameof(ProjectOwnerHomeView)); // replace with your real root
            }
                
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await _navigationService.NavigateToAsync(nameof(RegisterView));
        }
    }
}
