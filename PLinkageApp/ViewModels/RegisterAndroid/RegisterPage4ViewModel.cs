using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.ViewsAndroid;
using Microsoft.Maui.Graphics;

namespace PLinkageApp.ViewModels
{
    public partial class RegisterPage4ViewModel : ObservableObject
    {
        [ObservableProperty]
        public bool skillProviderSelected;

        [ObservableProperty]
        public bool projectOwnerSelected;

        [ObservableProperty]
        public Color skillProviderColor = Colors.White;

        [ObservableProperty]
        public Color projectOwnerColor = Colors.White;

        public ICommand ConfirmCommand { get; }

        public RegisterPage4ViewModel()
        {
            ConfirmCommand = new RelayCommand(OnConfirm);
        }

        private async void OnConfirm()
        {
            if (!SkillProviderSelected && !ProjectOwnerSelected)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Select a role",
                    "Please select your role before confirming.",
                    "OK");
                return;
            }

            string selectedRole = SkillProviderSelected ? "Skill Provider" : "Project Owner";

            await Application.Current.MainPage.DisplayAlert(
                "Role Selected",
                $"You have chosen: {selectedRole}",
                "OK");

            //await Shell.Current.GoToAsync(nameof(RegisterPage5));
        }

        [RelayCommand]
        private void SelectSkillProvider()
        {
            SkillProviderSelected = true;
            ProjectOwnerSelected = false;
            SkillProviderColor = new Color(0.9f, 0.85f, 1.0f); // light purple
            ProjectOwnerColor = Colors.White;
        }

        [RelayCommand]
        private void SelectProjectOwner()
        {
            SkillProviderSelected = false;
            ProjectOwnerSelected = true;
            ProjectOwnerColor = new Color(0.9f, 0.85f, 1.0f); // light purple
            SkillProviderColor = Colors.White;
        }
    }
}
