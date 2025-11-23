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
        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly INavigationService _navigationService;

        private bool _isInitialized = false;

        [ObservableProperty]
        private bool isUserCurrentlyActive;

        [ObservableProperty]
        private bool isRatingVisible;

        [ObservableProperty]
        private bool isMessageButtonVisible;

        [ObservableProperty]
        private bool isDeactivateButtonVisible;

        [ObservableProperty]
        private bool isSendOfferButtonVisible;

        [ObservableProperty]
        private bool isUserActivated;

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private SkillProviderDto skillProviderDto;

        public Guid SkillProviderId { get; set; }

        public ViewSkillProviderProfileViewModel(ISessionService sessionService, IAccountServiceClient accountServiceClient, ISkillProviderServiceClient skillProviderServiceClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _accountServiceClient = accountServiceClient;
            _skillProviderServiceClient = skillProviderServiceClient;
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

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadUserDataAsync();
        }

        [RelayCommand]
        private async Task MessageUser()
        {
            if (!_isInitialized)
                return;
            await _navigationService.NavigateToAsync("MessagesView", new Dictionary<string, object> { { "ChatId", Guid.Empty }, { "ReceiverId", SkillProviderDto.UserId }, { "ReceiverName", SkillProviderDto.UserName } });
        }

        [RelayCommand]
        private async Task ToggleUserActivation()
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
        private async Task ViewSkill(SkillDto skillDto)
        {
            if (!_isInitialized)
                return;
            await _navigationService.NavigateToAsync("ViewSkillView", new Dictionary<string, object> { { "SkillIndex", skillProviderDto.Skills.IndexOf(skillDto) }, { "SkillProviderId", skillProviderDto.UserId } });
        }

        [RelayCommand]
        private async Task ViewProject(SkillProviderProfileProjectsDto skillProviderProfileProjectsDto)
        {
            if (!_isInitialized)
                return;
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", skillProviderProfileProjectsDto.ProjectId } });
        }

        [RelayCommand]
        private async Task SendOffer()
        {
            if (!_isInitialized)
                return;
            await _navigationService.NavigateToAsync("SendOfferView", new Dictionary<string, object> { { "SkillProviderId", SkillProviderId }, { "SkillProviderFullName", SkillProviderDto.UserName } });
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
