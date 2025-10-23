using CommunityToolkit.Mvvm.ComponentModel;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using System.Collections.ObjectModel;
using PLinkageApp.Interfaces;

namespace PLinkageApp
{
    public partial class ProjectOwnerHomeViewModelTemp : ObservableObject
    {
        private readonly IDashboardServiceClient _dashboardServiceClient;
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        public ObservableCollection<SkillProviderCardDto> SkillProviderCards { get; set; }
        [ObservableProperty] private int activeProjectsValue = 0;
        [ObservableProperty] private int pendingSentOffersValue = 0;
        [ObservableProperty] private int pendingReceivedApplicationsValue = 0;
        [ObservableProperty] private int reportedResignationsValue = 0;
        [ObservableProperty] private int reportedNegotiationsValue = 0;

        [ObservableProperty]
        private bool isBusy = false;

        public ProjectOwnerHomeViewModelTemp(IDashboardServiceClient dashboardServiceClient, ISessionService sessionService, ISkillProviderServiceClient skillProviderServiceClient)
        {
            _dashboardServiceClient = dashboardServiceClient;
            _sessionService = sessionService;
            _skillProviderServiceClient = skillProviderServiceClient;

            SkillProviderCards = new ObservableCollection<SkillProviderCardDto>();
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
                    GetSuggestedSkillProviders()
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

        private async Task GetDashboardStats()
        {
            var userId = _sessionService.GetCurrentUserId();
            var result = await _dashboardServiceClient.GetProjectOwnerDashboardAsync(userId);

            if (result.Success && result.Data != null)
            {
                ActiveProjectsValue = result.Data.ActiveProjects;
                PendingReceivedApplicationsValue = result.Data.ReceivedApplications;
                PendingSentOffersValue = result.Data.PendingSentOffers;
                ReportedResignationsValue = result.Data.ReportedResignations;
                ReportedNegotiationsValue = result.Data.ReportedNegotiations;
            }
        }

        private async Task GetSuggestedSkillProviders()
        {
            var userLocation = _sessionService.GetCurrentUserLocation();
            ApiResponse<IEnumerable<SkillProviderCardDto>> result = null;
            SkillProviderCards.Clear();
            var selection = sortSelection == "Same Place as Me" ? "By Specific Location" : sortSelection;
            result = await _skillProviderServiceClient.GetFilteredSkillProvidersAsync(selection, userLocation, "Active", null);

            if (result.Success && result.Data != null)
            {

                foreach (var dto in result.Data)
                {
                    SkillProviderCards.Add(dto);
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
                {
                    // Call the async method safely from the property setter
                    _ = GetSuggestedSkillProviders();
                }
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