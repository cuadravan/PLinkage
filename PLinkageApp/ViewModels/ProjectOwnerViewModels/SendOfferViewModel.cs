using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly IOfferApplicationServiceClient _offerApplicationServiceClient;
        private readonly IProjectOwnerServiceClient _projectOwnerServiceClient;
        private readonly ISkillProviderServiceClient _skillProviderServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        // Properties
        [ObservableProperty] private ProjectOwnerProfileProjectDto selectedProject;
        [ObservableProperty] private string skillProviderFullName;
        [ObservableProperty] private string rateAsked;
        [ObservableProperty] private string timeFrameAsked;
        [ObservableProperty] private string errorMessage;
        public Guid SkillProviderId { get; set; }

        //private SkillProviderDto skillProviderDto;
        private ProjectOwnerDto ownerDto;

        public ObservableCollection<ProjectOwnerProfileProjectDto> Projects { get; set; } = new();

        public SendOfferViewModel(IProjectOwnerServiceClient projectOwnerServiceClient, ISkillProviderServiceClient skillProviderServiceClient, IProjectServiceClient projectServiceClient, IOfferApplicationServiceClient offerApplicationServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _projectServiceClient = projectServiceClient;
            _offerApplicationServiceClient = offerApplicationServiceClient;
            _projectOwnerServiceClient = projectOwnerServiceClient;
            _skillProviderServiceClient = skillProviderServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
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
                // First fetch the Skill Provider's information
                //var result = await _skillProviderServiceClient.GetSpecificAsync(SkillProviderId);
                //if (!result.Success || result.Data == null)
                //{
                //    await Shell.Current.DisplayAlert("Error", "Could not fetch the skill provider's information", "Ok");
                //    await _navigationService.GoBackAsync();
                //    return;
                //}
                //skillProviderDto = result.Data;
                //SkillProviderFullName = skillProviderDto.UserName;

                var result = await _projectOwnerServiceClient.GetSpecificAsync(currentUserId);
                if (!result.Success || result.Data == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Could not fetch the current user's information", "Ok");
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
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Could not fetch the project due to following error: {ex}", "Ok");
                await _navigationService.GoBackAsync();
                return;
            }
        }

        [RelayCommand]
        private async Task SendOffer()
        {
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

            try
            {
                var result = await _offerApplicationServiceClient.CreateApplicationOffer(offer);
                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Application Sent", "You have successfully sent an offer to the skill provider.", "OK");
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"Could not send offer. Server returned this message: {result.Message}", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Could not send offer due to following error: {ex}", "Ok");
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
