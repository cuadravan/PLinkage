using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PLinkageApp.Models;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    public partial class AddEducationViewModel : ObservableValidator
    {
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        public AddEducationViewModel(ISkillProviderServiceClient skillProviderServiceClient, ISessionService sessionService, INavigationService navigationService)
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
        [Required(ErrorMessage = "Month and Year Graduated is required.")]
        private DateTime timeGraduated;

        [ObservableProperty]
        private string errorMessage;

        // Commands
        [RelayCommand]
        private async Task AddEducation()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                ErrorMessage = GetErrors()
                    .OfType<ValidationResult>()
                    .FirstOrDefault()?.ErrorMessage ?? "Validation failed.";
                return;
            }

            var userId = _sessionService.GetCurrentUserId();

            var educationDto = new EducationDto
            {
                CourseName = CourseName,
                SchoolAttended = SchoolAttended,
                TimeGraduated = TimeGraduated
            };

            try
            {
                var result = await _skillProviderServiceClient.AddEducationAsync(userId, educationDto);
                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "Successfully added education!", "Ok");
                    await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", true } });
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed", $"Server returned following error. {result.Message}. Please try again.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Education addition failed due to following error: {ex}. Please try again.", "Ok");
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", false } });
        }
    }
}
