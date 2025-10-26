using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using System.Threading.Tasks;

namespace PLinkageApp.ViewModels
{
    public partial class SendOfferViewModelTemp : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public SendOfferViewModelTemp(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Dummy data
            projectName = "Selling Cookies";
            skillProviderName = "Van Cuadra";
            requestedRate = "150";
            requestedTimeFrame = "240";
        }

        [ObservableProperty] private string projectName;
        [ObservableProperty] private string skillProviderName;
        [ObservableProperty] private string requestedRate;
        [ObservableProperty] private string requestedTimeFrame;

        [RelayCommand]
        private async Task SendOffer()
        {
            await Shell.Current.DisplayAlert("Offer Sent", "Your offer has been successfully sent.", "OK");
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        private async Task GoBack()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
