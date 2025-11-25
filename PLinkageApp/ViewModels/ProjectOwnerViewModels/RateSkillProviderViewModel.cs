using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ProjectUpdateDto), "ProjectUpdateDto")]
    public partial class RateSkillProviderViewModel : ObservableObject
    {
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private ObservableCollection<RateSkillProviderIndividualDto> skillProvidersToRate = new();
        [ObservableProperty]
        private bool isBusy = false;
        public ProjectUpdateDto ProjectUpdateDto { get; set; }

        public RateSkillProviderViewModel(
            IDialogService dialogService,
            IProjectServiceClient projectServiceClient,
            ISessionService sessionService,
            INavigationService navigationService)
        {
            _projectServiceClient = projectServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }
      
        public async Task InitializeAsync()
        {
            IsBusy = true;
            try
            {
                foreach(var member in ProjectUpdateDto.ProjectMembers)
                {
                    SkillProvidersToRate.Add(new RateSkillProviderIndividualDto
                    {
                        FullName = member.UserName,
                        SkillProviderId = member.MemberId,
                        SkillProviderRating = 5
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialization error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SubmitRatings()
        {
            if (IsBusy)
                return;
            // 1. Validation
            if (SkillProvidersToRate.Any(sp => sp.SkillProviderRating <= 0))
            {
                await _dialogService.ShowAlertAsync("Incomplete Ratings", "Please rate all skill providers before submitting.", "OK");
                return;
            }
            IsBusy = true;
            try
            {
                var updateResult = await _projectServiceClient.UpdateProjectAsync(ProjectUpdateDto);

                if (!updateResult.Success)
                {
                    await _dialogService.ShowAlertAsync("Update Failed", $"Could not update project: {updateResult.Message}. Try again.", "Ok");
                    IsBusy = false;
                    return;
                }

                var ratingResult = await _projectServiceClient.RateSkillProvidersAsync(new RateSkillProviderDto
                {
                    rateSkillProviderIndividualDtos = SkillProvidersToRate.ToList()
                });

                if (!ratingResult.Success)
                {
                    await _dialogService.ShowAlertAsync("Partial Success", $"Project marked as completed, but ratings failed to save: {ratingResult.Message}", "Ok");
                    IsBusy = false;
                    return;
                }

                await _dialogService.ShowAlertAsync("Success", "Project completed and ratings submitted!", "Ok");

                await _navigationService.NavigateToAsync("../..", new Dictionary<string, object> { { "ForceReset", true } });
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error", $"Operation failed due to: {ex.Message}", "Ok");
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
