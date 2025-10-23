using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp
{
    [QueryProperty(nameof(ProjectId), "ProjectId")]
    public partial class ViewProjectViewModelTemp : ObservableObject
    {
        public Guid ProjectId { get; set; }

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        public ProjectDto project;

        private bool _isInitialized;

        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly INavigationService _navigationService;

        public ViewProjectViewModelTemp(ISessionService sessionService, IProjectServiceClient projectServiceClient, INavigationService navigationService)
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
