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
    public partial class BrowseSkillProviderViewModel : ObservableObject
    {
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private string categorySelection;
        private string statusSelection = "All";
        private string employmentSelection = "All";

        private bool _isInitialized = false;

        private const int FuzzySearchCutoff = 70;

        private List<SkillProviderCardDto> _allSkillProviders;
        public ObservableCollection<SkillProviderCardDto> SkillProviderCards { get; set; }

        [ObservableProperty]
        private bool isBusy = false;
        [ObservableProperty]
        private bool isAdmin = false;
        [ObservableProperty]
        private string searchQuery = "";
        [ObservableProperty]
        private string searchFilterSelection = "By Skills";
        [ObservableProperty]
        private CebuLocation? locationSelection = CebuLocation.CebuCity;

        
        public string CategorySelection
        {
            get => categorySelection;
            set
            {
                if (SetProperty(ref categorySelection, value))
                {
                    if (!_isInitialized)
                        return;
                    _ = GetSkillProviders();
                }
            }
        }
        public ObservableCollection<string> CategoryOptions { get; } = new(){};
        public ObservableCollection<CebuLocation> LocationOptions { get; } = new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());
        public ObservableCollection<string> EmploymentOptions { get; } = new()
        {
            "All",
            "Employed Only",
            "Unemployed Only"
        };
        public ObservableCollection<string> SearchFilterOptions { get; } = new()
        {
            "By Skills",
            "By Name"
        };
        public ObservableCollection<string> StatusOptions { get; } = new()
        {
            "All",
            "Active Only",
            "Deactivated Only"
        };

        public string StatusSelection
        {
            get => statusSelection;
            set
            {
                if (SetProperty(ref statusSelection, value) && value != "By Specific Location")
                    _ = GetSkillProviders();
            }
        }
        public string EmploymentSelection
        {
            get => employmentSelection;
            set
            {
                if (SetProperty(ref employmentSelection, value))
                    _ = GetSkillProviders();
            }
        }

        public BrowseSkillProviderViewModel(INavigationService navigationService, ISkillProviderServiceClient skillProviderServiceClient, ISessionService sessionService)
        {
            _skillProviderServiceClient = skillProviderServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;

            _allSkillProviders = new List<SkillProviderCardDto>();
            SkillProviderCards = new ObservableCollection<SkillProviderCardDto>();
        }

        public async Task InitializeAsync()
        {
            if (_allSkillProviders.Any())
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
                else if (userRole == UserRole.ProjectOwner)
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

                await GetSkillProviders();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetSkillProviders();
        }

        [RelayCommand]
        private async Task ViewSkillProvider(SkillProviderCardDto skillProviderCardDto)
        {
            await _navigationService.NavigateToAsync("ViewSkillProviderProfileView", new Dictionary<string, object> { { "SkillProviderId", skillProviderCardDto.UserId } });
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

                bool? employment = null;
                string status = string.Empty;

                if (IsAdmin)
                {
                    employment = EmploymentSelection switch
                    {
                        "All" => null,
                        "Employed Only" => true,
                        "Unemployed Only" => false,
                        _ => throw new NotImplementedException()
                    };
                    status = StatusSelection switch
                    {
                        "All" => "All",
                        "Active Only" => "Active",
                        "Deactivated Only" => "Deactivated",
                        _ => throw new NotImplementedException()
                    };
                }
                else
                {
                    employment = null;
                    status = "Active";
                }
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
       
        partial void OnLocationSelectionChanged(CebuLocation? oldValue, CebuLocation? newValue)
        {
            if (oldValue != newValue)
            {
                _ = GetSkillProviders();
            }
        }

        partial void OnSearchFilterSelectionChanged(string value)
        {
            FilterSkillProviderCards();
        }

        partial void OnSearchQueryChanged(string value)
        {
            FilterSkillProviderCards();
        }

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
