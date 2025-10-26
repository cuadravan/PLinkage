using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Models;
using PLinkageApp.Interfaces;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    public partial class AdminHomeViewModel : ObservableObject
    {
        private readonly IDashboardServiceClient _dashboardServiceClient;
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly INavigationService _navigationService;
        public ObservableCollection<SkillProviderCardDto> SkillProviderCards { get; set; }
        public ObservableCollection<ProjectCardDto> ProjectCards { get; set; }
        [ObservableProperty] private int activeProjectsValue = 0;
        [ObservableProperty] private int completedProjectsValue = 0;
        [ObservableProperty] private string employmentRatioValue;

        [ObservableProperty]
        private bool isBusy = false;

        public AdminHomeViewModel(INavigationService navigationService, IDashboardServiceClient dashboardServiceClient, ISessionService sessionService, ISkillProviderServiceClient skillProviderServiceClient, IProjectServiceClient projectServiceClient)
        {
            _dashboardServiceClient = dashboardServiceClient;
            _sessionService = sessionService;
            _skillProviderServiceClient = skillProviderServiceClient;
            _projectServiceClient = projectServiceClient;
            _navigationService = navigationService;

            SkillProviderCards = new ObservableCollection<SkillProviderCardDto>();
            ProjectCards = new ObservableCollection<ProjectCardDto>();
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadDashboardDataAsync();
        }

        public async Task InitializeAsync()
        {
            if (ProjectCards.Any() || SkillProviderCards.Any())
                return;

            await LoadDashboardDataAsync();
        }

        private async Task LoadDashboardDataAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                SkillProviderCards.Clear();
                ProjectCards.Clear();

                await Task.WhenAll(
                    GetDashboardStats(),
                    GetSuggestedSkillProviders(),
                    GetSuggestedProjects()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
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
            await _navigationService.NavigateToAsync("ViewSkillProviderProfileView", new Dictionary<string, object> { { "SkillProviderId", skillProviderCardDto.UserId } });
        }

        [RelayCommand]
        private async Task ViewProject(ProjectCardDto projectCardDto)
        {
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", projectCardDto.ProjectId } });
        }
    }
}