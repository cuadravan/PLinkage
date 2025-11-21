using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PLinkageApp.Models;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(EducationIndex), "EducationIndex")]
    public partial class UpdateEducationViewModel : ObservableValidator
    {
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        public int EducationIndex { get; set; }

        private EducationDto _specificEducationDto;

        public UpdateEducationViewModel(ISkillProviderServiceClient skillProviderServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _skillProviderServiceClient = skillProviderServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
            TimeGraduated = DateTime.Today;
        }

        // Fields
        [ObservableProperty]
        [Required(ErrorMessage = "Course Name is required.")]
        private string courseName;

        [ObservableProperty]
        [Required(ErrorMessage = "School is required.")]
        private string schoolAttended;

        [ObservableProperty]
        [Required(ErrorMessage = "Year Graduated is required.")]
        private DateTime timeGraduated;

        [ObservableProperty]
        private string errorMessage;

        private bool _isUpdated = false;

        // Load education
        public async Task InitializeAsync()
        {
            var userId = _sessionService.GetCurrentUserId();
            var educationIndex = EducationIndex;
            try
            {
                var result = await _skillProviderServiceClient.GetSpecificAsync(userId);
                if (result.Success && result.Data != null)
                {
                    _specificEducationDto = result.Data.Educations[EducationIndex];
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "It seems the education you are trying to access does not exist. Please contact admin or refresh the app.", "Ok");
                    await _navigationService.GoBackAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Loading education failed due to following error: {ex}. Please try again.", "Ok");
            }

            CourseName = _specificEducationDto.CourseName;
            SchoolAttended = _specificEducationDto.SchoolAttended;
            TimeGraduated = _specificEducationDto.TimeGraduated;
        }

        // Commands
        [RelayCommand]
        private async Task UpdateEducation()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                ErrorMessage = GetErrors()
                    .OfType<ValidationResult>()
                    .FirstOrDefault()?.ErrorMessage ?? "Validation failed.";
                return;
            }

            var educationDto = new EducationDto
            {
                TimeGraduated = TimeGraduated,
                SchoolAttended = SchoolAttended,
                CourseName = CourseName
            };
            var userId = _sessionService.GetCurrentUserId();
            try
            {
                var result = await _skillProviderServiceClient.UpdateEducationAsync(userId, EducationIndex, educationDto);
                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "Education updated successfully.", "Ok");
                    _isUpdated = true;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"Education could not be updated. Server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Education update failed due to following error: {ex}. Please try again.", "Ok");
            }
        }

        [RelayCommand]
        private async Task DeleteEducation()
        {
            var userId = _sessionService.GetCurrentUserId();
            try
            {
                var result = await _skillProviderServiceClient.DeleteEducationAsync(userId, EducationIndex);
                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "Education deleted successfully.", "Ok");
                    _isUpdated = true;
                    await Return();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"Education could not be deleted. Server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Education deletion failed due to following error: {ex}. Please try again.", "Ok");
            }
        }

        [RelayCommand]
        private async Task Return()
        {
            if (_isUpdated)
                await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", true } });
            else
                await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", false } });
        }
    }
}
