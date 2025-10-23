using CommunityToolkit.Mvvm.ComponentModel;
using PLinkageApp.Interfaces;
using System.Collections.ObjectModel;
using PLinkageShared.DTOs;
using PLinkageShared.ApiResponse;

namespace PLinkageApp
{
    public partial class SkillProviderHomeViewModelTemp : ObservableObject
    {
        private readonly IDashboardServiceClient _dashboardServiceClient;
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly ISessionService _sessionService;
        public ObservableCollection<ProjectCardDto> ProjectCards { get; set; }
        [ObservableProperty] private int activeProjectsValue = 0;
        [ObservableProperty] private int pendingSentApplicationsValue = 0;
        [ObservableProperty] private int pendingReceivedOffersValue = 0;

        [ObservableProperty]
        private bool isBusy = false;

        public SkillProviderHomeViewModelTemp(IDashboardServiceClient dashboardServiceClient, ISessionService sessionService, IProjectServiceClient projectServiceClient)
        {
            _dashboardServiceClient = dashboardServiceClient;
            _sessionService = sessionService;
            _projectServiceClient = projectServiceClient;

            ProjectCards = new ObservableCollection<ProjectCardDto>();
        }

        public async Task InitializeAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.WhenAll(
                    GetDashboardStats(),
                    GetSuggestedProjects()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetNextSuggestedProjectsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await GetSuggestedProjects();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GetDashboardStats()
        {
            var userId = _sessionService.GetCurrentUserId();
            var result = await _dashboardServiceClient.GetSkillProviderDashboardAsync(userId);

            if (result.Success && result.Data != null)
            {
                ActiveProjectsValue = result.Data.ActiveProjects;
                PendingSentApplicationsValue = result.Data.PendingSentApplications;
                PendingReceivedOffersValue = result.Data.ReceivedOffers;
            }
        }
        private async Task GetSuggestedProjects()
        {
            var userLocation = _sessionService.GetCurrentUserLocation();
            ApiResponse<IEnumerable<ProjectCardDto>> result = null;
            ProjectCards.Clear();
            if (sortSelection == "Same Place as Me")
            {
                result = await _projectServiceClient.GetFilteredProjectsAsync("By Specific Location", userLocation, "Active");
            }
            else
            {
                result = await _projectServiceClient.GetFilteredProjectsAsync(sortSelection, userLocation, "Active");
            }
            if (result.Success && result.Data != null)
            {
                foreach (var dto in result.Data)
                {
                    ProjectCards.Add(dto);
                }
            }
        }

        private string sortSelection = "All";
        public string SortSelection
        {
            get => sortSelection;
            set
            {
                if (SetProperty(ref sortSelection, value))
                    _ = GetSuggestedProjects();
            }
        }

        public ObservableCollection<string> SortOptions { get; } = new()
        {
            "All",
            "Same Place as Me",
            "Nearby (<= 10km)",
            "Within Urban (<= 20km)",
            "Extended (<= 50km)"
        };
    }

    
}
