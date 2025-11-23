using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ForceReset), "ForceReset")]
    public partial class ProjectOwnerProfileViewModel : ObservableObject
    {        
        private readonly ISessionService _sessionService;
        private readonly IProjectOwnerServiceClient _projectOwnerServiceClient;
        private readonly INavigationService _navigationService;

        private bool _isInitialized = false;

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private ProjectOwnerDto projectOwnerDto;

        public Guid ProjectOwnerId { get; set; }
        public bool ForceReset { get; set; }

        public ProjectOwnerProfileViewModel(IProjectOwnerServiceClient projectOwnerServiceClient, ISessionService sessionService, ISkillProviderServiceClient skillProviderServiceClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _projectOwnerServiceClient = projectOwnerServiceClient;
        }       

        public async Task InitializeAsync() // Runs when navigating to the page
        {
            if (_isInitialized && !ForceReset) //If already initialized, or not a force reset, then don't initialize
                return;
            try
            {
                ForceReset = false;
                ProjectOwnerId = _sessionService.GetCurrentUserId();
                await LoadUserDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RefreshAsync() // Command executed when refreshed using RefreshView
        {
            await LoadUserDataAsync();
        }

        [RelayCommand]
        private async Task UpdateProfile()
        {
            if (!_isInitialized || IsBusy)
                return;
            await _navigationService.NavigateToAsync("UpdateProfileView");
            // Logic for updating profile, navigate to UpdateProfileView
        }

        [RelayCommand]
        private async Task AddProject()
        {
            if (!_isInitialized || IsBusy)
                return;
            await _navigationService.NavigateToAsync("AddProjectView");
        }

        [RelayCommand]
        private async Task UpdateProject(ProjectOwnerProfileProjectDto projectOwnerProfileProjectDto)
        {
            if (!_isInitialized || IsBusy)
                return;
            await _navigationService.NavigateToAsync("UpdateProjectView", new Dictionary<string, object> { { "ProjectId", projectOwnerProfileProjectDto.ProjectId } });  
        }

        [RelayCommand]
        private async Task ViewProject(ProjectOwnerProfileProjectDto projectOwnerProfileProjectDto)
        {
            if (!_isInitialized || IsBusy)
                return;
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", projectOwnerProfileProjectDto.ProjectId } });
        }

        [RelayCommand]
        private async Task ProcessResignation()
        {
            if (!_isInitialized || IsBusy)
                return;
            await _navigationService.NavigateToAsync("ProcessResignationView");
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
                    _isInitialized = true;
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
    }
}
