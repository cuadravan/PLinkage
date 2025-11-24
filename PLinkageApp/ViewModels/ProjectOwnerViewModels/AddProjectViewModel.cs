using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace PLinkageApp.ViewModels
{
    public partial class AddProjectViewModel : ObservableValidator
    {
        // Services
        private readonly IProjectServiceClient _projectServiceClient;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        // Form fields
        [ObservableProperty] 
        private CebuLocation? projectLocationSelected;
        [ObservableProperty, Required(ErrorMessage = "Project name is required.")] 
        private string projectName;
        [ObservableProperty, Required(ErrorMessage = "Project description is required.")] 
        private string projectDescription;
        [ObservableProperty] 
        private DateTime projectStartDate = DateTime.Now;
        [ObservableProperty] 
        private DateTime projectEndDate = DateTime.Now;
        [ObservableProperty, Required(ErrorMessage = "Project status is required.")] 
        private ProjectStatus? projectStatusSelected;
        [ObservableProperty] 
        private ObservableCollection<string> projectSkillsRequired = new();
        [ObservableProperty] 
        private List<ProjectMemberDetailDto> projectMemberDetails = new();
        [ObservableProperty, Required(ErrorMessage = "Priority is required.")] 
        private string projectPrioritySelected;
        [ObservableProperty, Range(1, int.MaxValue, ErrorMessage = "Resources needed must be at least 1.")] 
        private int projectResourcesNeeded;
        [ObservableProperty] 
        private DateTime projectDateCreated;
        [ObservableProperty] 
        private DateTime projectDateUpdated;
        [ObservableProperty] 
        private string errorMessage;
        [ObservableProperty] 
        private string durationSummary;
        [ObservableProperty] 
        private string selectedSkill;
        [ObservableProperty]
        private bool isBusy = false;

        public ObservableCollection<ProjectStatus> StatusOptions { get; } = new(Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>().Where(x => x != ProjectStatus.Completed));
        public ObservableCollection<string> PriorityOptions { get; } = new() { "Low", "Medium", "High" };
        public ObservableCollection<CebuLocation> CebuLocations { get; } = new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());

        // Constructor
        public AddProjectViewModel(IProjectServiceClient projectServiceClient, ISessionService sessionService, INavigationService navigationService)
        {
            _projectServiceClient = projectServiceClient;
            _sessionService = sessionService;
            _navigationService = navigationService;

            ProjectDateCreated = ProjectDateUpdated = DateTime.Now;
            UpdateDurationSummary();
        }

        [RelayCommand]
        private async Task Submit()
        {
            if (IsBusy)
                return;
            
            if (!ValidateForm())
                return;

            var newProject = new ProjectCreationDto
            {
                ProjectOwnerId = _sessionService.GetCurrentUserId(),
                ProjectName = ProjectName,
                ProjectLocation = ProjectLocationSelected,
                ProjectDescription = ProjectDescription,
                ProjectStartDate = ProjectStartDate,
                ProjectEndDate = ProjectEndDate,
                ProjectStatus = ProjectStatusSelected,
                ProjectSkillsRequired = ProjectSkillsRequired.ToList(),
                ProjectPriority = ProjectPrioritySelected,
                ProjectResourcesNeeded = ProjectResourcesNeeded,
                ProjectResourcesAvailable = ProjectResourcesNeeded,
                ProjectDateCreated = ProjectDateCreated,
                ProjectDateUpdated = ProjectDateUpdated
            };
            IsBusy = true;
            try
            {
                var result = await _projectServiceClient.AddProjectAsync(newProject);
                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "Successfully added project!", "Ok");
                    await _navigationService.NavigateToAsync("..", new Dictionary<string, object> { { "ForceReset", true } });
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed", $"Server returned following message. {result.Message}. Please try again.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Project creation failed due to following error: {ex}. Please try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Cancel() => await _navigationService.GoBackAsync();

        [RelayCommand]
        private void AddSkill()
        {
            if (IsBusy)
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
            if (IsBusy)
                return;
            if (ProjectSkillsRequired.Contains(skill))
                ProjectSkillsRequired.Remove(skill);
        }

        partial void OnProjectStartDateChanged(DateTime value) => UpdateDurationSummary();
        partial void OnProjectEndDateChanged(DateTime value) => UpdateDurationSummary();

        private void UpdateDurationSummary()
        {
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

            if (ProjectStatusSelected == ProjectStatus.Completed)
            {
                ErrorMessage = "Project cannot be created with status Completed.";
                return false;
            }
            return true;
        }
    }
}
