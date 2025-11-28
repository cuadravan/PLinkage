using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ProjectId), "ProjectId")]
    [QueryProperty(nameof(ForceReset), "ForceReset")]
    public partial class ViewProjectViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private bool _isInitialized = false;

        [ObservableProperty]
        private bool isBusy = false;
        [ObservableProperty]
        private bool isOwner = false;
        [ObservableProperty]
        private bool isEmployed = false;
        [ObservableProperty]
        private bool canApply = false;
        [ObservableProperty]
        private ProjectDto project;

        public Guid ProjectId { get; set; } = Guid.Empty;
        public bool ForceReset { get; set; }

        public ViewProjectViewModel(IDialogService dialogService, ISessionService sessionService, IProjectServiceClient projectServiceClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _projectServiceClient = projectServiceClient;
            _dialogService = dialogService;
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized && !ForceReset) //If already initialized, or not a force reset, then don't initialize
                return;
            ForceReset = false;
            try
            {
                await LoadProjectDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadProjectDataAsync();
        }

        [RelayCommand]
        private async Task Resign()
        {
            if (!_isInitialized)
                return;
            if (!IsEmployed)
                return;
            await _navigationService.NavigateToAsync("ResignProjectView", new Dictionary<string, object> { { "ProjectId", Project.ProjectId } });
        }

        [RelayCommand]
        private async Task Apply()
        {
            if (!_isInitialized)
                return;
            if (_sessionService.GetCurrentUserRole() != UserRole.SkillProvider)
                return;
            await _navigationService.NavigateToAsync("ApplyView", new Dictionary<string, object> { { "ProjectId", Project.ProjectId } });
        }

        [RelayCommand]
        private async Task UpdateProject()
        {
            if (!_isInitialized)
                return;
            if (_sessionService.GetCurrentUserRole() != UserRole.ProjectOwner)
                return;
            if (project.ProjectStatus == "Completed")
            {
                await _dialogService.ShowAlertAsync("Cannot Update", "A completed project cannot be updated.", "Ok");
                return;
             }             
            await _navigationService.NavigateToAsync("UpdateProjectView", new Dictionary<string, object> { { "ProjectId", Project.ProjectId } });
        }

        [RelayCommand]
        private async Task ViewProjectOwner()
        {
            await _navigationService.NavigateToAsync("ViewProjectOwnerProfileView", new Dictionary<string, object> { { "ProjectOwnerId", project.ProjectOwnerId } });
        }

        [RelayCommand]
        private async Task ViewSkillProvider(ProjectMemberDetailDto projectMemberDetailDto)
        {
            await _navigationService.NavigateToAsync("ViewSkillProviderProfileView", new Dictionary<string, object> { { "SkillProviderId", projectMemberDetailDto.MemberId } });
        }

        private async Task LoadProjectDataAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {

                ApiResponse<ProjectDto> result = null;
                IsOwner = false;
                IsEmployed = false;
                CanApply = false;

                result = await _projectServiceClient.GetSpecificAsync(ProjectId);

                if (result.Success && result.Data != null)
                {
                    
                    Project = result.Data;
                    var userId = _sessionService.GetCurrentUserId();
                    var userRole = _sessionService.GetCurrentUserRole();
                    
                    if (userId == Project.ProjectOwnerId && Project.ProjectStatus != "Completed")
                        IsOwner = true; // Owner can update Active or Deactivated project
                    else if (Project.ProjectMembers.Any(pm => pm.MemberId == userId) && Project.ProjectStatus == "Active")
                        IsEmployed = true; // Members can resign from active projects
                    else if (!IsEmployed && userRole == UserRole.SkillProvider && Project.ProjectStatus == "Active")
                        CanApply = true; // SP can apply to active projects
                    _isInitialized = true;
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Failed to Fetch Project", $"The server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting project: {ex.Message}");
                await _dialogService.ShowAlertAsync("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
