using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.ViewsAndroid; // ✅ gives access to RegisterPage2 class
using Microsoft.Maui.Controls; // ✅ gives access to Application and Shell classes

namespace PLinkageApp.ViewModels
{
    public partial class RegisterPage1ViewModel : ObservableObject
    {
        // Properties bound to the Entry fields
        [ObservableProperty]
        private string firstName;

        [ObservableProperty]
        private string lastName;



        // Command for "Next" button
        public ICommand GoToNextCommand { get; }

        public RegisterPage1ViewModel()
        {
            GoToNextCommand = new RelayCommand(OnNext);
        }

        private async void OnNext()
        {
            // Optional: validate inputs before navigation
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                await Application.Current.MainPage.DisplayAlert("Missing Info", "Please enter both first and last names.", "OK");
                return;
            }

            // Navigate to the next register page
            //await Shell.Current.GoToAsync(nameof(RegisterPage2));
        }
    }
}
