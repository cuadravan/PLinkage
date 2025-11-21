using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Models;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using FuzzySharp;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    public partial class BrowseProjectViewModel : ObservableObject
    {
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private List<ProjectCardDto> _allProjects;
        public ObservableCollection<ProjectCardDto> ProjectCards { get; set; }

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private bool isAdmin = false;

        private bool _isInitialized = false;

        public BrowseProjectViewModel(IProjectServiceClient projectServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _projectServiceClient = projectServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
            _allProjects = new List<ProjectCardDto>();
            ProjectCards = new ObservableCollection<ProjectCardDto>();

        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetProjects();
        }

        public async Task InitializeAsync()
        {
            if (_allProjects.Any())
                return;

            try
            {
                var userRole = _sessionService.GetCurrentUserRole();

                CategoryOptions.Clear();

                CategoryOptions.Add("All");
                CategoryOptions.Add("By Specific Location");

                if (userRole == UserRole.Admin)
                {
                    IsAdmin = true;
                }
                else if (userRole == UserRole.SkillProvider)
                {
                    IsAdmin = false;
                    CategoryOptions.Add("Same Location as Me");
                    CategoryOptions.Add("Nearby (<= 10km)");
                    CategoryOptions.Add("Within Urban (<= 20km)");
                    CategoryOptions.Add("Extended (<= 50km)");
                }
                else
                {
                    // Not allowed
                    await _navigationService.GoBackAsync();
                }
                CategorySelection = CategoryOptions.FirstOrDefault();
                _isInitialized = true;
                await GetProjects();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        private async Task GetProjects()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                var userLocation = _sessionService.GetCurrentUserLocation();
                ApiResponse<IEnumerable<ProjectCardDto>> result = null;



                _allProjects.Clear();
                ProjectCards.Clear();
                var selection = string.Empty;
                CebuLocation? location = null;
                if (CategorySelection == "Same Location as Me") // API's same location as me is same as by specific location while location is user's location
                {
                    selection = "By Specific Location";
                    location = _sessionService.GetCurrentUserLocation();
                }
                else if (CategorySelection == "By Specific Location")
                {
                    selection = "By Specific Location";
                    location = LocationSelection;
                }
                else
                {
                    selection = CategorySelection;
                    location = _sessionService.GetCurrentUserLocation();
                }

                string status = string.Empty;
                if (IsAdmin)
                {
                    status = StatusSelection switch
                    {
                        "All" => "All",
                        "Active Only" => "Active",
                        "Completed Only" => "Completed",
                        "Deactivated Only" => "Deactivated",
                        _ => throw new NotImplementedException()
                    };
                }
                else
                {
                    status = "Active";
                }

                result = await _projectServiceClient.GetFilteredProjectsAsync(selection, location, status);

                if (result.Success && result.Data != null)
                {

                    foreach (var dto in result.Data)
                    {
                        _allProjects.Add(dto);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
                FilterProjectCards();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting projects: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        private async Task ViewProject(ProjectCardDto projectCardDto)
        {
            //await Shell.Current.DisplayAlert("Hey!", $"You clicked on project with id: {projectCardDto.ProjectId}", "Okay");
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", projectCardDto.ProjectId } });

        }

        // CATEGORY

        private string categorySelection = "All";
        public string CategorySelection
        {
            get => categorySelection;
            set
            {
                if (SetProperty(ref categorySelection, value))
                {
                    if (!_isInitialized)
                        return;

                    _ = GetProjects();
                }
            }
        }
        public ObservableCollection<string> CategoryOptions { get; } = new()
        {
        };

        // LOCATION

        [ObservableProperty]
        private CebuLocation? locationSelection = CebuLocation.CebuCity;

        partial void OnLocationSelectionChanged(CebuLocation? oldValue, CebuLocation? newValue)
        {
            if (oldValue != newValue)
            {
                _ = GetProjects();
            }
        }

        public ObservableCollection<CebuLocation> LocationOptions { get; } = new(
            Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());


        // STATUS

        private string statusSelection = "All";
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
            "Completed Only",
            "Deactivated Only"
        };

        public ObservableCollection<string> SearchFilterOptions { get; } = new()
        {
            "By Skills",
            "By Name"
        };

        // SEARCH FILTER

        [ObservableProperty]
        private string searchFilterSelection = "By Skills";

        partial void OnSearchFilterSelectionChanged(string value)
        {
            FilterProjectCards(); // Rename this
        }

        // SEARCH QUERY

        [ObservableProperty]
        private string searchQuery = "";

        partial void OnSearchQueryChanged(string value)
        {
            FilterProjectCards(); // Rename this
        }

        private const int FuzzySearchCutoff = 70;

        // NOTE: You should rename this method to FilterProjectCards
        private void FilterProjectCards()
        {
            var query = SearchQuery.Trim().ToLowerInvariant();

            IEnumerable<ProjectCardDto> filteredList;

            if (string.IsNullOrEmpty(query))
            {
                filteredList = _allProjects;
            }
            else
            {
                switch (SearchFilterSelection)
                {
                    case "By Name":
                        filteredList = _allProjects
                            .Where(card => Fuzz.PartialRatio(query, card.Title.ToLowerInvariant())
                                         > FuzzySearchCutoff);
                        break;

                    case "By Skills":
                        filteredList = _allProjects
                            .Where(card => card.Skills != null && card.Skills
                                .Any(skill => Fuzz.PartialRatio(query, skill.ToLowerInvariant())
                                              > FuzzySearchCutoff));
                        break;

                    default:
                        filteredList = _allProjects;
                        break;
                }
            }

            ProjectCards.Clear();
            foreach (var card in filteredList)
            {
                ProjectCards.Add(card);
            }
        }
    }
}
