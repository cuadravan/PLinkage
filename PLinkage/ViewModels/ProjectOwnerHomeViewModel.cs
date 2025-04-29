using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.ViewModels
{
    public partial class ProjectOwnerHomeViewModel : ObservableObject
    {
        // Services
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;


        // Properties
        [ObservableProperty] private string userName;
        [ObservableProperty] private ObservableCollection<SkillProvider> suggestedSkillProviders = new();
        [ObservableProperty] private int sentApplicationCount;
        [ObservableProperty] private int receivedOfferCount;
        [ObservableProperty] private int activeProjects;
        [ObservableProperty] private string summaryText;

        public IAsyncRelayCommand LoadDashboardDataCommand { get; }

        private string sortSelection = "All";
        public string SortSelection
        {
            get => sortSelection;
            set
            {
                if (SetProperty(ref sortSelection, value))
                    _ = LoadSuggestedSkillProviders();
            }
        }

        public ObservableCollection<string> SortOptions { get; } = new()
        {
            "All",
            "Same Place as Me",
            "Near Me"
        };

        // Constructor
        public ProjectOwnerHomeViewModel(INavigationService navigationService, IUnitOfWork unitOfWork, ISessionService sessionService)
        {
            _navigationService = navigationService;
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            LoadDashboardDataCommand = new AsyncRelayCommand(LoadDashboardData);
        }

        // Core Methods
        private async Task LoadDashboardData()
        {
            await _unitOfWork.ReloadAsync();
            var currentUser = _sessionService.GetCurrentUser();
            if (currentUser == null) return;

            UserName = currentUser.UserFirstName ?? string.Empty;
            await LoadSuggestedSkillProviders();
            await CountSentApplications(currentUser.UserId);
            await CountReceivedOffers(currentUser.UserId);
            await CountActiveProjects(currentUser.UserId);

            SummaryText = $"You have {ActiveProjects} active projects, {SentApplicationCount} pending sent applications, and {ReceivedOfferCount} received offers.";
        }

        private async Task LoadSuggestedSkillProviders()
        {
            // fetch all and exclude deactivated users
            var skillProviders = (await _unitOfWork.SkillProvider.GetAllAsync())
                .Where(sp => !string.Equals(sp.UserStatus, "Deactivated", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var currentUser = await _unitOfWork.ProjectOwner
                .GetByIdAsync(_sessionService.GetCurrentUser().UserId);
            if (currentUser == null || !currentUser.UserLocation.HasValue)
                return;

            var ownerCoord = CebuLocationCoordinates.Map[currentUser.UserLocation.Value];

            IEnumerable<SkillProvider> filtered = SortSelection switch
            {
                "Same Place as Me" => skillProviders
                    .Where(sp => sp.UserLocation == currentUser.UserLocation),

                "Near Me" => skillProviders
                    .Where(sp =>
                        sp.UserLocation.HasValue &&
                        CebuLocationCoordinates.Map.ContainsKey(sp.UserLocation.Value) &&
                        CalculateDistanceKm(ownerCoord, CebuLocationCoordinates.Map[sp.UserLocation.Value]) <= 50),

                _ => skillProviders
            };

            SuggestedSkillProviders = new ObservableCollection<SkillProvider>(filtered);
        }


        private async Task CountSentApplications(Guid userId)
        {
            var allApplications = await _unitOfWork.OfferApplications.GetAllAsync();
            SentApplicationCount = allApplications.Count(app => app.SenderId == userId);
        }

        private async Task CountReceivedOffers(Guid userId)
        {
            var allOffers = await _unitOfWork.OfferApplications.GetAllAsync();
            ReceivedOfferCount = allOffers.Count(offer => offer.ReceiverId == userId);
        }

        private async Task CountActiveProjects(Guid userId)
        {
            var allProjects = await _unitOfWork.Projects.GetAllAsync();
            ActiveProjects = allProjects.Count(p => p.ProjectOwnerId == userId && p.ProjectStatus == ProjectStatus.Active);
        }

        [RelayCommand]
        private async Task Refresh() => await LoadDashboardData();

        [RelayCommand]
        private void ViewSkillProvider(SkillProvider skillProvider)
        {
            Application.Current.MainPage.DisplayAlert("Project", $"You selected: {skillProvider.UserFirstName}", "OK");
        }

        private static double CalculateDistanceKm((double Latitude, double Longitude) coord1, (double Latitude, double Longitude) coord2)
        {
            const double EarthRadius = 6371; // km

            double lat1Rad = Math.PI * coord1.Latitude / 180;
            double lat2Rad = Math.PI * coord2.Latitude / 180;
            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = Math.PI * (coord2.Longitude - coord1.Longitude) / 180;

            double a = Math.Pow(Math.Sin(deltaLat / 2), 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Pow(Math.Sin(deltaLon / 2), 2);

            return EarthRadius * (2 * Math.Asin(Math.Sqrt(a)));
        }
    }
}
