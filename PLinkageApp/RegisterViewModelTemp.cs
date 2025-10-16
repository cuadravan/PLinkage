using CommunityToolkit.Mvvm.ComponentModel;
using PLinkageApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PLinkageApp.Services;
using CommunityToolkit.Mvvm.Input;
using PLinkageShared.Enums;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using PLinkageShared.DTOs;

namespace PLinkageApp
{
    public partial class RegisterViewModelTemp: ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IAccountServiceClient _accountServiceClient;

        [ObservableProperty]
        public string firstName = string.Empty;

        [ObservableProperty]
        public string lastName = string.Empty;

        [ObservableProperty]
        public string email = string.Empty;

        [ObservableProperty]
        public string password = string.Empty;

        [ObservableProperty]
        public string confirmPassword = string.Empty;

        [ObservableProperty]
        public DateTime birthdate = DateTime.Today;

        [ObservableProperty]
        public string selectedGender = string.Empty;

        [ObservableProperty]
        public CebuLocation? selectedLocation = null;

        [ObservableProperty]
        public string mobileNumber = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ObservableCollection<CebuLocation> LocationOptions { get; } =
            new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());

        public ObservableCollection<string> GenderOptions { get; } = new()
        {
            "Male",
            "Female"
        };

        [ObservableProperty]
        public bool skillProviderSelected = false;

        [ObservableProperty]
        public bool projectOwnerSelected = false;

        private UserRole? userRoleSelected = null;

        public RegisterViewModelTemp(INavigationService navigationService, IAccountServiceClient accountServiceClient)
        {
            _navigationService = navigationService;
            _accountServiceClient = accountServiceClient;
        }
        [RelayCommand]
        private async Task GoToRegisterView2()
        {
            if (!Regex.IsMatch(FirstName, @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$"))
            {
                ErrorMessage = "First Name must be valid name.";
            }
            else if (!Regex.IsMatch(LastName, @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$"))
            {
                ErrorMessage = "Last Name must be valid name.";
            }
            else
            {
                ErrorMessage = string.Empty;
                await _navigationService.NavigateToAsync("RegisterView2");
            }              
        }

        [RelayCommand]
        private async Task GoToRegisterView3()
        {
            if (!String.IsNullOrEmpty(Email) && Regex.IsMatch(Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
            {
                var response = await _accountServiceClient.CheckEmailUniquenessAsync(Email);
                if (!response.Success)
                {
                    ErrorMessage = "Email is already in use.";
                    return;
                }
            }
            else
            {
                ErrorMessage = "Email must not be empty or invalid.";
                return;
            }
            if(!Regex.IsMatch(Password, "^.{8,}$"))
            {
                ErrorMessage = "Password must at least be 8 characters long.";
                return;
            }

            if(!String.Equals(Password, ConfirmPassword))
            {
                ErrorMessage = "Passwords must match.";
                return;
            }
            ErrorMessage = string.Empty;
            await _navigationService.NavigateToAsync("RegisterView3");
        }

        [RelayCommand]
        private async Task GoToRegisterView4()
        {
            var today = DateTime.Today;
            var age = today.Year - Birthdate.Year;
            if (Birthdate.Date > today.AddYears(-age)) age--;
            if (age < 18)
            {
                ErrorMessage = "You must be older than 18 years old to use this app.";
                return;
            }
            if (String.IsNullOrEmpty(SelectedGender))
            {
                ErrorMessage = "Please select your gender.";
                return;
            }
            if (SelectedLocation == null)
            {
                ErrorMessage = "Please select your location.";
                return;
            }
            if(!Regex.IsMatch(MobileNumber, @"^\d{10,11}$"))
            {
                ErrorMessage = "Phone Number must be 10-11 digits.";
                return;
            }
            ErrorMessage = string.Empty;
            await _navigationService.NavigateToAsync("RegisterView4");
        }

        [RelayCommand]
        private async Task GoToRegisterView5()
        {
            if (userRoleSelected == null)
            {
                ErrorMessage = "Select a role first.";
                return;
            }
            ErrorMessage = "";
            bool finalization = await Shell.Current.DisplayAlert("Please confirm details:",
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
                    await _navigationService.NavigateToAsync("RegisterView5");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Please Try Again", "The server is having issues processing your registration. Please try again.", "Ok");
                }
            }
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            // Erase stack and go back to loginview
            await _navigationService.NavigateAndClearStackAsync("LoginView");
        }

        [RelayCommand]
        private void SelectSkillProvider()
        {
            SkillProviderSelected = true;
            ProjectOwnerSelected = false;
            userRoleSelected = UserRole.SkillProvider;
        }

        [RelayCommand]
        private void SelectProjectOwner()
        {
            SkillProviderSelected = false;
            ProjectOwnerSelected = true;
            userRoleSelected = UserRole.ProjectOwner;

        }
    }
}
