using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using PLinkageApp.Interfaces;

namespace PLinkageApp.ViewModels
{
    public partial class ViewSkillViewModelTemp : ObservableObject
    {
        private readonly INavigationService _navigationService;

        // Constructor
        public ViewSkillViewModelTemp(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Dummy data for testing
            skillName = "Welding";
            skillDescription = "I can weld.";
            skillLevel = "3";
            timeAcquired = new DateTime(2010, 10, 10);
            organizationInvolved = "Myself";
            yearsOfExperience = "4";
        }

        // Properties
        [ObservableProperty] private string skillName;
        [ObservableProperty] private string skillDescription;
        [ObservableProperty] private string skillLevel;
        [ObservableProperty] private DateTime timeAcquired;
        [ObservableProperty] private string organizationInvolved;
        [ObservableProperty] private string yearsOfExperience;

        // Commands
        [RelayCommand]
        private async Task GoBack()
        {
            // Navigates back to the profile page
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        private async Task GoToUpdate()
        {
            // Navigates to UpdateSkillView
            await _navigationService.NavigateToAsync(nameof(ViewsAndroid.UpdateSkillView));
        }
    }
}
