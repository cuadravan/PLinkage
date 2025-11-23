using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ProjectId), "ProjectId")]
    public partial class SendApplicationViewModel : ObservableObject
    {
        // Services
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly IOfferApplicationServiceClient _offerApplicationServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private bool _isInitialized = false;

        // Properties
        [ObservableProperty] private string projectName;
        [ObservableProperty] private string skillProviderFullName;
        [ObservableProperty] private string rateAsked;
        [ObservableProperty] private string timeFrameAsked;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool isBusy = false;
        public Guid ProjectId { get; set; }

        private ProjectDto projectDto;

        public SendApplicationViewModel(IProjectServiceClient projectServiceClient, IOfferApplicationServiceClient offerApplicationServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _projectServiceClient = projectServiceClient;
            _offerApplicationServiceClient = offerApplicationServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
        }

        public async Task InitializeAsync()
        {
            var currentUserId = _sessionService.GetCurrentUserId();
            var currentUserRole = _sessionService.GetCurrentUserRole();

            if (currentUserId == Guid.Empty || currentUserRole != UserRole.SkillProvider)
            {
                await _navigationService.GoBackAsync();
                return;
            }
            IsBusy = true;
            try
            {
                var result = await _projectServiceClient.GetSpecificAsync(ProjectId);
                if(!result.Success || result.Data == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Could not fetch the project", "Ok");
                    await _navigationService.GoBackAsync();
                    IsBusy = false;
                    return;
                }

                projectDto = result.Data;
                ProjectName = projectDto.ProjectName;
                SkillProviderFullName = _sessionService.GetCurrentUserName();
                _isInitialized = true;
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Could not fetch the project due to following error: {ex}", "Ok");
                await _navigationService.GoBackAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Apply()
        {
            if (IsBusy || !_isInitialized)
                return;
            if (string.IsNullOrWhiteSpace(RateAsked) ||
                string.IsNullOrWhiteSpace(TimeFrameAsked))
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

            var allowedHours = (projectDto.ProjectEndDate - projectDto.ProjectStartDate).TotalHours;

            if (hours > allowedHours)
            {
                ErrorMessage = $"Your requested timeframe exceeds the project duration of {allowedHours:N0} hours.";
                return;
            }

            var application = new OfferApplicationCreationDto
            {
                UserRoleOfCreator = UserRole.SkillProvider,
                OfferApplicationType = "Application",
                SenderId = _sessionService.GetCurrentUserId(),
                ReceiverId = projectDto.ProjectOwnerId,
                ProjectId = projectDto.ProjectId,
                OfferApplicationRate = rate,
                OfferApplicationTimeFrame = hours
            };
            IsBusy = true;
            try
            {
                var result = await _offerApplicationServiceClient.CreateApplicationOffer(application);
                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Application Sent", "You have successfully applied to the project.", "OK");
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"Could not send application. Server returned this message: {result.Message}", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Could not send application due to following error: {ex}", "Ok");
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
