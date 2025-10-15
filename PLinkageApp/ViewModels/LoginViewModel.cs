using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageApp.Services;
using PLinkageShared.DTOs;
using System.Data;

namespace PLinkageApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        // Services
        private readonly INavigationService _navigationService;
        private readonly AppShellViewModel _appShellViewModel;
        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;

        // Constructor
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
        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool isBusy = false;

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
                            await _navigationService.NavigateToAsync("SkillProviderHomeView");
                            break;
                        case UserRole.ProjectOwner:
                            await _navigationService.NavigateToAsync("ProjectOwnerHomeView");
                            break;
                        case UserRole.Admin:
                            await _navigationService.NavigateToAsync("AdminHomeView");
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

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage1));
        }
    }
}