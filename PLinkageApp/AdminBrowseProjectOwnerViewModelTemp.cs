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
    public partial class AdminBrowseProjectOwnerViewModelTemp : ObservableObject
    {
        private readonly IProjectOwnerServiceClient _projectOwnerServiceClient;
        private readonly ISessionService _sessionService;

        private List<ProjectOwnerCardDto> _allProjectOwners;

        public ObservableCollection<ProjectOwnerCardDto> ProjectOwnerCards { get; set; }

        [ObservableProperty]
        private bool isBusy = false;

        public AdminBrowseProjectOwnerViewModelTemp(IProjectOwnerServiceClient projectOwnerServiceClient, ISessionService sessionService)
        {
            _projectOwnerServiceClient = projectOwnerServiceClient;
            _sessionService = sessionService;

            _allProjectOwners = new List<ProjectOwnerCardDto>();
            ProjectOwnerCards = new ObservableCollection<ProjectOwnerCardDto>();
        }

        // --- NEW ---
        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetProjects();
        }

        // --- MODIFIED ---
        public async Task InitializeAsync()
        {
            // Only load data on the first appearance
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
                FilterProjectOwnerCards(); // --- RENAMED ---
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting project owners: {ex.Message}"); // --- FIXED LOG ---
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        private async Task ViewProjectOwner(ProjectOwnerCardDto projectOwnerCardDto)
        {
            await Shell.Current.DisplayAlert("Hey!", $"You clicked on project owner with id: {projectOwnerCardDto.UserId}", "Okay");
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
            "Deactivated Only"
        };

        // SEARCH FILTER

        // SEARCH QUERY

        [ObservableProperty]
        private string searchQuery = "";

        partial void OnSearchQueryChanged(string value)
        {
            FilterProjectOwnerCards(); // --- RENAMED ---
        }

        private const int FuzzySearchCutoff = 70;

        // --- RENAMED ---
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
                    .Where(card => Fuzz.PartialRatio(query, card.UserName.ToLowerInvariant())
                                     > FuzzySearchCutoff);

            }
            ProjectOwnerCards.Clear();
            foreach (var card in filteredList)
            {
                ProjectOwnerCards.Add(card);
            }
        }
    }
}