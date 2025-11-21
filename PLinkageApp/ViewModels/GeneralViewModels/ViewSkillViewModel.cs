using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(SkillIndex), "SkillIndex")]
    [QueryProperty(nameof(SkillProviderId), "SkillProviderId")]
    public partial class ViewSkillViewModel : ObservableValidator
    {
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private bool _isUpdated = false;
        public int SkillIndex { get; set; }
        public Guid SkillProviderId { get; set; }
        [ObservableProperty]
        public bool isOwner = false;

        public SkillDto specificSkillDto;

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

        public ObservableCollection<int> SkillLevelOptions { get; } = new ObservableCollection<int>()
        {
            1, 2, 3, 4, 5
        };

        public ViewSkillViewModel(ISkillProviderServiceClient skillProviderServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _skillProviderServiceClient = skillProviderServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
        }

        // Load education
        public async Task InitializeAsync()
        {
            var userId = _sessionService.GetCurrentUserId();
            if(userId == SkillProviderId)
            {
                IsOwner = true;
            }
            try
            {
                var result = await _skillProviderServiceClient.GetSpecificAsync(SkillProviderId);
                if (result.Success && result.Data != null)
                {
                    specificSkillDto = result.Data.Skills[SkillIndex];
                    await Reset();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "It seems the skill you are trying to access does not exist. Please contact admin or refresh the app.", "Ok");
                    await _navigationService.GoBackAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Loading skill failed due to following error: {ex}. Please try again.", "Ok");
                await _navigationService.GoBackAsync();
            }
        }

        // Commands
        [RelayCommand]
        private async Task UpdateSkill()
        {
            if (!IsOwner)
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

            ErrorMessage = string.Empty;

            var skillDto = new SkillDto
            {
                SkillName = SkillName,
                SkillDescription = SkillDescription,
                SkillLevel = SkillLevel,
                OrganizationInvolved = OrganizationInvolved,
                TimeAcquired = TimeAcquired,
                YearsOfExperience = YearsOfExperience
            };
            var userId = _sessionService.GetCurrentUserId();
            try
            {
                var result = await _skillProviderServiceClient.UpdateSkillAsync(userId, SkillIndex, skillDto);
                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "Skill updated successfully.", "Ok");
                    _isUpdated = true;
                    specificSkillDto.SkillName = SkillName;
                    specificSkillDto.SkillLevel = SkillLevel;
                    specificSkillDto.SkillDescription = SkillDescription;
                    specificSkillDto.TimeAcquired = TimeAcquired;
                    specificSkillDto.YearsOfExperience = YearsOfExperience;
                    specificSkillDto.OrganizationInvolved = OrganizationInvolved;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"Skill could not be updated. Server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Skill update failed due to following error: {ex}. Please try again.", "Ok");
            }
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

        [RelayCommand]
        private async Task DeleteSkill()
        {
            if (!IsOwner)
                return;
            var userId = _sessionService.GetCurrentUserId();
            try
            {
                var result = await _skillProviderServiceClient.DeleteSkillAsync(userId, SkillIndex);
                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "Skill deleted successfully.", "Ok");
                    _isUpdated = true;
                    await Return();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"Skill could not be deleted. Server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Skill deletion failed due to following error: {ex}. Please try again.", "Ok");
            }
        }

        [RelayCommand]
        private async Task Reset()
        {
            SkillName = specificSkillDto.SkillName;
            SkillLevel = specificSkillDto.SkillLevel;
            SkillDescription = specificSkillDto.SkillDescription;
            OrganizationInvolved = specificSkillDto.OrganizationInvolved;
            TimeAcquired = specificSkillDto.TimeAcquired;
            YearsOfExperience = specificSkillDto.YearsOfExperience;
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
