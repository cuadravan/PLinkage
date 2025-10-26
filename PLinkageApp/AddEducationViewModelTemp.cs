using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using PLinkageApp.Interfaces;

namespace PLinkageApp.ViewModels
{
    public partial class AddEducationViewModelTemp : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public AddEducationViewModelTemp(INavigationService navigationService)
        {
            _navigationService = navigationService;
            timeGraduated = DateTime.Today;
        }

        // Properties
        [ObservableProperty] private string courseName;
        [ObservableProperty] private string schoolAttended;
        [ObservableProperty] private DateTime timeGraduated;

        // Commands
        [RelayCommand]
        private async Task AddEducation()
        {
            // Simulate saving logic
            await Shell.Current.DisplayAlert("Success", "Education added successfully!", "OK");
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
