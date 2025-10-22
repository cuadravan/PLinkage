using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PLinkageApp.Services;
using PLinkageApp.Interfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp
{
    public partial class AdminHomeViewModelTemp : ObservableObject
    {
        private readonly IDashboardServiceClient _dashboardServiceClient;
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        private readonly IProjectServiceClient _projectServiceClient;
        public ObservableCollection<SkillProviderCardDto> SkillProviderCards { get; set; }
        public ObservableCollection<ProjectCardDto> ProjectCards { get; set; }
        [ObservableProperty] private int activeProjectsValue = 0;
        [ObservableProperty] private int completedProjectsValue = 0;
        [ObservableProperty] private string employmentRatioValue;

        [ObservableProperty]
        private bool isBusy = false;

        public AdminHomeViewModelTemp(IDashboardServiceClient dashboardServiceClient, ISessionService sessionService, ISkillProviderServiceClient skillProviderServiceClient, IProjectServiceClient projectServiceClient)
        {
            _dashboardServiceClient = dashboardServiceClient;
            _sessionService = sessionService;
            _skillProviderServiceClient = skillProviderServiceClient;
            _projectServiceClient = projectServiceClient;

            SkillProviderCards = new ObservableCollection<SkillProviderCardDto>();
            ProjectCards = new ObservableCollection<ProjectCardDto>();
        }

        // --- NEW ---
        // This command is for the RefreshView
        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadDashboardDataAsync();
        }

        // --- MODIFIED ---
        public async Task InitializeAsync()
        {
            // Only load data if the lists are empty (i.e., first-time load)
            if (ProjectCards.Any() || SkillProviderCards.Any())
                return;

            await LoadDashboardDataAsync();
        }

        // --- NEW ---
        // Created a central method for all data loading
        private async Task LoadDashboardDataAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                // Clear lists before fetching new data
                SkillProviderCards.Clear();
                ProjectCards.Clear();

                // Run both network calls concurrently and wait for them both to finish
                await Task.WhenAll(
                    GetDashboardStats(),
                    GetSuggestedSkillProviders(),
                    GetSuggestedProjects()
                );
            }
            catch (Exception ex)
            {
                // Central place to handle any initialization errors, e.g., show a popup
                Console.WriteLine($"Error during initialization: {ex.Message}");
                // Optionally display an alert to the user
                await Shell.Current.DisplayAlert("Load Error", "Failed to load dashboard data.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GetDashboardStats()
        {
            var userId = _sessionService.GetCurrentUserId();
            var result = await _dashboardServiceClient.GetAdminDashboardAsync(userId);

            if (result.Success && result.Data != null)
            {
                ActiveProjectsValue = result.Data.OverallActiveProjects;
                CompletedProjectsValue = result.Data.OverallCompleteProjects;
                EmploymentRatioValue = result.Data.EmploymentRatio.ToString("F2");
            }
        }

        private async Task GetSuggestedSkillProviders()
        {
            var userLocation = _sessionService.GetCurrentUserLocation();
            ApiResponse<IEnumerable<SkillProviderCardDto>> result = null;
            // Note: Clear was moved to LoadDashboardDataAsync
            result = await _skillProviderServiceClient.GetFilteredSkillProvidersAsync("All", userLocation, "Active", null);

            if (result.Success && result.Data != null)
            {
                foreach (var dto in result.Data)
                {
                    SkillProviderCards.Add(dto);
                }
            }
        }

        private async Task GetSuggestedProjects()
        {
            var userLocation = _sessionService.GetCurrentUserLocation();
            ApiResponse<IEnumerable<ProjectCardDto>> result = null;
            // Note: Clear was moved to LoadDashboardDataAsync
            result = await _projectServiceClient.GetFilteredProjectsAsync("All", userLocation, "Active");

            if (result.Success && result.Data != null)
            {
                foreach (var dto in result.Data)
                {
                    ProjectCards.Add(dto);
                }
            }
        }

        [RelayCommand]
        private async Task ViewSkillProvider(SkillProviderCardDto skillProviderCardDto)
        {
            await Shell.Current.DisplayAlert("Hey!", $"You clicked on skill provider with id: {skillProviderCardDto.UserId}", "Okay");
        }

        [RelayCommand]
        private async Task ViewProject(ProjectCardDto projectCardDto)
        {
            await Shell.Current.DisplayAlert("Hey!", $"You clicked on project with id: {projectCardDto.ProjectId}", "Okay");
        }
    }
}