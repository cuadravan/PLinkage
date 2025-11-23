using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Interfaces;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    public partial class SkillProviderHomeViewModel : ObservableObject
    {
        private readonly IDashboardServiceClient _dashboardServiceClient;
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private string sortSelection = "All";
        public ObservableCollection<ProjectCardDto> ProjectCards { get; set; } = new ObservableCollection<ProjectCardDto>();
        [ObservableProperty] private int activeProjectsValue = 0;
        [ObservableProperty] private int pendingSentApplicationsValue = 0;
        [ObservableProperty] private int pendingReceivedOffersValue = 0;

        [ObservableProperty]
        private bool isBusy = false;
    
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

        public SkillProviderHomeViewModel(INavigationService navigationService, IDashboardServiceClient dashboardServiceClient, ISessionService sessionService, IProjectServiceClient projectServiceClient)
        {
            _dashboardServiceClient = dashboardServiceClient;
            _sessionService = sessionService;
            _projectServiceClient = projectServiceClient;
            _navigationService = navigationService;
        }

        public async Task InitializeAsync()
        {
            if (ProjectCards.Any())
                return;

            await LoadDashboardDataAsync();
        }      

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadDashboardDataAsync();
        }

        [RelayCommand]
        private async Task ViewProject(ProjectCardDto projectCardDto)
        {
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", projectCardDto.ProjectId } });
        }

        public async Task LoadDashboardDataAsync()
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

        //public async Task GetNextSuggestedProjectsAsync()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;
        //    try
        //    {
        //        await GetSuggestedProjects();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error during initialization: {ex.Message}");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

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
                var currentUserId = _sessionService.GetCurrentUserId();
                foreach (var dto in result.Data)
                {
                    if(!dto.EmployedProviderIds.Contains(currentUserId))
                        ProjectCards.Add(dto);
                }
            }
        }       
    }
}
