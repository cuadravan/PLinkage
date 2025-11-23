using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ForceReset), "ForceReset")]
    public partial class SkillProviderProfileViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly INavigationService _navigationService;

        private bool _isInitialized = false;

        [ObservableProperty]
        public bool isRatingVisible;

        [ObservableProperty]
        public bool isUserActivated;

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        public SkillProviderDto skillProviderDto;  

        public Guid SkillProviderId { get; set; }
        public bool ForceReset { get; set; }

        public SkillProviderProfileViewModel(ISessionService sessionService, IAccountServiceClient accountServiceClient, ISkillProviderServiceClient skillProviderServiceClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _accountServiceClient = accountServiceClient;
            _skillProviderServiceClient = skillProviderServiceClient;
        }

        public async Task InitializeAsync() // Runs when navigating to the page
        {
            if (_isInitialized && !ForceReset) //If already initialized, or not a force reset, then don't initialize
                return;
            try
            {
                ForceReset = false;
                _isInitialized = true;
                SkillProviderId = _sessionService.GetCurrentUserId();
                await LoadUserDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RefreshAsync() // Command executed when refreshed using RefreshView
        {
            await LoadUserDataAsync();            
        }        

        

        [RelayCommand]
        private async Task UpdateProfile()
        {
            if (IsBusy || !_isInitialized)
                return;
            await _navigationService.NavigateToAsync("UpdateProfileView");
            // Logic for updating profile, navigate to UpdateProfileView
        }

        [RelayCommand]
        private async Task AddEducation()
        {
            if (!_isInitialized || IsBusy)
                return;
            await _navigationService.NavigateToAsync("AddEducationView");
            // Logic for adding education, navigate to AddEducationView
        }

        [RelayCommand]
        private async Task UpdateEducation(EducationDto educationDto)
        {
            if (!_isInitialized || IsBusy)
                return;
            int index = skillProviderDto.Educations.IndexOf(educationDto);
            await _navigationService.NavigateToAsync("UpdateEducationView", new Dictionary<string, object> { { "EducationIndex", index } });
            // Logic for viewing education, navigate to UpdateEducationView (which contains the DeleteEducation)    
        }

        [RelayCommand]
        private async Task AddSkill()
        {
            if (!_isInitialized || IsBusy)
                return;
            await _navigationService.NavigateToAsync("AddSkillView");
            // Logic for adding skill, navigate to AddSkillView   
        }

        [RelayCommand]
        private async Task ViewSkill(SkillDto skillDto)
        {
            if (!_isInitialized || IsBusy)
                return;
            int index = skillProviderDto.Skills.IndexOf(skillDto);
            await _navigationService.NavigateToAsync("ViewSkillView", new Dictionary<string, object> { { "SkillIndex", index }, { "SkillProviderId", skillProviderDto.UserId} });
            // Logic for viewing skill, navigate to ViewSkillView (which contains the UpdateSkill and DeleteSkill)
        }

        [RelayCommand]
        private async Task ViewProject(SkillProviderProfileProjectsDto skillProviderProfileProjectsDto)
        {
            if (!_isInitialized || IsBusy)
                return;
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", skillProviderProfileProjectsDto.ProjectId } });
        }

        private async Task LoadUserDataAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {

                ApiResponse<SkillProviderDto> result = null;

                result = await _skillProviderServiceClient.GetSpecificAsync(SkillProviderId);

                if (result.Success && result.Data != null)
                {

                    SkillProviderDto = result.Data;
                    _isInitialized = true;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting skill provider profile: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}