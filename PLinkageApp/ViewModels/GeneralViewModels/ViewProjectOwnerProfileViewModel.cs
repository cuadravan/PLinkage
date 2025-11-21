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
    [QueryProperty(nameof(ProjectOwnerId), "ProjectOwnerId")]
    public partial class ViewProjectOwnerProfileViewModel : ObservableObject
    {
        public Guid ProjectOwnerId { get; set; }

        [ObservableProperty]
        public bool isUserCurrentlyActive;

        [ObservableProperty]
        public bool isRatingVisible;

        [ObservableProperty]
        public bool isMessageButtonVisible;

        [ObservableProperty]
        public bool isDeactivateButtonVisible;

        [ObservableProperty]
        public bool isUserActivated;

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        public ProjectOwnerDto projectOwnerDto;

        private bool _isInitialized;

        private readonly ISessionService _sessionService;
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly IProjectOwnerServiceClient _projectownerServiceClient;
        private readonly INavigationService _navigationService;

        public ViewProjectOwnerProfileViewModel(ISessionService sessionService, IAccountServiceClient accountServiceClient, IProjectOwnerServiceClient projectOwnerServiceClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _accountServiceClient = accountServiceClient;
            _projectownerServiceClient = projectOwnerServiceClient;
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
                }
                else if(currentVisitorUserRole == UserRole.SkillProvider)
                {
                    IsMessageButtonVisible = true;
                    IsDeactivateButtonVisible = false;
                }
                else
                {
                    IsMessageButtonVisible = false;
                    IsDeactivateButtonVisible = false;
                }
                    IsRatingVisible = false;
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

                ApiResponse<ProjectOwnerDto> result = null;

                result = await _projectownerServiceClient.GetSpecificAsync(ProjectOwnerId);

                if (result.Success && result.Data != null)
                {

                    ProjectOwnerDto = result.Data;
                    if (ProjectOwnerDto.UserStatus == "Active")
                    {
                        IsUserCurrentlyActive = true;
                    }
                    else
                    {
                        IsUserCurrentlyActive = false;
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting project owner profile: {ex.Message}");
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
            await _navigationService.NavigateToAsync("MessagesView", new Dictionary<string, object> { { "ChatId", Guid.Empty }, { "ReceiverId", ProjectOwnerDto.UserId }, { "ReceiverName", ProjectOwnerDto.UserName } });
        }

        [RelayCommand]
        public async Task ToggleUserActivation()
        {
            IsUserCurrentlyActive = !IsUserCurrentlyActive;

            string status = IsUserCurrentlyActive ? "Activated" : "Deactivated";
            try
            {
                var response = await _accountServiceClient.ActivateDeactivateUserAsync(ProjectOwnerId);
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
        public async Task ViewProject(ProjectOwnerProfileProjectDto projectOwnerProfileProjectDto)
        {
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", projectOwnerProfileProjectDto.ProjectId } });
        }

    }
}
