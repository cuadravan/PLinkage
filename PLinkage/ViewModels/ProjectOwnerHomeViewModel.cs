using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.ViewModels
{
    public partial class ProjectOwnerHomeViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;

        public ProjectOwnerHomeViewModel(INavigationService navigationService, IUnitOfWork unitOfWork, ISessionService sessionService)
        {
            _navigationService = navigationService;
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;

            OnAppearingCommand = new AsyncRelayCommand(OnAppearing);
        }

        public ObservableCollection<string> SortOptions { get; } = new()
        {
            "All",
            "Same Place as Me",
            "Near Me"
        };

        [ObservableProperty] private string userName;
        [ObservableProperty] private ObservableCollection<SkillProvider> suggestedSkillProviders = new();
        [ObservableProperty] private int sentApplicationCount;
        [ObservableProperty] private int receivedOfferCount;
        [ObservableProperty] private int activeProjects;
        [ObservableProperty] private string summaryText;

        public IAsyncRelayCommand OnAppearingCommand { get; }

        private string sortSelection = "All";

        public string SortSelection
        {
            get => sortSelection;
            set
            {
                if (SetProperty(ref sortSelection, value))
                {
                    // Trigger the filtering when the selection changes
                    LoadSuggestedSkillProviders();
                }
            }
        }

        public async Task OnAppearing()
        {
            await _unitOfWork.ReloadAsync();
            UserName = _sessionService.GetCurrentUser()?.UserFirstName ?? string.Empty;

            await LoadSuggestedSkillProviders();
            await CountSentApplications();
            await CountReceivedOffers();
            await CountActiveProjects();

            SummaryText = $"You have {ActiveProjects} active projects, {SentApplicationCount} pending sent applications, and {ReceivedOfferCount} received offers.";
        }

        private async Task LoadSuggestedSkillProviders()
        {
            var skillProviders = await _unitOfWork.SkillProvider.GetAllAsync();
            var tempCurrentUser = _sessionService.GetCurrentUser().UserId;
            var currentUser = await _unitOfWork.ProjectOwner.GetByIdAsync(tempCurrentUser);

            if (currentUser == null || !currentUser.UserLocation.HasValue)
                return;

            var ownerCoord = CebuLocationCoordinates.Map[currentUser.UserLocation.Value];

            IEnumerable<SkillProvider> filtered = skillProviders;

            switch (SortSelection)
            {
                case "Same Place as Me":
                    filtered = skillProviders.Where(sp => sp.UserLocation == currentUser.UserLocation);
                    break;

                case "Near Me":
                    filtered = skillProviders.Where(sp =>
                        sp.UserLocation.HasValue &&
                        CebuLocationCoordinates.Map.ContainsKey(sp.UserLocation.Value) &&
                        CalculateDistanceKm(ownerCoord, CebuLocationCoordinates.Map[sp.UserLocation.Value]) <= 50);

                    break;

                case "All":
                default:
                    // No filtering
                    break;
            }

            SuggestedSkillProviders = new ObservableCollection<SkillProvider>(filtered);
        }


        private async Task CountSentApplications()
        {
            var currentUser = _sessionService.GetCurrentUser();
            if (currentUser == null) return;

            var allApplications = await _unitOfWork.OfferApplications.GetAllAsync();
            SentApplicationCount = allApplications.Count(app => app.SenderId == currentUser.UserId);
        }

        private async Task CountReceivedOffers()
        {
            var currentUser = _sessionService.GetCurrentUser();
            if (currentUser == null) return;

            var allOffers = await _unitOfWork.OfferApplications.GetAllAsync();
            ReceivedOfferCount = allOffers.Count(offer => offer.ReceiverId == currentUser.UserId);
        }

        private async Task CountActiveProjects()
        {
            var currentUser = _sessionService.GetCurrentUser();
            if (currentUser == null) return;

            var allProjects = await _unitOfWork.Projects.GetAllAsync();
            ActiveProjects = allProjects.Count(p => p.ProjectOwnerId == currentUser.UserId && p.ProjectStatus == ProjectStatus.Active);
        }

        [RelayCommand]
        private void ViewSkillProvider(SkillProvider skillProvider)
        {
            // This runs when the button is clicked
            Application.Current.MainPage.DisplayAlert("Project", $"You selected: {skillProvider.UserFirstName}", "OK");
        }



        private double CalculateDistanceKm((double Latitude, double Longitude) coord1, (double Latitude, double Longitude) coord2)
        {
            const double EarthRadius = 6371; // Kilometers

            double lat1Rad = Math.PI * coord1.Latitude / 180;
            double lat2Rad = Math.PI * coord2.Latitude / 180;
            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = Math.PI * (coord2.Longitude - coord1.Longitude) / 180;

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));

            return EarthRadius * c;
        }

    }
}
