using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    public partial class AddEducationViewModel : ObservableValidator
    {
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

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
        [ObservableProperty]
        private bool isBusy = false;

        public AddEducationViewModel(IDialogService dialogService, ISkillProviderServiceClient skillProviderServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _skillProviderServiceClient = skillProviderServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            TimeGraduated = DateTime.Today;
        }

        

        // Commands
        [RelayCommand]
        private async Task AddEducation()
        {
            if (IsBusy)
                return;
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
            IsBusy = true;
            try
            {
                var result = await _skillProviderServiceClient.AddEducationAsync(userId, educationDto);
                if (result.Success)
                {
                    await _dialogService.ShowAlertAsync("Success", "Successfully added education!", "Ok");
                    await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", true } });
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Failed", $"Server returned following error. {result.Message}. Please try again.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Failed", $"Education addition failed due to following error: {ex}. Please try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            if (IsBusy)
                return;
            await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", false } });
        }
    }
}
