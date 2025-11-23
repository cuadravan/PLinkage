using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using FuzzySharp;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    public partial class AdminBrowseProjectOwnerViewModel : ObservableObject
    {      
        private readonly IProjectOwnerServiceClient _projectOwnerServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private List<ProjectOwnerCardDto> _allProjectOwners = new List<ProjectOwnerCardDto>(); // Storing original results

        private string categorySelection = "All";
        private string statusSelection = "All";

        private const int FuzzySearchCutoff = 70;

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private CebuLocation? locationSelection = CebuLocation.CebuCity;

        [ObservableProperty]
        private string searchQuery = "";

        public ObservableCollection<ProjectOwnerCardDto> ProjectOwnerCards { get; set; }

        public string CategorySelection
        {
            get => categorySelection;
            set
            {
                if (SetProperty(ref categorySelection, value))
                {
                    _ = GetProjects();
                }
            }
        }

        public ObservableCollection<string> CategoryOptions { get; } = new()
        {
            "All",
            "By Specific Location"
        };

        public ObservableCollection<CebuLocation> LocationOptions { get; } = new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());
        public string StatusSelection
        {
            get => statusSelection;
            set
            {
                if (SetProperty(ref statusSelection, value) && value != "By Specific Location")
                    _ = GetProjects();
            }
        }

        public ObservableCollection<string> StatusOptions { get; } = new()
        {
            "All",
            "Active Only",
            "Deactivated Only"
        };

        public AdminBrowseProjectOwnerViewModel(INavigationService navigationService, IProjectOwnerServiceClient projectOwnerServiceClient, ISessionService sessionService)
        {
            _projectOwnerServiceClient = projectOwnerServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;

            _allProjectOwners = new List<ProjectOwnerCardDto>();
            ProjectOwnerCards = new ObservableCollection<ProjectOwnerCardDto>();
        }

        public async Task InitializeAsync()
        {
            if (_allProjectOwners.Any())
                return;
            try
            {
                await GetProjects();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetProjects();
        }

        [RelayCommand]
        private async Task ViewProjectOwner(ProjectOwnerCardDto projectOwnerCardDto)
        {
            await _navigationService.NavigateToAsync("ViewProjectOwnerProfileView", new Dictionary<string, object> { { "ProjectOwnerId", projectOwnerCardDto.UserId } });
        }

        partial void OnLocationSelectionChanged(CebuLocation? oldValue, CebuLocation? newValue)
        {
            if (oldValue != newValue)
            {
                _ = GetProjects();
            }
        }

        partial void OnSearchQueryChanged(string value)
        {
            FilterProjectOwnerCards();
        }

        private async Task GetProjects()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                var userLocation = _sessionService.GetCurrentUserLocation();
                ApiResponse<IEnumerable<ProjectOwnerCardDto>> result = null;

                _allProjectOwners.Clear();
                ProjectOwnerCards.Clear();
                var selection = CategorySelection;
                var status = StatusSelection switch
                {
                    "All" => "All",
                    "Active Only" => "Active",
                    "Deactivated Only" => "Deactivated",
                    _ => throw new NotImplementedException()
                };
                var location = LocationSelection;
                result = await _projectOwnerServiceClient.GetFilteredProjectOwnersAsync(selection, location, status);

                if (result.Success && result.Data != null)
                {

                    foreach (var dto in result.Data)
                    {
                        _allProjectOwners.Add(dto);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
                FilterProjectOwnerCards();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting project owners: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }

        private void FilterProjectOwnerCards()
        {
            var query = SearchQuery.Trim().ToLowerInvariant();

            IEnumerable<ProjectOwnerCardDto> filteredList;

            if (string.IsNullOrEmpty(query))
            {
                filteredList = _allProjectOwners;
            }
            else
            {
                filteredList = _allProjectOwners
                    .Where(card => Fuzz.PartialRatio(query, card.UserName.ToLowerInvariant()) > FuzzySearchCutoff);
            }
            ProjectOwnerCards.Clear();
            foreach (var card in filteredList)
            {
                ProjectOwnerCards.Add(card);
            }
        }
    }
}