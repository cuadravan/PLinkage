using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLinkage.Interfaces;
using PLinkage.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;

namespace PLinkage.ViewModels
{
    public partial class UpdateProjectViewModel: ObservableValidator
    {
        // Services
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;
        private Guid _projectId;

        // Constructor
        public UpdateProjectViewModel(IUnitOfWork unitOfWork, ISessionService sessionService, INavigationService navigationService)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _navigationService = navigationService;

            OnAppearingCommand = new AsyncRelayCommand(OnAppearing);
        }

        // Form fields
        [ObservableProperty] private CebuLocation? projectLocationSelected;
        [ObservableProperty, Required(ErrorMessage = "Project name is required.")] private string projectName;
        [ObservableProperty, Required(ErrorMessage = "Project description is required.")] private string projectDescription;
        [ObservableProperty] private DateTime projectStartDate;
        [ObservableProperty] private DateTime projectEndDate;
        [ObservableProperty, Required(ErrorMessage = "Project status is required.")] private ProjectStatus? projectStatusSelected;
        [ObservableProperty] private ObservableCollection<string> projectSkillsRequired = new();
        [ObservableProperty] private List<ProjectMemberDetail> projectMembers = new();
        [ObservableProperty, Required(ErrorMessage = "Priority is required.")] private string projectPrioritySelected;
        [ObservableProperty, Range(1, int.MaxValue, ErrorMessage = "Resources needed must be at least 1.")] private int projectResourcesNeeded;
        [ObservableProperty] private DateTime projectDateCreated;
        [ObservableProperty] private DateTime projectDateUpdated;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private string durationSummary;
        [ObservableProperty] private string selectedSkill;
        [ObservableProperty] private ObservableCollection<SkillProvider> employedSkillProviders = new();

        public ObservableCollection<ProjectStatus> StatusOptions { get; } =
            new(Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>());

        public ObservableCollection<string> PriorityOptions { get; } = new() { "Low", "Medium", "High" };

        public ObservableCollection<CebuLocation> CebuLocations { get; } =
            new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());

        // Auto-update duration summary
        partial void OnProjectStartDateChanged(DateTime value) => UpdateDurationSummary();
        partial void OnProjectEndDateChanged(DateTime value) => UpdateDurationSummary();

        private void UpdateDurationSummary()
        {
            if (ProjectEndDate < ProjectStartDate)
            {
                DurationSummary = "Invalid date range";
                return;
            }

            var duration = ProjectEndDate - ProjectStartDate;
            DurationSummary = $"{(int)duration.TotalDays} days | {Math.Floor(duration.TotalDays / 7)} weeks | {Math.Floor(duration.TotalDays / 30)} months";
        }

        public IAsyncRelayCommand OnAppearingCommand { get; }

        // Core Methods
        public async Task OnAppearing()
        {
            _projectId = _sessionService.VisitingProjectID;
            if (_projectId == Guid.Empty) return;

            await _unitOfWork.ReloadAsync();
            await LoadCurrentProject();
        }
        private async Task LoadCurrentProject()
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(_sessionService.VisitingProjectID);
            if (project == null) return;

            ProjectName = project.ProjectName;
            ProjectDescription = project.ProjectDescription;
            ProjectStartDate = project.ProjectStartDate;
            ProjectLocationSelected = project.ProjectLocation;
            ProjectStartDate = project.ProjectStartDate;
            ProjectEndDate = project.ProjectEndDate;
            ProjectPrioritySelected = project.ProjectPriority;
            ProjectStatusSelected = project.ProjectStatus;
            ProjectSkillsRequired = new ObservableCollection<string>(project.ProjectSkillsRequired);
            ProjectMembers = project.ProjectMembers;
            ProjectResourcesNeeded = project.ProjectResourcesNeeded;
            ProjectDateCreated = project.ProjectDateCreated;
            ProjectDateUpdated = project.ProjectDateUpdated;
            await LoadEmployedSkillProviders();
            UpdateDurationSummary();
        }

        private async Task LoadEmployedSkillProviders()
        {
            var allSkillProviders = await _unitOfWork.SkillProvider.GetAllAsync();
            var memberIds = ProjectMembers.Select(pm => pm.MemberId);
            var filtered = allSkillProviders.Where(sp => memberIds.Contains(sp.UserId));
            EmployedSkillProviders = new ObservableCollection<SkillProvider>(filtered);
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
                !ProjectLocationSelected.HasValue ||
                string.IsNullOrWhiteSpace(ProjectPrioritySelected) ||
                ProjectEndDate < ProjectStartDate)
            {
                ErrorMessage = "Please ensure all required fields are correctly filled.";
                return false;
            }
            if (ProjectResourcesNeeded < ProjectMembers.Count)
            {
                ErrorMessage = "Resources needed cannot be less than the number of currently employed members.";
                return false;
            }
            if (EmployedSkillProviders.Count == 0 && (ProjectStatusSelected?.Equals(ProjectStatus.Completed) ?? false))
            {
                ErrorMessage = "A complete project cannot have no employed skill providers";
                return false;
            }

            return true;
        }

        // Commands

        [RelayCommand]
        private async Task Submit()
        {
            if (!ValidateForm())
                return;

            var project = await _unitOfWork.Projects.GetByIdAsync(_sessionService.VisitingProjectID);
            if (project == null) return;

            project.ProjectDescription = ProjectDescription;
            project.ProjectPriority = ProjectPrioritySelected;
            project.ProjectStartDate = ProjectStartDate;
            project.ProjectSkillsRequired = ProjectSkillsRequired.ToList();
            project.ProjectResourcesNeeded = ProjectResourcesNeeded;
            project.ProjectStatus = ProjectStatusSelected;
            project.ProjectDateUpdated = DateTime.Now;

            if(project.ProjectStatus == ProjectStatus.Completed)
            {
                project.ProjectEndDate = ProjectEndDate;
                await _unitOfWork.Projects.UpdateAsync(project);
                await _unitOfWork.SaveChangesAsync();
                ErrorMessage = string.Empty;
                await _navigationService.NavigateToAsync("ProjectOwnerRateSkillProviderView");
            }
            else
            {
                await _unitOfWork.Projects.UpdateAsync(project);
                await _unitOfWork.SaveChangesAsync();
                ErrorMessage = string.Empty;

                await Shell.Current.DisplayAlert("Success", "Project updated successfully!", "OK");
                await _navigationService.NavigateToAsync("ProjectOwnerProfileView");
            }
                
        }

        [RelayCommand]
        private async Task Reset()
        {
            await LoadCurrentProject();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            var result = await Shell.Current.DisplayAlert("Cancel", "Are you sure you want to cancel?", "Yes", "No");
            if (result)
            {
                await _navigationService.NavigateToAsync("ProjectOwnerProfileView");
            }
        }

        [RelayCommand]
        private void AddSkill()
        {
            if (!string.IsNullOrWhiteSpace(SelectedSkill) && !ProjectSkillsRequired.Contains(SelectedSkill.Trim()))
            {
                ProjectSkillsRequired.Add(SelectedSkill.Trim());
                SelectedSkill = string.Empty;
            }
        }

        [RelayCommand]
        private void RemoveSkill(string skill)
        {
            if (ProjectSkillsRequired.Contains(skill))
                ProjectSkillsRequired.Remove(skill);
        }

        [RelayCommand]
        private void RemoveSkillProvider(SkillProvider skillProvider)
        {
            if(EmployedSkillProviders.Contains(skillProvider))
            {
                EmployedSkillProviders.Remove(skillProvider);
                ProjectMembers.RemoveAll(m => m.MemberId == skillProvider.UserId);
            }
        }
    }
}
