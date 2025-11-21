using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Models;
using System.Globalization;
using PLinkageApp.Interfaces;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ForceReset), "ForceReset")]
    public partial class SkillProviderProfileViewModel : ObservableObject
    {
        public Guid SkillProviderId { get; set; }
        bool _forceReset;

        public bool ForceReset
        {
            get => _forceReset;
            set
            {
                _forceReset = value;
                if (_forceReset)
                {
                    _ = InitializeAsync(); // Trigger logic when property is set
                }
            }
        }

        //[ObservableProperty]
        //public bool isUserCurrentlyActive;

        [ObservableProperty]
        public bool isRatingVisible;

        //[ObservableProperty]
        //public bool isMessageButtonVisible;

        //[ObservableProperty]
        //public bool isDeactivateButtonVisible;

        //[ObservableProperty]
        //public bool isSendOfferButtonVisible;

        [ObservableProperty]
        public bool isUserActivated;

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        public SkillProviderDto skillProviderDto;

        private bool _isInitialized = false;

        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly INavigationService _navigationService;

        public SkillProviderProfileViewModel(ISessionService sessionService, IAccountServiceClient accountServiceClient, ISkillProviderServiceClient skillProviderServiceClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _accountServiceClient = accountServiceClient;
            _skillProviderServiceClient = skillProviderServiceClient;
        }

        [RelayCommand]
        private async Task RefreshAsync() // Command executed when refreshed using RefreshView
        {
            await LoadUserDataAsync();            
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

        [RelayCommand]
        public async Task UpdateProfile()
        {
            await _navigationService.NavigateToAsync("UpdateProfileView");
            // Logic for updating profile, navigate to UpdateProfileView
        }

        [RelayCommand]
        public async Task AddEducation()
        {
            await _navigationService.NavigateToAsync("AddEducationView");
            // Logic for adding education, navigate to AddEducationView
        }

        [RelayCommand]
        public async Task UpdateEducation(EducationDto educationDto)
        {
            int index = skillProviderDto.Educations.IndexOf(educationDto);
            await _navigationService.NavigateToAsync("UpdateEducationView", new Dictionary<string, object> { { "EducationIndex", index } });
            // Logic for viewing education, navigate to UpdateEducationView (which contains the DeleteEducation)    
        }

        [RelayCommand]
        public async Task AddSkill()
        {
            await _navigationService.NavigateToAsync("AddSkillView");
            // Logic for adding skill, navigate to AddSkillView   
        }

        [RelayCommand]
        public async Task ViewSkill(SkillDto skillDto)
        {
            int index = skillProviderDto.Skills.IndexOf(skillDto);
            await _navigationService.NavigateToAsync("ViewSkillView", new Dictionary<string, object> { { "SkillIndex", index }, { "SkillProviderId", skillProviderDto.UserId} });
            // Logic for viewing skill, navigate to ViewSkillView (which contains the UpdateSkill and DeleteSkill)
        }

        [RelayCommand]
        public async Task ViewProject(SkillProviderProfileProjectsDto skillProviderProfileProjectsDto)
        {
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", skillProviderProfileProjectsDto.ProjectId } });
        }
    }
}