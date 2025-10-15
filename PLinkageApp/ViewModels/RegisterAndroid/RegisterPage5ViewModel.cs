using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.ViewsAndroid;

namespace PLinkageApp.ViewModels
{
    public partial class RegisterPage5ViewModel : ObservableObject
    {
        public ICommand GoToLoginCommand { get; }

        public RegisterPage5ViewModel()
        {
            GoToLoginCommand = new RelayCommand(OnGoToLogin);
        }

        private async void OnGoToLogin()
        {
            // Option 1: Navigate back to LoginView (Shell navigation)
            await Shell.Current.GoToAsync("//LoginView");

            // Option 2: If LoginView is not part of Shell routes, use this:
            // Application.Current.MainPage = new NavigationPage(new LoginView());
        }
    }
}
