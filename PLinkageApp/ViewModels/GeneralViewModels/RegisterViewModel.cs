using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.DTOs;
using System.Text.RegularExpressions;

namespace PLinkageApp.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly IDialogService _dialogService;

        private UserRole? userRoleSelected = null;

        [ObservableProperty]
        private string firstName = string.Empty;

        [ObservableProperty]
        private string lastName = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        [ObservableProperty]
        private DateTime birthdate = DateTime.Today;

        [ObservableProperty]
        private string selectedGender = string.Empty;

        [ObservableProperty]
        private CebuLocation? selectedLocation = null;

        [ObservableProperty]
        private string mobileNumber = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool skillProviderSelected = false;

        [ObservableProperty]
        private bool projectOwnerSelected = false;

        [ObservableProperty]
        private bool isBusy = false;

        public ObservableCollection<CebuLocation> LocationOptions { get; } =
            new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());

        public ObservableCollection<string> GenderOptions { get; } = new()
        {
            "Male",
            "Female"
        };      

        public RegisterViewModel(IDialogService dialogService, INavigationService navigationService, IAccountServiceClient accountServiceClient)
        {
            _navigationService = navigationService;
            _accountServiceClient = accountServiceClient;
            _dialogService = dialogService;
        }

        [RelayCommand]
        private async Task GoToRegisterView2()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (!Regex.IsMatch(FirstName, @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$"))
            {
                ErrorMessage = "First Name must be valid name.";
                IsBusy = false;
            }
            else if (!Regex.IsMatch(LastName, @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$"))
            {
                ErrorMessage = "Last Name must be valid name.";
                IsBusy = false;
            }
            else
            {
                ErrorMessage = string.Empty;
                IsBusy = false;
                await _navigationService.NavigateToAsync("RegisterView2");
            }
        }

        [RelayCommand]
        private async Task GoToRegisterView3()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (!String.IsNullOrEmpty(Email) && Regex.IsMatch(Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
            {
                var response = await _accountServiceClient.CheckEmailUniquenessAsync(Email);
                if (!response.Success)
                {
                    ErrorMessage = "Email is already in use.";
                    IsBusy = false;
                    return;
                }
            }
            else
            {
                ErrorMessage = "Email must not be empty or invalid.";
                IsBusy = false;
                return;
            }
            if (!Regex.IsMatch(Password, "^.{8,}$"))
            {
                ErrorMessage = "Password must at least be 8 characters long.";
                IsBusy = false;
                return;
            }

            if (!String.Equals(Password, ConfirmPassword))
            {
                ErrorMessage = "Passwords must match.";
                IsBusy = false;
                return;
            }
            ErrorMessage = string.Empty;
            IsBusy = false;
            await _navigationService.NavigateToAsync("RegisterView3");
        }

        [RelayCommand]
        private async Task GoToRegisterView4()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            var today = DateTime.Today;
            var age = today.Year - Birthdate.Year;
            if (Birthdate.Date > today.AddYears(-age)) age--;
            if (age < 18)
            {
                ErrorMessage = "You must be older than 18 years old to use this app.";
            }
            else if (String.IsNullOrEmpty(SelectedGender))
            {
                ErrorMessage = "Please select your gender.";
            }
            else if (SelectedLocation == null)
            {
                ErrorMessage = "Please select your location.";
            }
            else if (!Regex.IsMatch(MobileNumber, @"^\d{10,11}$"))
            {
                ErrorMessage = "Phone Number must be 10-11 digits.";
            }
            else
            {
                ErrorMessage = string.Empty;
                IsBusy = false;
                await _navigationService.NavigateToAsync("RegisterView4");
            }
            IsBusy = false;
        }

        [RelayCommand]
        private async Task GoToRegisterView5()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (userRoleSelected == null)
            {
                ErrorMessage = "Select a role first.";
                IsBusy = false;
                return;
            }
            ErrorMessage = "";
            bool finalization = await _dialogService.ShowConfirmationAsync("Please confirm details:",
                $"First Name: {FirstName}\n" +
                $"Last Name: {LastName}\n" +
                $"Email: {Email}\n" +
                $"Birthdate: {Birthdate: MMMM d, yyyy}\n" +
                $"Gender: {SelectedGender}\n" +
                $"Location: {SelectedLocation}\n" +
                $"Phone Number: {MobileNumber}\n" +
                $"Role Selected: {userRoleSelected}\n",
                "Proceed",
                "Cancel"
                );
            if (finalization)
            {
                RegisterUserDto registerUserDto = new RegisterUserDto
                {
                    UserFirstName = FirstName,
                    UserLastName = LastName,
                    UserEmail = Email,
                    UserPassword = Password,
                    UserPhone = MobileNumber,
                    UserLocation = SelectedLocation,
                    UserBirthDate = Birthdate,
                    UserGender = SelectedGender,
                    UserRole = userRoleSelected,
                    JoinedOn = DateTime.Now
                };
                var response = await _accountServiceClient.RegisterAsync(registerUserDto);
                if (response.Success)
                {
                    IsBusy = false;
                    await _navigationService.NavigateToAsync("RegisterView5");
                }
                else
                {
                    IsBusy = false;
                    await _dialogService.ShowAlertAsync("Please Try Again", "The server is having issues processing your registration. Please try again.", "Ok");
                }
            }
            IsBusy = false;
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            MobileNumber = string.Empty;
            SelectedLocation = null;
            Birthdate = DateTime.Now;
            SelectedGender = string.Empty;
            userRoleSelected = null;
            SkillProviderSelected = false;
            ProjectOwnerSelected = false;
            await _navigationService.NavigateAndClearStackAsync("LoginView");
        }

        [RelayCommand]
        private void SelectSkillProvider()
        {
            if (IsBusy)
                return;
            SkillProviderSelected = true;
            ProjectOwnerSelected = false;
            userRoleSelected = UserRole.SkillProvider;
        }

        [RelayCommand]
        private void SelectProjectOwner()
        {
            if (IsBusy)
                return;
            SkillProviderSelected = false;
            ProjectOwnerSelected = true;
            userRoleSelected = UserRole.ProjectOwner;

        }
    }
}
