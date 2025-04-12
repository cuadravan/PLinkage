using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkage.Interfaces;
using PLinkage.Models;
using PLinkage.Views;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PLinkage.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterViewModel(INavigationService navigationService, IUnitOfWork unitOfWork)
        {
            _navigationService = navigationService;
            _unitOfWork = unitOfWork;
        }

        [ObservableProperty] private string firstName;
        [ObservableProperty] private string lastName;
        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private string confirmPassword;
        [ObservableProperty] private DateTime birthdate = DateTime.Now;
        [ObservableProperty] private bool isMale;
        [ObservableProperty] private bool isFemale;
        [ObservableProperty] private string mobileNumber;
        [ObservableProperty] private CebuLocation? selectedLocation;
        [ObservableProperty] private string selectedRole;

        [ObservableProperty] private string errorMessage; // 👈 error display

        public ObservableCollection<CebuLocation> CebuLocations { get; } =
            new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());

        public ObservableCollection<string> Roles { get; } = new()
        {
            "Skill Provider",
            "Project Owner"
        };

        private bool ValidateForm()
        {
            ErrorMessage = string.Empty; // Reset message

            if (!Regex.IsMatch(FirstName ?? "", @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$"))
                return SetError("Please enter a valid First Name.");

            if (!Regex.IsMatch(LastName ?? "", @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$"))
                return SetError("Please enter a valid Last Name.");

            if (!Regex.IsMatch(Email ?? "", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                return SetError("Please enter a valid Email Address.");

            if (!Regex.IsMatch(Password ?? "", @"^.{8,}$"))
                return SetError("Password must be at least 8 characters.");

            if (Password != ConfirmPassword)
                return SetError("Passwords do not match.");

            if (!(IsMale || IsFemale))
                return SetError("Please select a gender.");

            if (!SelectedLocation.HasValue)
                return SetError("Please select a location.");

            if (string.IsNullOrWhiteSpace(SelectedRole))
                return SetError("Please select a role.");

            if (!string.IsNullOrWhiteSpace(MobileNumber) && !Regex.IsMatch(MobileNumber, @"^\d{10,11}$"))
                return SetError("Mobile number must be 10–11 digits.");

            return true;
        }

        private bool SetError(string message)
        {
            ErrorMessage = message;
            return false;
        }

        [RelayCommand]
        private async Task Register()
        {
            if (!ValidateForm())
                return;

            if (SelectedRole == "Skill Provider")
            {
                var skillProvider = new SkillProvider
                {
                    UserFirstName = FirstName,
                    UserLastName = LastName,
                    UserEmail = Email,
                    UserPassword = Password,
                    UserBirthDate = Birthdate.Date,
                    UserGender = IsMale ? "Male" : "Female",
                    UserPhone = MobileNumber,
                    UserLocation = SelectedLocation,
                    JoinedOn = DateTime.Now,
                };

                await _unitOfWork.SkillProvider.AddAsync(skillProvider);
            }
            else if (SelectedRole == "Project Owner")
            {
                var projectOwner = new ProjectOwner
                {
                    UserFirstName = FirstName,
                    UserLastName = LastName,
                    UserEmail = Email,
                    UserPassword = Password,
                    UserBirthDate = Birthdate.Date,
                    UserGender = IsMale ? "Male" : "Female",
                    UserPhone = MobileNumber,
                    UserLocation = SelectedLocation,
                    JoinedOn = DateTime.Now,
                };

                await _unitOfWork.ProjectOwner.AddAsync(projectOwner);
            }

            await _unitOfWork.SaveChangesAsync();
            ErrorMessage = string.Empty;

            await _navigationService.NavigateToAsync(nameof(LoginView));
        }

        [RelayCommand]
        private async Task Clear()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            IsFemale = false;
            IsMale = false;
            SelectedLocation = null;
            MobileNumber = string.Empty;
            SelectedRole = null;
            Birthdate = DateTime.Now;
            ErrorMessage = string.Empty;
        }
    }
}
