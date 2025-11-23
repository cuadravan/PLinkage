using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(DisplayDto), "DisplayDto")]
    public partial class NegotiateViewModel : ObservableObject
    {
        // Services
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly IOfferApplicationServiceClient _offerApplicationServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private bool _isInitialized = false;

        // Properties
        [ObservableProperty] private string projectName;
        [ObservableProperty] private string skillProviderName;
        [ObservableProperty] private DateTime startDate;
        [ObservableProperty] private DateTime endDate;
        [ObservableProperty] private string requestedRate;
        [ObservableProperty] private string requestedTimeFrame;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool isBusy = false;
        public Guid ProjectId { get; set; }

        public OfferApplicationDisplayDto DisplayDto { get; set; }

        public NegotiateViewModel(IProjectServiceClient projectServiceClient, IOfferApplicationServiceClient offerApplicationServiceClient, ISessionService sessionService, INavigationService navigationService)
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

            try
            {
                var result = await _projectServiceClient.GetSpecificAsync(DisplayDto.ProjectId);
                if (!result.Success)
                {
                    await Shell.Current.DisplayAlert("Project Not Found", "Server does not contain instance of project. Please contact administrator.", "OK");
                    await _navigationService.GoBackAsync();
                    return;
                }
                ProjectName = result.Data.ProjectName;
                SkillProviderName = _sessionService.GetCurrentUserName();
                StartDate = result.Data.ProjectStartDate;
                EndDate = result.Data.ProjectEndDate;
                _isInitialized = true;

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Could not retrieve project details due to following error: {ex}", "Ok");
                await _navigationService.GoBackAsync();
            }
            
        }

        [RelayCommand]
        private async Task Negotiate()
        {
            if (!_isInitialized || IsBusy)
                return;
            if (string.IsNullOrWhiteSpace(RequestedRate) ||
                string.IsNullOrWhiteSpace(RequestedTimeFrame))
            {
                ErrorMessage = "Please complete all fields before applying.";
                return;
            }

            if (!decimal.TryParse(RequestedRate, out decimal rate))
            {
                ErrorMessage = "Invalid rate. It should be a number";
                return;
            }

            if (!int.TryParse(RequestedTimeFrame, out int hours))
            {
                ErrorMessage = "Invalid timeframe. It should be an integer representing number of hours.";
                return;
            }

            var allowedHours = (endDate - startDate).TotalHours;

            if (hours > allowedHours)
            {
                ErrorMessage = $"Your requested timeframe exceeds the project duration of {allowedHours:N0} hours.";
                return;
            }

            var processDto = new OfferApplicationProcessDto
            {
                OfferApplicationId = DisplayDto.OfferApplicationId,
                Process = "Negotiate",
                Type = DisplayDto.OfferApplicationType,
                SenderId = DisplayDto.SenderId,
                ReceiverId = DisplayDto.ReceiverId,
                ProjectId = DisplayDto.ProjectId,
                NegotiatedRate = decimal.Parse(RequestedRate),
                NegotiatedTimeFrame = int.Parse(RequestedTimeFrame)
            };
            IsBusy = true;
            try
            {
                var result = await _offerApplicationServiceClient.ProcessOfferApplication(processDto);

                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "You have successfully sent a negotiation to the project owner.", "Ok");
                    await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", true } });
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"There was an error in processing your negotiation. Server sent the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"There was an error in processing your negotiation. Error: {ex}", "Ok");
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
