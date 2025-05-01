using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkage.Models;
using PLinkage.Interfaces;

namespace PLinkage.ViewModels
{
    public partial class ViewProjectViewModel : ObservableObject
    {
        // Services
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private Guid _projectId;

        // Constructor
        public ViewProjectViewModel(IUnitOfWork unitOfWork, ISessionService sessionService, INavigationService navigationService)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _navigationService = navigationService;

            OnAppearingCommand = new AsyncRelayCommand(OnAppearing);
        }

        // Properties
        [ObservableProperty] private string projectName;
        [ObservableProperty] private CebuLocation? projectLocation;
        [ObservableProperty] private string projectDescription;
        [ObservableProperty] private DateTime projectStartDate;
        [ObservableProperty] private DateTime projectEndDate;
        [ObservableProperty] private string projectPriority;
        [ObservableProperty] private ProjectStatus? projectStatus;
        [ObservableProperty] private ObservableCollection<string> projectSkillsRequired = new();
        [ObservableProperty] private List<ProjectMemberDetail> projectMembers = new();
        [ObservableProperty] private int projectResourcesNeeded;
        [ObservableProperty] private DateTime projectDateCreated;
        [ObservableProperty] private DateTime projectDateUpdated;
        [ObservableProperty] private string durationSummary;
        [ObservableProperty] private ObservableCollection<EmployedSkillProviderWrapper> employedSkillProviders = new();

        public IAsyncRelayCommand OnAppearingCommand { get; }

        // Core logic
        public async Task OnAppearing()
        {
            _projectId = _sessionService.VisitingProjectID;
            if (_projectId == Guid.Empty) return;

            await _unitOfWork.ReloadAsync();
            await LoadProjectDetailsAsync();
        }

        private async Task LoadProjectDetailsAsync()
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(_projectId);
            if (project == null) return;

            ProjectName = project.ProjectName;
            ProjectLocation = project.ProjectLocation;
            ProjectDescription = project.ProjectDescription;
            ProjectStartDate = project.ProjectStartDate;
            ProjectEndDate = project.ProjectEndDate;
            ProjectPriority = project.ProjectPriority;
            ProjectStatus = project.ProjectStatus;
            ProjectSkillsRequired = new ObservableCollection<string>(project.ProjectSkillsRequired);
            ProjectMembers = project.ProjectMembers;
            ProjectResourcesNeeded = project.ProjectResourcesNeeded;
            ProjectDateCreated = project.ProjectDateCreated;
            ProjectDateUpdated = project.ProjectDateUpdated;

            UpdateDurationSummary();
            await LoadEmployedSkillProviders();
        }

        private async Task LoadEmployedSkillProviders()
        {
            var allSkillProviders = await _unitOfWork.SkillProvider.GetAllAsync();
            EmployedSkillProviders = new ObservableCollection<EmployedSkillProviderWrapper>(
        ProjectMembers.Select(pm =>
        {
            var sp = allSkillProviders.FirstOrDefault(s => s.UserId == pm.MemberId);
            return new EmployedSkillProviderWrapper
            {
                MemberId = pm.MemberId,
                FullName = sp != null ? $"{sp.UserFirstName} {sp.UserLastName}" : "Unknown",
                Email = sp?.UserEmail ?? "Unknown",
                Rate = pm.Rate,
                TimeFrame = pm.TimeFrame
            };
        }));
        }

        private void UpdateDurationSummary()
        {
            if (ProjectEndDate >= ProjectStartDate)
            {
                var duration = ProjectEndDate - ProjectStartDate;
                DurationSummary = $"{(int)duration.TotalDays} days | {Math.Floor(duration.TotalDays / 7)} weeks | {Math.Floor(duration.TotalDays / 30)} months";
            }
            else
            {
                DurationSummary = "Invalid date range";
            }
        }

        [RelayCommand]
        private async Task UpdateProject()
        {
            _sessionService.VisitingProjectID = _projectId;
            await _navigationService.NavigateToAsync("ProjectOwnerUpdateProjectView");
        }

        [RelayCommand]
        private async Task Back()
        {
            _sessionService.VisitingProjectID = Guid.Empty;
            await _navigationService.NavigateToAsync("ProjectOwnerProfileView");
        }
    }
}

public class EmployedSkillProviderWrapper
{
    public Guid MemberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int TimeFrame { get; set; }
}
