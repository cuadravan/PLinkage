using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using PLinkageApp.Interfaces;

namespace PLinkageApp.ViewModels
{
    public partial class ApplyViewModelTemp : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public ApplyViewModelTemp(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Dummy sample data for display
            projectName = "Selling Cookies";
            skillProviderName = "Van Kristian Cuadra";
            requestedRate = "";
            requestedTimeFrame = "";
        }

        // Fields
        [ObservableProperty] private string projectName;
        [ObservableProperty] private string skillProviderName;
        [ObservableProperty] private string requestedRate;
        [ObservableProperty] private string requestedTimeFrame;

        // Commands
        [RelayCommand]
        private async Task Apply()
        {
            await Shell.Current.DisplayAlert("Application Sent", "Your application has been submitted successfully!", "OK");
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
