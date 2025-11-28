using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.DTOs;
using System.Text.RegularExpressions;

namespace PLinkageApp.ViewModels
{
    // Add is Busy
    public partial class UpdateProfileViewModel : ObservableValidator
    {
        private readonly INavigationService _navigationService;
        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly IProjectOwnerServiceClient _projectOwnerServiceClient;
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly IDialogService _dialogService;

        private bool _isUpdated = false;

        private UserRole? currentUserRole;
        private Guid currentUserId;

        // Stored Information Used When Resetting
        private string storedFirstName;
        private string storedLastName;
        private string storedMobileNumber;
        private DateTime storedBirthdate;
        private CebuLocation? storedLocation;
        private string storedGender;

        [ObservableProperty]
        private string firstName;
        [ObservableProperty]
        private string lastName;
        [ObservableProperty]
        private string mobileNumber;
        [ObservableProperty]
        private DateTime birthdate;
        [ObservableProperty]
        private CebuLocation? locationSelection;
        [ObservableProperty]
        private string genderSelection;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private string confirmPassword;
        [ObservableProperty]
        private string errorMessage = string.Empty;
        [ObservableProperty]
        private string errorMessagePassword = string.Empty;
        [ObservableProperty]
        private bool isBusy = false;

        public ObservableCollection<CebuLocation> LocationOptions { get; } = new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());   
        public ObservableCollection<string> GenderOptions { get; } = new ObservableCollection<string>()
        {
            "Male", "Female"
        };

        // Constructor
        public UpdateProfileViewModel(IDialogService dialogService, INavigationService navigationService, ISessionService sessionService, IAccountServiceClient accountServiceClient, IProjectOwnerServiceClient projectOwnerServiceClient, ISkillProviderServiceClient skillProviderServiceClient)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _projectOwnerServiceClient = projectOwnerServiceClient;
            _skillProviderServiceClient = skillProviderServiceClient;
            _accountServiceClient = accountServiceClient;
            _dialogService = dialogService;
        }

        public async Task InitializeAsync()
        {
            currentUserId = _sessionService.GetCurrentUserId();
            currentUserRole = _sessionService.GetCurrentUserRole();

            if (currentUserId == Guid.Empty || currentUserRole == null)
            {
                await _dialogService.ShowAlertAsync("Error", "Application is bugged. Contact an administrator, or refresh the app.", "Ok");
                await _navigationService.GoBackAsync();
            }
            try
            {
                if (currentUserRole == UserRole.SkillProvider)
                {
                    var result = await _skillProviderServiceClient.GetSpecificAsync(currentUserId);
                    if (!result.Success || result.Data == null)
                    {
                        await _dialogService.ShowAlertAsync("Error", "Server could not find the profile requested. Contact an administrator, or refresh the app.", "Ok");
                        await _navigationService.GoBackAsync();
                    }
                    storedFirstName = result.Data.UserFirstName;
                    storedLastName = result.Data.UserLastName;
                    storedMobileNumber = result.Data.UserPhone;
                    storedLocation = (CebuLocation?)Enum.Parse(typeof(CebuLocation), result.Data.UserLocation);
                    storedBirthdate = result.Data.UserBirthDate;
                    storedGender = result.Data.UserGender;
                    await Reset();
                }
                else if (currentUserRole == UserRole.ProjectOwner)
                {
                    var result = await _projectOwnerServiceClient.GetSpecificAsync(currentUserId);
                    if (!result.Success || result.Data == null)
                    {
                        await _dialogService.ShowAlertAsync("Error", "Server could not find the profile requested. Contact an administrator, or refresh the app.", "Ok");
                        await _navigationService.GoBackAsync();
                    }
                    storedFirstName = result.Data.UserFirstName;
                    storedLastName = result.Data.UserLastName;
                    storedMobileNumber = result.Data.UserPhone;
                    storedLocation = (CebuLocation?)Enum.Parse(typeof(CebuLocation), result.Data.UserLocation);
                    storedBirthdate = result.Data.UserBirthDate;
                    storedGender = result.Data.UserGender;
                    await Reset();
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Failed", $"Profile request failed due to following error: {ex}. Please try again.", "Ok");
            }
        }

        [RelayCommand]
        private async Task UpdateProfile()
        {
            if (!Regex.IsMatch(FirstName, @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$"))
            {
                ErrorMessage = "First Name must be valid name.";
                return;
            }
            else if (!Regex.IsMatch(LastName, @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$"))
            {
                ErrorMessage = "Last Name must be valid name.";
                return;
            }

            var today = DateTime.Today;
            var age = today.Year - Birthdate.Year;
            if (Birthdate.Date > today.AddYears(-age)) age--;
            if (age < 18)
            {
                ErrorMessage = "You must be older than 18 years old to use this app.";
                return;
            }
            if (String.IsNullOrEmpty(GenderSelection))
            {
                ErrorMessage = "Please select your gender.";
                return;
            }
            if (LocationSelection == null)
            {
                ErrorMessage = "Please select your location.";
                return;
            }
            if (!Regex.IsMatch(MobileNumber, @"^\d{10,11}$"))
            {
                ErrorMessage = "Phone Number must be 10-11 numeric digits.";
                return;
            }
            ErrorMessage = string.Empty;
            var updateProfileDto = new UserProfileUpdateDto
            {
                UserFirstName = FirstName,
                UserLastName = LastName,
                UserPhone = MobileNumber,
                UserLocation = LocationSelection,
                UserBirthDate = Birthdate,
                UserGender = GenderSelection
            };
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                if (currentUserRole == UserRole.SkillProvider)
                {
                    var result = await _skillProviderServiceClient.UpdateSkillProviderAsync(currentUserId, updateProfileDto);
                    if (result.Success)
                    {
                        await _dialogService.ShowAlertAsync("Success", "Successfully updated profile!", "Ok");
                        _isUpdated = true;
                        storedFirstName = FirstName;
                        storedLastName = LastName;
                        storedGender = GenderSelection;
                        storedLocation = LocationSelection;
                        storedMobileNumber = MobileNumber;
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Failed", $"Server returned following error. {result.Message}. Please try again.", "Ok");
                    }
                }
                else if(currentUserRole == UserRole.ProjectOwner)
                {
                    var result = await _projectOwnerServiceClient.UpdateProjectOwnerAsync(currentUserId, updateProfileDto);
                    if (result.Success)
                    {
                        await _dialogService.ShowAlertAsync("Success", "Successfully updated profile!", "Ok");
                        _isUpdated = true;
                        storedFirstName = FirstName;
                        storedLastName = LastName;
                        storedGender = GenderSelection;
                        storedLocation = LocationSelection;
                        storedMobileNumber = MobileNumber;
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Failed", $"Server returned following error. {result.Message}. Please try again.", "Ok");
                    }
                }
            }
            catch(Exception ex)
            {
                await _dialogService.ShowAlertAsync("Failed", $"Profile update failed due to following error: {ex}. Please try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Reset()
        {
            if (IsBusy)
                return;
            FirstName = storedFirstName;
            LastName = storedLastName;
            MobileNumber = storedMobileNumber;
            LocationSelection = storedLocation;
            Birthdate = storedBirthdate;
            GenderSelection = storedGender;
        }

        [RelayCommand]
        private async Task ChangePassword()
        {
            if (!Regex.IsMatch(Password, "^.{8,}$"))
            {
                ErrorMessagePassword = "Password must at least be 8 characters long.";
                return;
            }

            if (!String.Equals(Password, ConfirmPassword))
            {
                ErrorMessagePassword = "Passwords must match.";
                return;
            }
            ErrorMessagePassword = string.Empty;
            var changePasswordDto = new ChangePasswordDto
            {
                UserId = currentUserId,
                UserRole = currentUserRole,
                NewPassword = Password
            };
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                if (currentUserRole == UserRole.SkillProvider)
                {
                    var result = await _accountServiceClient.ChangePasswordAsync(changePasswordDto);
                    if (result.Success)
                    {
                        await _dialogService.ShowAlertAsync("Success", "Successfully changed password!", "Ok");
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Failed", $"Server returned following error. {result.Message}. Please try again.", "Ok");
                    }
                }
                else if (currentUserRole == UserRole.ProjectOwner)
                {
                    var result = await _accountServiceClient.ChangePasswordAsync(changePasswordDto);
                    if (result.Success)
                    {
                        await _dialogService.ShowAlertAsync("Success", "Successfully updated profile!", "Ok");
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Failed", $"Server returned following error. {result.Message}. Please try again.", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Failed", $"Password changed failed due to following error: {ex}. Please try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ResetPasswordField()
        {
            if (IsBusy)
                return;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        [RelayCommand]
        private async Task Return()
        {
            if (IsBusy)
                return;
            if (_isUpdated)
                await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", true } });
            else
                await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", false } });
        }

    }
}
