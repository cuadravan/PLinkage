using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;

namespace PLinkageApp.ViewModels
{
    public partial class AddSkillViewModelTemp : ObservableValidator
    {
        // 🧩 Form Fields
        [ObservableProperty]
        [Required(ErrorMessage = "Skill name is required.")]
        private string skillName;

        [ObservableProperty]
        [Required(ErrorMessage = "Description is required.")]
        private string skillDescription;

        [ObservableProperty]
        [Required(ErrorMessage = "Skill level is required.")]
        private string skillLevel;

        [ObservableProperty]
        [Required(ErrorMessage = "Time acquired is required.")]
        private DateTime timeAcquired = DateTime.Today;

        [ObservableProperty]
        [Required(ErrorMessage = "Organization involved is required.")]
        private string organizationInvolved;

        [ObservableProperty]
        [Required(ErrorMessage = "Years of experience is required.")]
        private string yearsOfExperience;

        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool isBusy;

        public AddSkillViewModelTemp()
        {
            // Optional default initialization
            timeAcquired = DateTime.Today;
        }
        // Validate on change
        // ✅ Real generated method signatures
        partial void OnSkillNameChanged(string oldValue, string newValue) => ValidateProperty(newValue, nameof(skillName));
        partial void OnSkillDescriptionChanged(string oldValue, string newValue) => ValidateProperty(newValue, nameof(skillDescription));
        partial void OnSkillLevelChanged(string oldValue, string newValue) => ValidateProperty(newValue, nameof(skillLevel));
        partial void OnOrganizationInvolvedChanged(string oldValue, string newValue) => ValidateProperty(newValue, nameof(organizationInvolved));
        partial void OnYearsOfExperienceChanged(string oldValue, string newValue) => ValidateProperty(newValue, nameof(yearsOfExperience));
        // ✅ Add Skill Command
        [RelayCommand]
        private async Task AddSkillAsync()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                errorMessage = GetErrors()
                    .OfType<ValidationResult>()
                    .FirstOrDefault()?.ErrorMessage ?? "Please fill all required fields.";
                return;
            }

            isBusy = true;
            try
            {
                await Task.Delay(1000); // simulate save
                await App.Current.MainPage.DisplayAlert("Success", $"Skill '{skillName}' added successfully!", "OK");

                // Reset form
                skillName = string.Empty;
                skillDescription = string.Empty;
                skillLevel = string.Empty;
                organizationInvolved = string.Empty;
                yearsOfExperience = string.Empty;
                timeAcquired = DateTime.Today;
                errorMessage = string.Empty;
            }
            finally
            {
                isBusy = false;
            }
        }

        // 🚪 Cancel Command
        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
