using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(SkillProviderId), "SkillProviderId")]
    [QueryProperty(nameof(SkillProviderFullName), "SkillProviderFullName")]
    public partial class SendOfferViewModel : ObservableObject
    {
        // Services
        private readonly IOfferApplicationServiceClient _offerApplicationServiceClient;
        private readonly IProjectOwnerServiceClient _projectOwnerServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private bool _isInitialized = false;
        private ProjectOwnerDto ownerDto;

        // Properties
        [ObservableProperty] private ProjectOwnerProfileProjectDto selectedProject;
        [ObservableProperty] private string skillProviderFullName;
        [ObservableProperty] private string rateAsked;
        [ObservableProperty] private string timeFrameAsked;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool isBusy = false;
        public Guid SkillProviderId { get; set; }
       
        public ObservableCollection<ProjectOwnerProfileProjectDto> Projects { get; set; } = new();

        public SendOfferViewModel(IDialogService dialogService, IProjectOwnerServiceClient projectOwnerServiceClient, IOfferApplicationServiceClient offerApplicationServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _offerApplicationServiceClient = offerApplicationServiceClient;
            _projectOwnerServiceClient = projectOwnerServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        public async Task InitializeAsync()
        {
            var currentUserId = _sessionService.GetCurrentUserId();
            var currentUserRole = _sessionService.GetCurrentUserRole();

            if (currentUserId == Guid.Empty || currentUserRole != UserRole.ProjectOwner)
            {
                await _navigationService.GoBackAsync();
                return;
            }

            try
            {
                var result = await _projectOwnerServiceClient.GetSpecificAsync(currentUserId);
                if (!result.Success || result.Data == null)
                {
                    await _dialogService.ShowAlertAsync("Error", "Could not fetch the current user's information", "Ok");
                    await _navigationService.GoBackAsync();
                    return;
                }
                ownerDto = result.Data;
                Projects.Clear();
                foreach(var project in ownerDto.ProfileProjects)
                {
                    if(project.ProjectStatus is ProjectStatus.Active) // Can only send offer with active projects
                        Projects.Add(project);
                }
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error", $"Could not fetch the project due to following error: {ex}", "Ok");
                await _navigationService.GoBackAsync();
                return;
            }
        }

        [RelayCommand]
        private async Task SendOffer()
        {
            if (!_isInitialized || IsBusy)
                return;
            if (string.IsNullOrWhiteSpace(RateAsked) ||
                string.IsNullOrWhiteSpace(TimeFrameAsked) ||
                SelectedProject == null)
            {
                ErrorMessage = "Please complete all fields before applying.";
                return;
            }

            if (!decimal.TryParse(RateAsked, out decimal rate))
            {
                ErrorMessage = "Invalid rate. It should be a number";
                return;
            }

            if (!int.TryParse(TimeFrameAsked, out int hours))
            {
                ErrorMessage = "Invalid timeframe. It should be an integer representing number of hours.";
                return;
            }

            var allowedHours = (SelectedProject.ProjectEndDate - SelectedProject.ProjectStartDate).TotalHours;

            if (hours > allowedHours)
            {
                ErrorMessage = $"Your requested timeframe exceeds the project duration of {allowedHours:N0} hours.";
                return;
            }

            var offer = new OfferApplicationCreationDto
            {
                UserRoleOfCreator = UserRole.ProjectOwner,
                OfferApplicationType = "Offer",
                SenderId = _sessionService.GetCurrentUserId(),
                ReceiverId = SkillProviderId,
                ProjectId = SelectedProject.ProjectId,
                OfferApplicationRate = rate,
                OfferApplicationTimeFrame = hours
            };
            IsBusy = true;
            try
            {
                var result = await _offerApplicationServiceClient.CreateApplicationOffer(offer);
                if (result.Success)
                {
                    await _dialogService.ShowAlertAsync("Application Sent", "You have successfully sent an offer to the skill provider.", "OK");
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Error", $"Could not send offer. Server returned this message: {result.Message}", "OK");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error", $"Could not send offer due to following error: {ex}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            if (IsBusy)
                return;
            await _navigationService.GoBackAsync();
        }
    }
}
