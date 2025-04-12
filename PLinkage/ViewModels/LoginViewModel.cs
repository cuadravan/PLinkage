using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkage.Interfaces;
using PLinkage.Views;

namespace PLinkage.ViewModels // Next we add 
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await _navigationService.NavigateToAsync(nameof(RegisterView));
        }
    }

}
