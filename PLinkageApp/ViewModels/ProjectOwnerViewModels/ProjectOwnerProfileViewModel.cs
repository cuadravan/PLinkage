using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Models;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ForceReset), "ForceReset")]
    public partial class ProjectOwnerProfileViewModel : ObservableObject
    {
        public Guid ProjectOwnerId { get; set; }
        bool _forceReset;

        public bool ForceReset
        {
            get => _forceReset;
            set
            {
                _forceReset = value;
                if (_forceReset)
                {
                    _ = InitializeAsync(); // Trigger logic when property is set
                }
            }
        }

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        public ProjectOwnerDto projectOwnerDto;

        private bool _isInitialized = false;

        private readonly ISessionService _sessionService;
        private readonly IProjectOwnerServiceClient _projectOwnerServiceClient;
        private readonly INavigationService _navigationService;

        public ProjectOwnerProfileViewModel(IProjectOwnerServiceClient projectOwnerServiceClient, ISessionService sessionService, ISkillProviderServiceClient skillProviderServiceClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _projectOwnerServiceClient = projectOwnerServiceClient;
        }

        [RelayCommand]
        private async Task RefreshAsync() // Command executed when refreshed using RefreshView
        {
            await LoadUserDataAsync();
        }

        public async Task InitializeAsync() // Runs when navigating to the page
        {
            if (_isInitialized && !ForceReset) //If already initialized, or not a force reset, then don't initialize
                return;
            try
            {
                ForceReset = false;
                _isInitialized = true;
                ProjectOwnerId = _sessionService.GetCurrentUserId();
                await LoadUserDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        private async Task LoadUserDataAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {

                ApiResponse<ProjectOwnerDto> result = null;

                result = await _projectOwnerServiceClient.GetSpecificAsync(ProjectOwnerId);

                if (result.Success && result.Data != null)
                {

                    ProjectOwnerDto = result.Data;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting project owner profile: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        public async Task UpdateProfile()
        {
            await _navigationService.NavigateToAsync("UpdateProfileView");
            // Logic for updating profile, navigate to UpdateProfileView
        }

        [RelayCommand]
        public async Task AddProject()
        {
            await _navigationService.NavigateToAsync("AddProjectView");
        }

        [RelayCommand]
        public async Task UpdateProject(ProjectOwnerProfileProjectDto projectOwnerProfileProjectDto)
        {
            await _navigationService.NavigateToAsync("UpdateProjectView", new Dictionary<string, object> { { "ProjectId", projectOwnerProfileProjectDto.ProjectId } });  
        }

        [RelayCommand]
        public async Task ViewProject(ProjectOwnerProfileProjectDto projectOwnerProfileProjectDto)
        {
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", projectOwnerProfileProjectDto.ProjectId } });
        }

        [RelayCommand]
        public async Task ProcessResignation()
        {
            await _navigationService.NavigateToAsync("ProcessResignationView");
        }
    }
}
