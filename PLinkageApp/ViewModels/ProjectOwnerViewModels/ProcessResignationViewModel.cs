using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System.Collections.ObjectModel;

namespace PLinkageApp.ViewModels
{
    public partial class ProcessResignationViewModel: ObservableObject
    {
        // Services
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly IProjectOwnerServiceClient _projectOwnerServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private bool isBusy = false;

        public ObservableCollection<ResignationItemDto> ResignationItems { get; set; } = new ObservableCollection<ResignationItemDto>();

        public ProcessResignationViewModel(IDialogService dialogService, IProjectOwnerServiceClient projectOwnerServiceClient, IProjectServiceClient projectServiceClient, IOfferApplicationServiceClient offerApplicationServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _projectServiceClient = projectServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
            _projectOwnerServiceClient = projectOwnerServiceClient;
            _dialogService = dialogService;
        }


        public async Task InitializeAsync()
        {
            if (IsBusy)
                return;
            var currentUserId = _sessionService.GetCurrentUserId();
            var currentUserRole = _sessionService.GetCurrentUserRole();

            if (currentUserId == Guid.Empty || currentUserRole != UserRole.ProjectOwner)
            {
                await _navigationService.GoBackAsync();
                return;
            }

            IsBusy = true;
            try
            {
                var result = await _projectOwnerServiceClient.GetResignationAsync(currentUserId);
                if (!result.Success)
                {
                    await _dialogService.ShowAlertAsync("Resignations Not Found", $"Server returned the following message: {result.Message}. Please contact administrator.", "OK");
                    await _navigationService.GoBackAsync();                  
                }
                else
                {
                    ResignationItems.Clear();
                    foreach (var item in result.Data)
                    {
                        ResignationItems.Add(item);
                    }
                }                  
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error", $"Could not retrieve resignations due to following error: {ex}", "Ok");
                await _navigationService.GoBackAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ApproveResignation(ResignationItemDto dto)
        {
            if (IsBusy)
                return;
            IsBusy = true;
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
                    await _dialogService.ShowAlertAsync("Success", "You have successfully approved a resignation.", "Ok");
                    ResignationItems.Remove(dto);
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Error", $"There was an error in processing the resignation. Server sent the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error", $"There was an error in processing the resignation. Error: {ex}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RejectResignation(ResignationItemDto dto)
        {
            if (IsBusy)
                return;
            IsBusy = true;
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
                    await _dialogService.ShowAlertAsync("Success", "You have successfully rejected a resignation.", "Ok");
                    ResignationItems.Remove(dto);
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Error", $"There was an error in processing the resignation. Server sent the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error", $"There was an error in processing the resignation. Error: {ex}", "Ok");
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
            await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", false } });
        }
    }
}
