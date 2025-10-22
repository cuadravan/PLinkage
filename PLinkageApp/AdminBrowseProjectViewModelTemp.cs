using CommunityToolkit.Mvvm.ComponentModel;
using PLinkageApp.Interfaces;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzySharp;
using CommunityToolkit.Mvvm.Input;

namespace PLinkageApp
{
    public partial class AdminBrowseProjectViewModelTemp : ObservableObject
    {
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly ISessionService _sessionService;

        private List<ProjectCardDto> _allProjects;
        public ObservableCollection<ProjectCardDto> ProjectCards { get; set; }

        [ObservableProperty]
        private bool isBusy = false;

        public AdminBrowseProjectViewModelTemp(IProjectServiceClient projectServiceClient, ISessionService sessionService)
        {
            _projectServiceClient = projectServiceClient;
            _sessionService = sessionService;

            _allProjects = new List<ProjectCardDto>();
            ProjectCards = new ObservableCollection<ProjectCardDto>();
        }

        // --- NEW ---
        [RelayCommand]
        private async Task RefreshAsync()
        {
            // This command is for an explicit user refresh (e.g., pull-to-refresh).
            await GetProjects();
        }

        // --- MODIFIED ---
        public async Task InitializeAsync()
        {
            // If we already have data, don't fetch it again just on appearing.
            if (_allProjects.Any())
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
                var selection = CategorySelection;
                var status = StatusSelection switch
                {
                    "All" => "All",
                    "Active Only" => "Active",
                    "Completed Only" => "Completed",
                    "Deactivated Only" => "Deactivated",
                    _ => throw new NotImplementedException()
                };
                var location = LocationSelection;
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
                FilterSkillProviderCards(); // You might want to rename this to FilterProjectCards()
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting projects: {ex.Message}"); // Updated log message
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
            await Shell.Current.DisplayAlert("Hey!", $"You clicked on project with id: {projectCardDto.ProjectId}", "Okay");
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
                    _ = GetProjects();
                }
            }
        }
        public ObservableCollection<string> CategoryOptions { get; } = new()
        {
            "All",
            "By Specific Location"
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
            FilterSkillProviderCards(); // Rename this
        }

        // SEARCH QUERY

        [ObservableProperty]
        private string searchQuery = "";

        partial void OnSearchQueryChanged(string value)
        {
            FilterSkillProviderCards(); // Rename this
        }

        private const int FuzzySearchCutoff = 70;

        // NOTE: You should rename this method to FilterProjectCards
        private void FilterSkillProviderCards()
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