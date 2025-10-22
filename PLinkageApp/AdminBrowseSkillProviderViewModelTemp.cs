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
    public partial class AdminBrowseSkillProviderViewModelTemp : ObservableObject
    {
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;

        private List<SkillProviderCardDto> _allSkillProviders;
        public ObservableCollection<SkillProviderCardDto> SkillProviderCards { get; set; }

        [ObservableProperty]
        private bool isBusy = false;

        public AdminBrowseSkillProviderViewModelTemp(ISkillProviderServiceClient skillProviderServiceClient, ISessionService sessionService)
        {
            _skillProviderServiceClient = skillProviderServiceClient;
            _sessionService = sessionService;

            _allSkillProviders = new List<SkillProviderCardDto>();
            SkillProviderCards = new ObservableCollection<SkillProviderCardDto>();
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetSkillProviders();
        }

        public async Task InitializeAsync()
        {
            if (_allSkillProviders.Any())
                return;
            try
            {
                await GetSkillProviders();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        private async Task GetSkillProviders()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                var userLocation = _sessionService.GetCurrentUserLocation();
                ApiResponse<IEnumerable<SkillProviderCardDto>> result = null;

                _allSkillProviders.Clear();
                SkillProviderCards.Clear();
                var selection = CategorySelection;
                bool? employment = EmploymentSelection switch
                {
                    "All" => null,
                    "Employed Only" => true,
                    "Unemployed Only" => false,
                    _ => throw new NotImplementedException()
                };
                var status = StatusSelection switch
                {
                    "All" => "All",
                    "Active Only" => "Active",
                    "Deactivated Only" => "Deactivated",
                    _ => throw new NotImplementedException()
                };
                var location = LocationSelection;
                result = await _skillProviderServiceClient.GetFilteredSkillProvidersAsync(selection, location, status, employment);

                if (result.Success && result.Data != null)
                {

                    foreach (var dto in result.Data)
                    {
                        _allSkillProviders.Add(dto);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
                FilterSkillProviderCards();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting skill providers: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        private async Task ViewSkillProvider(SkillProviderCardDto skillProviderCardDto)
        {
            await Shell.Current.DisplayAlert("Hey!", $"You clicked on skill provider with id: {skillProviderCardDto.UserId}", "Okay");
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
                    _ = GetSkillProviders();
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
                _ = GetSkillProviders();
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
                    _ = GetSkillProviders();
            }
        }

        public ObservableCollection<string> StatusOptions { get; } = new()
        {
            "All",
            "Active Only",
            "Deactivated Only"
        };

        // EMPLOYMENT

        private string employmentSelection = "All";
        public string EmploymentSelection
        {
            get => employmentSelection;
            set
            {
                if (SetProperty(ref employmentSelection, value))
                    _ = GetSkillProviders();
            }
        }

        public ObservableCollection<string> EmploymentOptions { get; } = new()
        {
            "All",
            "Employed Only",
            "Unemployed Only"
        };

        // EMPLOYMENT

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
            FilterSkillProviderCards();
        }

        // SEARCH QUERY

        [ObservableProperty]
        private string searchQuery = "";

        partial void OnSearchQueryChanged(string value)
        {
            FilterSkillProviderCards();
        }

        private const int FuzzySearchCutoff = 70;

        private void FilterSkillProviderCards()
        {
            var query = SearchQuery.Trim().ToLowerInvariant();

            IEnumerable<SkillProviderCardDto> filteredList;

            if (string.IsNullOrEmpty(query))
            {
                filteredList = _allSkillProviders;
            }
            else
            {
                switch (SearchFilterSelection)
                {
                    case "By Name":
                        filteredList = _allSkillProviders
                            .Where(card => Fuzz.PartialRatio(query, card.UserName.ToLowerInvariant())
                                           > FuzzySearchCutoff);
                        break;

                    case "By Skills":
                        filteredList = _allSkillProviders
                            .Where(card => card.Skills != null && card.Skills
                                .Any(skill => Fuzz.PartialRatio(query, skill.ToLowerInvariant())
                                              > FuzzySearchCutoff));
                        break;

                    default:
                        filteredList = _allSkillProviders;
                        break;
                }
            }

            SkillProviderCards.Clear();
            foreach (var card in filteredList)
            {
                SkillProviderCards.Add(card);
            }
        }
    }
}
