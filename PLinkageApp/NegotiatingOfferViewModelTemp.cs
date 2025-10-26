using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using PLinkageApp.Interfaces;

namespace PLinkageApp.ViewModels
{
    public partial class NegotiatingOfferViewModelTemp : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public NegotiatingOfferViewModelTemp(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Dummy data for display
            projectName = "Selling Cookies";
            skillProviderName = "Van Kristian Cuadra";
            requestedRate = "";
            requestedTimeFrame = "";
        }

        [ObservableProperty] private string projectName;
        [ObservableProperty] private string skillProviderName;
        [ObservableProperty] private string requestedRate;
        [ObservableProperty] private string requestedTimeFrame;

        [RelayCommand]
        private async Task Negotiate()
        {
            await Shell.Current.DisplayAlert("Negotiation Sent", "Your negotiation request has been submitted!", "OK");
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
