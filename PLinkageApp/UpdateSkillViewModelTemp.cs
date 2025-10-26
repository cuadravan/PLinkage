using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace PLinkageApp.ViewModels
{
    public partial class UpdateSkillViewModelTemp : ObservableObject
    {
        [ObservableProperty] private string skillName = "C# Development";
        [ObservableProperty] private string skillDescription = "Building applications using .NET MAUI and C#.";
        [ObservableProperty] private string skillLevel = "4";
        [ObservableProperty] private DateTime timeAcquired = new DateTime(2021, 5, 1);
        [ObservableProperty] private string organizationInvolved = "Tech Solutions Inc.";
        [ObservableProperty] private string yearsOfExperience = "3";

        [RelayCommand]
        private async Task UpdateSkillAsync()
        {
            await App.Current.MainPage.DisplayAlert("Updated", $"Skill '{skillName}' updated successfully!", "OK");
        }

        [RelayCommand]
        private void Reset()
        {
            skillName = string.Empty;
            skillDescription = string.Empty;
            skillLevel = string.Empty;
            organizationInvolved = string.Empty;
            yearsOfExperience = string.Empty;
            timeAcquired = DateTime.Today;
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
