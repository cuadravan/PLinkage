using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AppShellViewModel _appShellViewModel;
        private readonly INavigationService _navigationService;    
        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;

        public LoginViewModel(
            IAccountServiceClient accountServiceClient,
            INavigationService navigationService,
            AppShellViewModel appShellViewModel,
            ISessionService sessionService)
        {
            _navigationService = navigationService;
            _appShellViewModel = appShellViewModel;
            _sessionService = sessionService;
            _accountServiceClient = accountServiceClient;
        }

        // Properties
        [ObservableProperty] 
        private string email;
        [ObservableProperty] 
        private string password;
        [ObservableProperty] 
        private string errorMessage;
        [ObservableProperty] 
        private bool isBusy = false;

        // Commands
        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy)
            {
                return;
            }
            if (!ValidateInput())
            {
                return;
            }

            try
            {
                IsBusy = true;

                var loginRequest = new LoginRequestDto
                {
                    UserEmail = Email,
                    UserPassword = Password
                };

                var result = await _accountServiceClient.LoginAsync(loginRequest);

                if (result.Success && result.Data != null)
                {
                    Email = string.Empty;
                    Password = string.Empty;
                    _sessionService.SetCurrentUser(result.Data);
                    _appShellViewModel.UpdateRoleProperties();
                    var userRole = _sessionService.GetCurrentUserRole();

                    if (Application.Current.MainPage is AppShellAndroid shell)
                    {
                        shell.ConfigureTabs(userRole);
                    }

                    switch (_sessionService.GetCurrentUserRole())
                    {
                        case UserRole.SkillProvider:
                            await _navigationService.NavigateAndClearStackAsync("SkillProviderHomeView");
                            break;
                        case UserRole.ProjectOwner:
                            await _navigationService.NavigateAndClearStackAsync("ProjectOwnerHomeView");
                            break;
                        case UserRole.Admin:
                            await _navigationService.NavigateAndClearStackAsync("AdminHomeView");
                            break;
                        default:
                            ErrorMessage = "Unknown user role.";
                            break;
                    }
                }
                else
                {
                    ErrorMessage = result.Message ?? "Login failed due to an unknown API error.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task GoToRegister()
        {
            if (IsBusy)
                return;
#if ANDROID
            await _navigationService.NavigateToAsync("RegisterView1");
#else
            await _navigationService.NavigateToAsync("RegisterView");
#endif
        }

        private bool ValidateInput()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Email address cannot be blank.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Password cannot be blank.";
                return false;
            }

            return true;
        }    
    }
}