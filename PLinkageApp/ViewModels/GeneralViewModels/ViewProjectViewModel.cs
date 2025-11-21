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
    [QueryProperty(nameof(ForceReset), "ForceReset")]
    public partial class ViewProjectViewModel : ObservableObject
    {

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

        public Guid ProjectId { get; set; }

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private bool isOwner = false;

        [ObservableProperty]
        private bool isEmployed = false;

        [ObservableProperty]
        private bool canApply = false;

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
            if (_isInitialized && !ForceReset) //If already initialized, or not a force reset, then don't initialize
                return;
            ForceReset = false;
            _isInitialized = true;
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
                    var userId = _sessionService.GetCurrentUserId();
                    var userRole = _sessionService.GetCurrentUserRole();
                    IsOwner = false;
                    IsEmployed = false;
                    CanApply = false;
                    if (userId == Project.ProjectOwnerId && Project.ProjectStatus != "Completed")
                        IsOwner = true; // Owner can update Active or Deactivated project
                    else if (Project.ProjectMembers.Any(pm => pm.MemberId == userId) && Project.ProjectStatus == "Active")
                        IsEmployed = true; // Members can resign from active projects
                    else if (!IsEmployed && userRole == UserRole.SkillProvider && Project.ProjectStatus == "Active")
                        CanApply = true; // SP can apply to active projects



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

        [RelayCommand]
        public async Task Resign()
        {
            if (!IsEmployed)
                return;
            await _navigationService.NavigateToAsync("ResignProjectView", new Dictionary<string, object> { { "ProjectId", project.ProjectId } });
        }

        [RelayCommand]
        public async Task Apply()
        {
            if (_sessionService.GetCurrentUserRole() != UserRole.SkillProvider)
                return;
            await _navigationService.NavigateToAsync("ApplyView", new Dictionary<string, object> { { "ProjectId", project.ProjectId } });
        }

        [RelayCommand]
        public async Task UpdateProject()
        {
            if (_sessionService.GetCurrentUserRole() != UserRole.ProjectOwner)
                return;
            await _navigationService.NavigateToAsync("UpdateProjectView", new Dictionary<string, object> { { "ProjectId", project.ProjectId } });
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
