using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.ViewModels
{
    public partial class ProcessResignationViewModel: ObservableObject
    {
        // Services
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly IProjectOwnerServiceClient _projectOwnerServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<ResignationItemDto> ResignationItems { get; set; } = new ObservableCollection<ResignationItemDto>();

        public ProcessResignationViewModel(IProjectOwnerServiceClient projectOwnerServiceClient, IProjectServiceClient projectServiceClient, IOfferApplicationServiceClient offerApplicationServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _projectServiceClient = projectServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
            _projectOwnerServiceClient = projectOwnerServiceClient;
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
                var result = await _projectOwnerServiceClient.GetResignationAsync(currentUserId);
                if (!result.Success)
                {
                    await Shell.Current.DisplayAlert("Project Not Found", $"Server returned the following message: {result.Message}. Please contact administrator.", "OK");
                    await _navigationService.GoBackAsync();
                    return;
                }
                ResignationItems.Clear();
                foreach(var item in result.Data)
                {
                    ResignationItems.Add(item);
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Could not retrieve resignations due to following error: {ex}", "Ok");
                await _navigationService.GoBackAsync();
            }
        }

        [RelayCommand]
        private async Task ApproveResignation(ResignationItemDto dto)
        {

            var processResignationDto = new ProcessResignationDto
            {
                ProjectId = dto.ProjectId,
                SkillProviderId = dto.SkillProviderId,
                ProjectName = dto.ProjectName,
                SkillProviderName = dto.SkillProviderName,
                ApproveResignation = true
            };

            try
            {
                var result = await _projectServiceClient.ProcessResignationAsync(processResignationDto);

                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "You have successfully approved a resignation.", "Ok");
                    ResignationItems.Remove(dto);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"There was an error in processing the resignation. Server sent the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"There was an error in processing the resignation. Error: {ex}", "Ok");
            }
        }

        [RelayCommand]
        private async Task RejectResignation(ResignationItemDto dto)
        {

            var processResignationDto = new ProcessResignationDto
            {
                ProjectId = dto.ProjectId,
                SkillProviderId = dto.SkillProviderId,
                ProjectName = dto.ProjectName,
                SkillProviderName = dto.SkillProviderName,
                ApproveResignation = false
            };

            try
            {
                var result = await _projectServiceClient.ProcessResignationAsync(processResignationDto);

                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "You have successfully rejected a resignation.", "Ok");
                    ResignationItems.Remove(dto);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"There was an error in processing the resignation. Server sent the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"There was an error in processing the resignation. Error: {ex}", "Ok");
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", false } });
        }
    }
}
