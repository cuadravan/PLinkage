using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkage.Interfaces;
using PLinkage.Models;
using PLinkage.Views;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace PLinkage.ViewModels
{
    public partial class UpdateProfileViewModel: ObservableValidator
    {
        [ObservableProperty, Required(ErrorMessage = "First Name is required"),
         RegularExpression(@"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$", ErrorMessage = "Please enter a valid First Name.")]
        private string firstName;

        [ObservableProperty, Required(ErrorMessage = "Last Name is required"),
         RegularExpression(@"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$", ErrorMessage = "Please enter a valid Last Name.")]
        private string lastName;

        [ObservableProperty]
        private DateTime birthdate = DateTime.Now;

        [ObservableProperty]
        private bool isMale;

        [ObservableProperty]
        private bool isFemale;

        [ObservableProperty,
         RegularExpression(@"^\d{10,11}$", ErrorMessage = "Mobile number must be 10–11 digits.")]
        private string mobileNumber;

        [ObservableProperty]
        private CebuLocation? selectedLocation;

        [ObservableProperty]
        private string errorMessage;

        public ObservableCollection<CebuLocation> CebuLocations { get; } =
            new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());

        private readonly IUnitOfWork _unitOfWork;
        private readonly INavigationService _navigationService;
        private readonly ISessionService _sessionService;

        public UpdateProfileViewModel(IUnitOfWork unitOfWork, INavigationService navigationService, ISessionService sessionService)
        {
            _unitOfWork = unitOfWork;
            _navigationService = navigationService;
            _sessionService = sessionService;
            LoadCurrentProfile();
        }

        private async Task LoadCurrentProfile()
        {
            var user = await _unitOfWork.ProjectOwner.GetByIdAsync(_sessionService.GetCurrentUser().UserId);
            FirstName = user.UserFirstName;
            LastName = user.UserLastName;
            IsMale = user.UserGender == "Male" ? true : false;
            IsFemale = user.UserGender == "Female" ? true : false;
            Birthdate = user.UserBirthDate;
            MobileNumber = user.UserPhone;
            SelectedLocation = user.UserLocation;
        }

        private bool ValidateForm()
        {
            ErrorMessage = string.Empty;
            ValidateAllProperties();

            if (HasErrors)
            {
                var firstError = GetErrors()
                    .OfType<ValidationResult>()
                    .FirstOrDefault();

                if (firstError != null)
                    ErrorMessage = firstError.ErrorMessage;

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

        [RelayCommand]
        private async Task Update()
        {
            if (!ValidateForm())
                return;

            // Load all existing users from both repositories
            var skillProviders = await _unitOfWork.SkillProvider.GetAllAsync();
            var projectOwners = await _unitOfWork.ProjectOwner.GetAllAsync();

            var user = await _unitOfWork.ProjectOwner.GetByIdAsync(_sessionService.GetCurrentUser().UserId);

            user.UserFirstName = FirstName;
            user.UserLastName = LastName;
            user.UserBirthDate = Birthdate;
            user.UserGender = IsMale ? "Male" : "Female";
            user.UserPhone = MobileNumber;
            user.UserLocation = SelectedLocation;
            
            await _unitOfWork.ProjectOwner.UpdateAsync(user);

            await _unitOfWork.SaveChangesAsync();
            ErrorMessage = string.Empty;

            _sessionService.SetCurrentUser(user);

            await _navigationService.NavigateToAsync(nameof(ProjectOwnerProfileView));
        }


        [RelayCommand]
        private async Task Clear()
        {
            await LoadCurrentProfile();

        }

        [RelayCommand]
        private async Task BackToProfile()
        {
            await _navigationService.NavigateToAsync(nameof(ProjectOwnerProfileView));
        }
    
    }
}
