using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.ViewsAndroid; // ✅ gives access to RegisterPage2 class
using Microsoft.Maui.Controls; // ✅ gives access to Application and Shell classes

namespace PLinkageApp.ViewModels
{
    public partial class RegisterPage2ViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        public ICommand GoToNextCommand { get; }

        public RegisterPage2ViewModel()
        {
            GoToNextCommand = new RelayCommand(OnNext);
        }

        private async void OnNext()
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Missing Info", "Please fill out all fields.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            if (password.Length < 8)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password must be at least 8 characters long.", "OK");
                return;
            }

            // Go to RegisterPage3
            await Shell.Current.GoToAsync(nameof(RegisterPage3));
        }
    }
}
