using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.ViewsAndroid; // gives access to RegisterPage4 class 

namespace PLinkageApp.ViewModels
{
    public partial class RegisterPage3ViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime birthdate = DateTime.Today;

        [ObservableProperty]
        private string selectedGender;

        [ObservableProperty]
        private string selectedLocation;

        [ObservableProperty]
        private string mobileNumber;

        public ObservableCollection<string> GenderOptions { get; } =
            new ObservableCollection<string> { "Male", "Female", "Other" };

        public ObservableCollection<string> LocationOptions { get; } =
            new ObservableCollection<string>
            {
                "Cebu City",
                "Manila",
                "Davao City",
                "Iloilo City",
                "Baguio City"
            };

        public ICommand GoToNextCommand { get; }

        public RegisterPage3ViewModel()
        {
            GoToNextCommand = new RelayCommand(OnNext);
        }

        private async void OnNext()
        {
            if (string.IsNullOrWhiteSpace(mobileNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Missing Info", "Please enter your mobile number.", "OK");
                return;
            }

            // Navigate to next registration step
            //await Shell.Current.GoToAsync(nameof(RegisterPage4));
        }
    }
}
