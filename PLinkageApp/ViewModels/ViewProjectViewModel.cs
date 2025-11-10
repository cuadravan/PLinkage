using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Models;
using System.Globalization;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ProjectId), "ProjectId")]
    public partial class ViewProjectViewModel : ObservableObject
    {
        public Guid ProjectId { get; set; }

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private bool isOwner = true;

        [ObservableProperty]
        private bool isEmployed = false;

        [ObservableProperty]
        private bool isNotEmployed = false;

        [ObservableProperty]
        public ProjectDto project;

        private bool _isInitialized;

        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly INavigationService _navigationService;

        public ViewProjectViewModel(ISessionService sessionService, IProjectServiceClient projectServiceClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _projectServiceClient = projectServiceClient;
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadProjectDataAsync();
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;
            try
            {
                await LoadProjectDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        private async Task LoadProjectDataAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {

                ApiResponse<ProjectDto> result = null;

                result = await _projectServiceClient.GetSpecificAsync(ProjectId);

                if (result.Success && result.Data != null)
                {

                    Project = result.Data;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting project: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}

public class EmployedSkillProviderWrapper
{
    public Guid MemberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int TimeFrame { get; set; }
}
