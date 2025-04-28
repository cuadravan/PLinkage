using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkage.Interfaces;
using PLinkage.Models;
using Windows.Networking;

namespace PLinkage.ViewModels
{
    public partial class UpdateProjectViewModel: ObservableValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;
        private Guid _projectId;

        public UpdateProjectViewModel(IUnitOfWork unitOfWork, ISessionService sessionService, INavigationService navigationService)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _navigationService = navigationService;

            ProjectDateCreated = DateTime.Now;
            ProjectDateUpdated = DateTime.Now;

            OnAppearingCommand = new AsyncRelayCommand(OnAppearing);
        }

        public IAsyncRelayCommand OnAppearingCommand { get; }

        public async Task OnAppearing()
        {
            _projectId = _sessionService.VisitingProjectID;
            if (_projectId == Guid.Empty)
            {
                return;
            }
            await _unitOfWork.ReloadAsync();
            await LoadProjectDetailsAsync();
        }

        private async Task LoadProjectDetailsAsync()
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(_projectId);
            ProjectName = project.ProjectName;
            ProjectLocation = project.ProjectLocation;
            ProjectDescription = project.ProjectDescription;
            ProjectStartDate = project.ProjectStartDate;
            ProjectEndDate = project.ProjectEndDate;
            ProjectPriority = project.ProjectPriority;
            ProjectStatus = project.ProjectStatus;
            ProjectSkillsRequired = new ObservableCollection<string>(project.ProjectSkillsRequired);
            ProjectMembersId = project.ProjectMembersId;
            ProjectResourcesNeeded = project.ProjectResourcesNeeded;
            ProjectDateCreated = project.ProjectDateCreated;
            ProjectDateUpdated = project.ProjectDateUpdated;
            UpdateDurationSummary();
            await LoadEmployedSkillProviders();
        }

        private void UpdateDurationSummary()
        {
            if (ProjectEndDate >= ProjectStartDate)
            {
                var duration = ProjectEndDate - ProjectStartDate;
                var days = duration.TotalDays;
                var weeks = Math.Floor(days / 7);
                var months = Math.Floor(days / 30);

                DurationSummary = $"{(int)days} days | {weeks} weeks | {months} months";
            }
            else
            {
                DurationSummary = "Invalid date range";
            }
        }
        private async Task LoadEmployedSkillProviders()
        {
            var employedSkillProvidersList = await _unitOfWork.SkillProvider.GetAllAsync();
            IEnumerable<SkillProvider> filtered = employedSkillProvidersList
                .Where(sp => ProjectMembersId.Contains(sp.UserId))
                .ToList();
            EmployedSkillProviders = new ObservableCollection<SkillProvider>(filtered);
        }

        public ObservableCollection<ProjectStatus> StatusOptions { get; } =
            new(Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>());

        public ObservableCollection<string> PriorityOptions { get; } = new()
        {
            "Low", "Medium", "High"
        };

        public ObservableCollection<CebuLocation> CebuLocations { get; } =
            new(Enum.GetValues(typeof(CebuLocation)).Cast<CebuLocation>());

        [ObservableProperty]
        private CebuLocation? selectedLocation;

        [ObservableProperty]
        private CebuLocation? projectLocation;

        [ObservableProperty, Required(ErrorMessage = "Project name is required.")]
        private string projectName;

        [ObservableProperty, Required(ErrorMessage = "Project description is required.")]
        private string projectDescription;

        [ObservableProperty]
        private DateTime projectStartDate = DateTime.Now;

        [ObservableProperty]
        private DateTime projectEndDate = DateTime.Now;

        [ObservableProperty, Required(ErrorMessage = "Project status is required.")]
        private ProjectStatus? projectStatus;

        [ObservableProperty]
        private ObservableCollection<string> projectSkillsRequired = new();

        [ObservableProperty]
        private List<Guid> projectMembersId = new();

        [ObservableProperty, Required(ErrorMessage = "Priority is required.")]
        private string projectPriority;

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
        private ObservableCollection<SkillProvider> employedSkillProviders;

        partial void OnProjectStartDateChanged(DateTime value) => UpdateDurationSummary();
        partial void OnProjectEndDateChanged(DateTime value) => UpdateDurationSummary();

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
                !SelectedLocation.HasValue)
            {
                ErrorMessage = "Project Name, Description, and Location must not be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ProjectPriority) || !ProjectStatus.HasValue)
            {
                ErrorMessage = "Priority and Status must be selected.";
                return false;
            }

            if (ProjectEndDate < ProjectStartDate)
            {
                ErrorMessage = "End Date must be later than Start Date.";
                return false;
            }

            return true;
        }

        [RelayCommand]
        private async Task Update()
        {
            if (!ValidateForm())
                return;

            var project = await _unitOfWork.Projects.GetByIdAsync(_projectId);

            project.ProjectName = ProjectName;

            var newProject = new Project
            {
                ProjectId = Guid.NewGuid(),
                ProjectOwnerId = _sessionService.GetCurrentUser().UserId,
                ProjectName = ProjectName,
                ProjectLocation = SelectedLocation,
                ProjectDescription = ProjectDescription,
                ProjectStartDate = ProjectStartDate,
                ProjectEndDate = ProjectEndDate,
                ProjectStatus = ProjectStatus,
                ProjectSkillsRequired = ProjectSkillsRequired.ToList(),
                ProjectMembersId = ProjectMembersId,
                ProjectPriority = ProjectPriority,
                ProjectResourcesNeeded = ProjectResourcesNeeded,
                ProjectDateCreated = ProjectDateCreated,
                ProjectDateUpdated = ProjectDateUpdated
            };

            

            await _unitOfWork.Projects.AddAsync(newProject);
            await _unitOfWork.SaveChangesAsync();

            // ✅ Toast success
            await Shell.Current.DisplayAlert("Success", "Project created successfully!", "OK");

            await _navigationService.NavigateToAsync("ProjectOwnerProfileView");
        }

        [RelayCommand]
        private async Task Update()
        {
            if (!ValidateForm()) return;

            var user = await _unitOfWork.Projects.GetByIdAsync(_sessionService.VisitingProjectID);

            user.UserFirstName = FirstName;
            user.UserLastName = LastName;
            user.UserBirthDate = Birthdate;
            user.UserGender = IsMale ? "Male" : "Female";
            user.UserPhone = MobileNumber;
            user.UserLocation = SelectedLocation;

            await _unitOfWork.ProjectOwner.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _sessionService.SetCurrentUser(user);
            ErrorMessage = string.Empty;

            await _navigationService.NavigateToAsync("ProjectOwnerProfileView");
        }

        [RelayCommand]
        private Task Clear() => LoadCurrentProfile();

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.NavigateToAsync("ProjectOwnerViewProjectView");
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
            if (EmployedSkillProviders.Contains(skillProvider))
                EmployedSkillProviders.Remove(skillProvider);
        }
    }
}
