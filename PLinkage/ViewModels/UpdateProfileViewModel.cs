using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.ViewModels
{
    public partial class UpdateProfileViewModel : ObservableValidator
    {
        // Services
        private readonly IUnitOfWork _unitOfWork;
        private readonly INavigationService _navigationService;
        private readonly ISessionService _sessionService;
        

        // Fields
        [ObservableProperty, Required(ErrorMessage = "First Name is required"),
         RegularExpression(@"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$", ErrorMessage = "Please enter a valid First Name.")]
        private string firstName;

        [ObservableProperty, Required(ErrorMessage = "Last Name is required"),
         RegularExpression(@"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$", ErrorMessage = "Please enter a valid Last Name.")]
        private string lastName;

        [ObservableProperty] private DateTime birthdate = DateTime.Now;
        [ObservableProperty] private bool isMale;
        [ObservableProperty] private bool isFemale;

        [ObservableProperty,
         RegularExpression(@"^\d{10,11}$", ErrorMessage = "Mobile number must be 10–11 digits.")]
        private string mobileNumber;

        [ObservableProperty] private CebuLocation? selectedLocation;
        [ObservableProperty] private string errorMessage;

        public ObservableCollection<CebuLocation> CebuLocations { get; } =
            new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());

        // Constructor
        public UpdateProfileViewModel(IUnitOfWork unitOfWork, INavigationService navigationService, ISessionService sessionService)
        {
            _unitOfWork = unitOfWork;
            _navigationService = navigationService;
            _sessionService = sessionService;

            _ = LoadCurrentProfile();
        }

        // Core Methods
        private async Task LoadCurrentProfile()
        {
            var user = await _unitOfWork.ProjectOwner.GetByIdAsync(_sessionService.GetCurrentUser().UserId);
            if (user == null) return;

            FirstName = user.UserFirstName;
            LastName = user.UserLastName;
            Birthdate = user.UserBirthDate;
            MobileNumber = user.UserPhone;
            SelectedLocation = user.UserLocation;
            IsMale = user.UserGender == "Male";
            IsFemale = user.UserGender == "Female";
        }

        private bool ValidateForm()
        {
            ErrorMessage = string.Empty;
            ValidateAllProperties();

            if (HasErrors)
            {
                ErrorMessage = GetErrors()
                    .OfType<ValidationResult>()
                    .FirstOrDefault()?.ErrorMessage ?? "Validation failed.";
                return false;
            }

            if (!(IsMale || IsFemale))
                return SetError("Please select a gender.");

            if (!SelectedLocation.HasValue)
                return SetError("Please select a location.");

            return true;
        }

        private bool SetError(string message)
        {
            ErrorMessage = message;
            return false;
        }

        // Commands
        [RelayCommand]
        private async Task Update()
        {
            if (!ValidateForm()) return;

            var user = await _unitOfWork.ProjectOwner.GetByIdAsync(_sessionService.GetCurrentUser().UserId);
            if (user == null) return;

            user.UserFirstName = FirstName;
            user.UserLastName = LastName;
            user.UserBirthDate = Birthdate;
            user.UserGender = IsMale ? "Male" : "Female";
            user.UserPhone = MobileNumber;
            user.UserLocation = SelectedLocation;

            await _unitOfWork.ProjectOwner.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _sessionService.SetCurrentUser(user);
            ErrorMessage = string.Empty;

            await _navigationService.NavigateToAsync("ProjectOwnerProfileView");
        }

        [RelayCommand]
        private Task Clear() => LoadCurrentProfile();

        [RelayCommand]
        private Task BackToProfile() => _navigationService.NavigateToAsync("ProjectOwnerProfileView");
    }
}
