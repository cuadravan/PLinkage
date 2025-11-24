using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ProjectId), "ProjectId")]
    public partial class UpdateProjectViewModel : ObservableValidator
    {
        // Services
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private ProjectDto _projectDto;

        private bool _isInitialized = false;
        private bool _projectMembersChanged = false;

        [ObservableProperty] private CebuLocation? projectLocationSelected;
        [ObservableProperty, Required(ErrorMessage = "Project name is required.")] private string projectName;
        [ObservableProperty, Required(ErrorMessage = "Project description is required.")] private string projectDescription;
        [ObservableProperty] private DateTime projectStartDate = DateTime.Now;
        [ObservableProperty] private DateTime projectEndDate = DateTime.Now;
        [ObservableProperty, Required(ErrorMessage = "Project status is required.")] private ProjectStatus? projectStatusSelected;
        [ObservableProperty] private ObservableCollection<string> projectSkillsRequired = new();
        [ObservableProperty] private ObservableCollection<ProjectMemberDetailDto> projectMembers = new();
        [ObservableProperty, Required(ErrorMessage = "Priority is required.")] private string projectPrioritySelected;
        [ObservableProperty, Range(1, int.MaxValue, ErrorMessage = "Resources needed must be at least 1.")] private int projectResourcesNeeded;
        [ObservableProperty] private DateTime projectDateCreated;
        [ObservableProperty] private DateTime projectDateUpdated;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private string durationSummary;
        [ObservableProperty] private string selectedSkill;
        [ObservableProperty] private DateTime projectMinStartDate = new DateTime(1900, 1, 1);
        [ObservableProperty] private bool isBusy = false;

        public Guid ProjectId { get; set; } = Guid.Empty;

        public ObservableCollection<ProjectStatus> StatusOptions { get; } = new(Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>());
        public ObservableCollection<string> PriorityOptions { get; } = new() { "Low", "Medium", "High" };
        public ObservableCollection<CebuLocation> CebuLocations { get; } =
            new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());

        // Constructor
        public UpdateProjectViewModel(IProjectServiceClient projectServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _projectServiceClient = projectServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;
        }

        public async Task InitializeAsync()
        {
            var userId = _sessionService.GetCurrentUserId();
            IsBusy = true;
            try
            {
                var result = await _projectServiceClient.GetSpecificAsync(ProjectId);
                if (result.Success && result.Data != null)
                {
                    if (result.Data.ProjectOwnerId != userId)
                    {
                        IsBusy = false;
                        return;
                    }
                        
                    _projectDto = result.Data;
                    if (_projectDto.ProjectStartDate.Date < DateTime.Now.Date)
                    {
                        ProjectMinStartDate = _projectDto.ProjectStartDate.Date;
                    }
                    else
                    {
                        ProjectMinStartDate = DateTime.Now.Date;
                    }
                    _isInitialized = true;

                    ProjectLocationSelected = (CebuLocation?)Enum.Parse(typeof(CebuLocation), _projectDto.ProjectLocation);
                    ProjectName = _projectDto.ProjectName;
                    ProjectDescription = _projectDto.ProjectDescription;
                    ProjectStartDate = _projectDto.ProjectStartDate;
                    ProjectEndDate = _projectDto.ProjectEndDate;
                    ProjectPrioritySelected = _projectDto.ProjectPriority;
                    ProjectResourcesNeeded = _projectDto.ProjectResourcesNeeded;
                    ProjectStatusSelected = (ProjectStatus?)Enum.Parse(typeof(ProjectStatus), _projectDto.ProjectStatus);
                    ProjectSkillsRequired.Clear();
                    foreach (var skill in _projectDto.ProjectSkillsRequired)
                        ProjectSkillsRequired.Add(skill);
                    ProjectMembers.Clear();
                    foreach (var member in _projectDto.ProjectMembers)
                        ProjectMembers.Add(member);
                    _projectMembersChanged = false;
                    ProjectDateCreated = _projectDto.ProjectDateCreated;
                    ProjectDateUpdated = DateTime.Now;
                    UpdateDurationSummary();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "It seems the project you are trying to access does not exist. Please contact admin or refresh the app.", "Ok");
                    await _navigationService.GoBackAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Loading project failed due to following error: {ex}. Please try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        [RelayCommand]
        private async Task Reset()
        {
            if (IsBusy || !_isInitialized)
                return;

            ProjectLocationSelected = (CebuLocation?)Enum.Parse(typeof(CebuLocation), _projectDto.ProjectLocation);
            ProjectName = _projectDto.ProjectName;
            ProjectDescription = _projectDto.ProjectDescription;
            ProjectStartDate = _projectDto.ProjectStartDate;
            ProjectEndDate = _projectDto.ProjectEndDate;
            ProjectPrioritySelected = _projectDto.ProjectPriority;
            ProjectResourcesNeeded = _projectDto.ProjectResourcesNeeded;
            ProjectStatusSelected = (ProjectStatus?)Enum.Parse(typeof(ProjectStatus), _projectDto.ProjectStatus);
            ProjectSkillsRequired.Clear();
            foreach (var skill in _projectDto.ProjectSkillsRequired)
                ProjectSkillsRequired.Add(skill);
            ProjectMembers.Clear();
            foreach (var member in _projectDto.ProjectMembers)
                ProjectMembers.Add(member);
            _projectMembersChanged = false;
            ProjectDateCreated = _projectDto.ProjectDateCreated;
            ProjectDateUpdated = DateTime.Now;
            UpdateDurationSummary();
        }
        
        [RelayCommand]
        private async Task Submit()
        {
            if (IsBusy || !_isInitialized)
                return;
            if (!ValidateForm())
                return;

            ErrorMessage = string.Empty;

            var projectUpdateDto = new ProjectUpdateDto
            {
                ProjectId = _projectDto.ProjectId,
                ProjectDescription = ProjectDescription,
                ProjectStartDate = ProjectStartDate,
                ProjectEndDate = _projectDto.ProjectEndDate,
                ProjectStatus = ProjectStatusSelected,
                ProjectSkillsRequired = ProjectSkillsRequired.ToList(),
                ProjectPriority = ProjectPrioritySelected,
                ProjectResourcesNeeded = ProjectResourcesNeeded,
                ProjectDateUpdated = ProjectDateUpdated,
                ProjectMembers = ProjectMembers.ToList(),
                ProjectMembersChanged = _projectMembersChanged
            };
            IsBusy = true;
            try
            {
                if(projectUpdateDto.ProjectStatus is ProjectStatus.Completed)
                {
                    ProjectEndDate = DateTime.Now.Date; // Update this to trigger change in duration summary for display of completion details
                    projectUpdateDto.ProjectEndDate = DateTime.Now.Date; //Update project to end at when it was marked completed
                    await ShowProjectSummary();
                    await Shell.Current.DisplayAlert("Proceed To Rating Employed Providers", "You will now be redirected to rate your skill providers. Please note that you must submit the ratings in order to update the project, otherwise it will be forgone.", "Ok");
                    await _navigationService.NavigateToAsync("RateSkillProviderView", new Dictionary<string, object> { { "ProjectUpdateDto", projectUpdateDto} });
                }
                else
                {
                    var result = await _projectServiceClient.UpdateProjectAsync(projectUpdateDto);
                    if (result.Success)
                    {
                        await Shell.Current.DisplayAlert("Success", "Successfully updated project!", "Ok");
                        await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", true } });
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Failed", $"Server returned following message. {result.Message}. Please try again.", "Ok");
                    }
                }  
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Project update failed due to following error: {ex}. Please try again.", "Ok");
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

        [RelayCommand]
        private void AddSkill()
        {
            if (IsBusy || !_isInitialized)
                return;
            var skill = SelectedSkill?.Trim();
            if (!string.IsNullOrWhiteSpace(skill) && !ProjectSkillsRequired.Contains(skill))
            {
                ProjectSkillsRequired.Add(skill);
                SelectedSkill = string.Empty;
            }
        }

        [RelayCommand]
        private void RemoveSkill(string skill)
        {
            if (IsBusy || !_isInitialized)
                return;
            if (ProjectSkillsRequired.Contains(skill))
                ProjectSkillsRequired.Remove(skill);
        }

        [RelayCommand]
        private async Task RemoveSkillProvider(ProjectMemberDetailDto member)
        {
            if (IsBusy || !_isInitialized)
                return;
            var confirm = await Shell.Current.DisplayAlert(
                "Remove Skill Provider",
                "Are you sure you want to remove this skill provider? This change won't be permanent until you submit the form.",
                "Yes",
                "No");

            if (!confirm)
                return;

            ProjectMembers.Remove(ProjectMembers.FirstOrDefault(m => m.MemberId == member.MemberId));
            _projectMembersChanged = true;
        }

        partial void OnProjectStartDateChanged(DateTime value) => UpdateDurationSummary();
        partial void OnProjectEndDateChanged(DateTime value) => UpdateDurationSummary();

        private void UpdateDurationSummary()
        {
            // 1. Use .Date to strip time, preventing partial day errors
            var start = ProjectStartDate.Date;
            var end = ProjectEndDate.Date;

            if (end < start)
            {
                DurationSummary = "Invalid date range";
                return;
            }

            var duration = end - start;

            double totalDays = duration.TotalDays + 1;

            DurationSummary = $"{(int)totalDays} days | {Math.Floor(totalDays / 7)} weeks | {Math.Floor(totalDays / 30)} months";
        }

        private bool ValidateForm()
        {
            ErrorMessage = string.Empty;
            ValidateAllProperties();

            if (HasErrors)
            {
                ErrorMessage = GetErrors()
                    .OfType<ValidationResult>()
                    .FirstOrDefault()?.ErrorMessage ?? "Validation failed.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ProjectName) ||
                string.IsNullOrWhiteSpace(ProjectDescription) ||
                string.IsNullOrWhiteSpace(ProjectPrioritySelected))
            {
                ErrorMessage = "Please ensure all required fields are correctly filled.";
                return false;
            }

            if (!ProjectLocationSelected.HasValue)
            {
                ErrorMessage = "Project location must be selected.";
                return false;
            }

            if (ProjectStartDate.Date == default || ProjectEndDate.Date == default)
            {
                ErrorMessage = "Project start and end dates must be set.";
                return false;
            }

            if (ProjectEndDate.Date < ProjectStartDate.Date)
            {
                ErrorMessage = "Project end date cannot be earlier than start date.";
                return false;
            }

            if (ProjectStartDate.Date == ProjectEndDate.Date)
            {
                ErrorMessage = "Project start date cannot be the same as the end date.";
                return false;
            }

            if (ProjectSkillsRequired.Count == 0)
            {
                ErrorMessage = "At least one skill is required for the project.";
                return false;
            }

            if ((ProjectStatusSelected == ProjectStatus.Deactivated) && ProjectMembers.Count > 0)
            {
                ErrorMessage = "Cannot deactivate project while there are employed skill providers.";
                return false;
            }

            if (ProjectResourcesNeeded < ProjectMembers.Count)
            {
                ErrorMessage = "Resources needed cannot be less than the number of currently employed members.";
                return false;
            }

            if (ProjectMembers.Count == 0 && (ProjectStatusSelected?.Equals(ProjectStatus.Completed) ?? false))
            {
                ErrorMessage = "A complete project cannot have no employed skill providers. If you want to close the project, mark it as deactivated instead.";
                return false;
            }

            return true;
        }
        private async Task ShowProjectSummary()
        {
            // Build skill providers list
            var providersList = string.Join("\n", ProjectMembers
                .Select(sp => $"- {sp.UserFirstName} {sp.UserLastName}"));

            // Build skills required list
            var skillsList = string.Join("\n", ProjectSkillsRequired
                .Select(s => $"- {s}"));

            await Shell.Current.DisplayAlert(
                "Actual Project Completion Details",
                $"Project: {ProjectName}\n\n" +
                $"Description: {ProjectDescription}\n\n" +
                $"Location: {ProjectLocationSelected}\n" +
                $"Priority: {ProjectPrioritySelected}\n" +
                $"Status: {ProjectStatusSelected}\n\n" +
                $"Duration: {DurationSummary}\n" +
                $"Start: {ProjectStartDate:MMMM d, yyyy}\n" +
                $"End: {ProjectEndDate:MMMM d, yyyy}\n\n" +
                $"Resources Needed: {ProjectResourcesNeeded}\n" +
                $"Members Employed: {ProjectMembers.Count}\n\n" +
                $"Skill Providers:\n{providersList}\n\n" +
                $"Skills Required:\n{skillsList}",
                "Continue to Rating");
        }
    }
}