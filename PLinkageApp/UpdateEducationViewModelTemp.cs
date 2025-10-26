using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using PLinkageApp.Interfaces;

namespace PLinkageApp.ViewModels
{
    public partial class UpdateEducationViewModelTemp : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public UpdateEducationViewModelTemp(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Dummy default values for testing
            courseName = "Remote Sensing Applications";
            schoolAttended = "University of Somewhere";
            timeGraduated = new DateTime(2022, 11, 12);
        }

        // Properties
        [ObservableProperty] private string courseName;
        [ObservableProperty] private string schoolAttended;
        [ObservableProperty] private DateTime timeGraduated;

        // Commands
        [RelayCommand]
        private async Task UpdateEducation()
        {
            // Simulate update logic
            await Shell.Current.DisplayAlert("Updated", "Education updated successfully!", "OK");
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
