using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ProjectId), "ProjectId")]
    public partial class ResignProjectViewModel : ObservableObject
    {
        // Services
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private Guid _projectId;

        // Properties
        [ObservableProperty] private string projectName;
        [ObservableProperty] private string skillProviderFullName;
        [ObservableProperty] private string resignationReason;
        [ObservableProperty] private string errorMessage;
        public Guid ProjectId { get; set; }

        private ProjectDto projectDto;

        public ResignProjectViewModel(IProjectServiceClient projectServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _projectServiceClient = projectServiceClient;
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
                var result = await _projectServiceClient.GetSpecificAsync(ProjectId);
                if (!result.Success || result.Data == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Could not fetch the project", "Ok");
                    await _navigationService.GoBackAsync();
                    return;
                }

                projectDto = result.Data;
                ProjectName = projectDto.ProjectName;
                SkillProviderFullName = _sessionService.GetCurrentUserName();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Could not fetch the project due to following error: {ex}", "Ok");
                await _navigationService.GoBackAsync();
                return;
            }
        }

        [RelayCommand]
        private async Task SendResignation()
        {
            if (string.IsNullOrWhiteSpace(ResignationReason))
            {
                ErrorMessage = "Please complete all fields before applying.";
                return;
            }

            var resignation = new RequestResignationDto
            {
                ProjectId = ProjectId,
                SkillProviderId = _sessionService.GetCurrentUserId(),
                Reason = ResignationReason
            };

            try
            {
                var result = await _projectServiceClient.RequestResignationAsync(resignation);
                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Resignation Sent", "You have successfully sent a resignation request to the project owner.", "OK");
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"Could not send resignation request. Server returned this message: {result.Message}", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Could not send resignation request due to following error: {ex}", "Ok");
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
