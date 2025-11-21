using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Models;
using System.Globalization;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(SkillProviderId), "SkillProviderId")]
    public partial class ViewSkillProviderProfileViewModel : ObservableObject
    {
        public Guid SkillProviderId { get; set; }

        [ObservableProperty]
        public bool isUserCurrentlyActive;

        [ObservableProperty]
        public bool isRatingVisible;

        [ObservableProperty]
        public bool isMessageButtonVisible;

        [ObservableProperty]
        public bool isDeactivateButtonVisible;

        [ObservableProperty]
        public bool isSendOfferButtonVisible;

        [ObservableProperty]
        public bool isUserActivated;

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        public SkillProviderDto skillProviderDto;

        private bool _isInitialized;

        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly INavigationService _navigationService;

        public ViewSkillProviderProfileViewModel(ISessionService sessionService, IAccountServiceClient accountServiceClient, ISkillProviderServiceClient skillProviderServiceClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _accountServiceClient = accountServiceClient;
            _skillProviderServiceClient = skillProviderServiceClient;
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadUserDataAsync();
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;
            try
            {
                var currentVisitorUserRole = _sessionService.GetCurrentUserRole();
                if (currentVisitorUserRole == UserRole.Admin)
                {
                    IsMessageButtonVisible = true;
                    IsDeactivateButtonVisible = true;
                    IsSendOfferButtonVisible = false;
                }
                else if(currentVisitorUserRole == UserRole.ProjectOwner)
                {
                    IsMessageButtonVisible = true;
                    IsDeactivateButtonVisible = false;
                    IsSendOfferButtonVisible = true;
                }
                else
                {
                    IsMessageButtonVisible = false;
                    IsDeactivateButtonVisible = false;
                    IsSendOfferButtonVisible = false;
                }
                IsRatingVisible = true;
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
                    if (SkillProviderDto.UserStatus == "Active")
                    {
                        IsUserCurrentlyActive = true;
                    }
                    else
                    {
                        IsUserCurrentlyActive = false;

                        if(_sessionService.GetCurrentUserRole() != UserRole.Admin) // If SP is deactivated but current user is not an admin, then they have no access
                        {
                            await Shell.Current.DisplayAlert("Information", "This user is currently deactivated. You cannot view their profile.", "Ok");
                            await _navigationService.GoBackAsync();
                        }
                    }
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
        public async Task MessageUser()
        {
            await _navigationService.NavigateToAsync("MessagesView", new Dictionary<string, object> { { "ChatId", Guid.Empty }, { "ReceiverId", SkillProviderDto.UserId }, { "ReceiverName", SkillProviderDto.UserName } });
        }

        [RelayCommand]
        public async Task ToggleUserActivation()
        {
            IsUserCurrentlyActive = !IsUserCurrentlyActive;

            string status = IsUserCurrentlyActive ? "Activated" : "Deactivated";
            try
            {
                var response = await _accountServiceClient.ActivateDeactivateUserAsync(SkillProviderId);
                if (response.Success)
                {
                    await Shell.Current.DisplayAlert("Success", response.Data, "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", response.Data, "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred while sending data: {ex.Message}", "Ok");
            }
        }

        [RelayCommand]
        public async Task ViewSkill(SkillDto skillDto)
        {
            int index = skillProviderDto.Skills.IndexOf(skillDto);
            await _navigationService.NavigateToAsync("ViewSkillView", new Dictionary<string, object> { { "SkillIndex", index }, { "SkillProviderId", skillProviderDto.UserId } });
            // Logic for viewing skill, navigate to ViewSkillView as viewer mode
        }



        [RelayCommand]
        public async Task ViewProject(SkillProviderProfileProjectsDto skillProviderProfileProjectsDto)
        {
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", skillProviderProfileProjectsDto.ProjectId } });
        }

        [RelayCommand]
        public async Task SendOffer()
        {
            await _navigationService.NavigateToAsync("SendOfferView", new Dictionary<string, object> { { "SkillProviderId", SkillProviderId }, { "SkillProviderFullName", SkillProviderDto.UserName } });
        }
    }
}
