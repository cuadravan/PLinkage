using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using System.Collections.ObjectModel;

namespace PLinkageApp.ViewModels
{
    public partial class AddSkillViewModel : ObservableValidator
    {
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        // Form Fields
        [ObservableProperty]
        [Required(ErrorMessage = "Skill Name is required.")]
        private string skillName;

        [ObservableProperty]
        [Required(ErrorMessage = "Description is required.")]
        private string skillDescription;

        [ObservableProperty]
        [Required(ErrorMessage = "Skill level is required.")]
        [Range(1, 5, ErrorMessage = "Skill level must be between 1 and 5.")]
        private int skillLevel;

        [ObservableProperty]
        [Required(ErrorMessage = "Time acquired is required.")]
        private DateTime timeAcquired;

        [ObservableProperty]
        [Required(ErrorMessage = "Organization is required.")]
        private string organizationInvolved;

        [ObservableProperty]
        [Required(ErrorMessage = "Years of experience is required.")]
        [Range(0, 100, ErrorMessage = "Years of experience must be between 1 and 100.")]
        private int yearsOfExperience;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isBusy = false;

        public ObservableCollection<int> SkillLevelOptions { get; } = new ObservableCollection<int>()
        {
            1, 2, 3, 4, 5
        };

        public AddSkillViewModel(IDialogService dialogService, ISkillProviderServiceClient skillProviderServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _skillProviderServiceClient = skillProviderServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            TimeAcquired = DateTime.Today;
        }

        // Save Command
        [RelayCommand]
        private async Task AddSkill()
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

            // Validate time acquired vs years of experience
            if (!ValidateExperienceDates())
            {
                return;
            }

            var userId = _sessionService.GetCurrentUserId();

            var skillDto = new SkillDto
            {
                SkillName = SkillName,
                SkillDescription = SkillDescription,
                SkillLevel = SkillLevel,
                TimeAcquired = TimeAcquired,
                OrganizationInvolved = OrganizationInvolved,
                YearsOfExperience = YearsOfExperience
            };
            IsBusy = true;
            try
            {
                var result = await _skillProviderServiceClient.AddSkillAsync(userId, skillDto);
                if (result.Success)
                {
                    await _dialogService.ShowAlertAsync("Success", "Successfully added skill!", "Ok");
                    await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", true } });
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Failed", $"Server returned following error. {result.Message}. Please try again.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Failed", $"Skill addition failed due to following error: {ex}. Please try again.", "Ok");
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

        private bool ValidateExperienceDates()
        {
            // Calculate the expected start year based on years of experience
            var expectedStartYear = DateTime.Now.Year - YearsOfExperience;

            // Check if the acquired date makes sense with the years of experience
            if (TimeAcquired.Year > expectedStartYear)
            {
                ErrorMessage = $"Years of experience ({YearsOfExperience}) doesn't match the acquired date. " +
                             $"Based on your experience, the skill should have been acquired by {expectedStartYear} or earlier.";
                return false;
            }

            // Check if the acquired date is in the future
            if (TimeAcquired > DateTime.Now)
            {
                ErrorMessage = "Time acquired cannot be in the future.";
                return false;
            }

            return true;
        }
    }
}