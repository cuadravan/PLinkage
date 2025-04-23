using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkage.Models;
using PLinkage.Interfaces;

namespace PLinkage.ViewModels
{
    public partial class ViewProjectViewModel: ObservableObject
    {
        private Guid _projectId;
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        public ViewProjectViewModel(IUnitOfWork unitOfWork, ISessionService sessionService, INavigationService navigationService)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _navigationService = navigationService;

            OnAppearingCommand = new AsyncRelayCommand(OnAppearing);
        }

        [ObservableProperty]
        private string projectName;

        [ObservableProperty]
        private CebuLocation? projectLocation;

        [ObservableProperty]
        private string projectDescription;

        [ObservableProperty]
        private DateTime projectStartDate;

        [ObservableProperty]
        private DateTime projectEndDate;

        [ObservableProperty]
        private string projectPriority;

        [ObservableProperty]
        private ProjectStatus? projectStatus;

        [ObservableProperty]
        private ObservableCollection<string> projectSkillsRequired;

        [ObservableProperty]
        private List<Guid> projectMembersId;    

        [ObservableProperty]
        private int projectResourcesNeeded;

        [ObservableProperty]
        private DateTime projectDateCreated;

        [ObservableProperty]
        private DateTime projectDateUpdated;

        [ObservableProperty]
        private string durationSummary;

        [ObservableProperty]
        private ObservableCollection<SkillProvider> employedSkillProviders;

        public IAsyncRelayCommand OnAppearingCommand { get; }

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

        [RelayCommand]
        private async Task Back()
        {
            await _navigationService.NavigateToAsync("ProjectOwnerProfileView"); // This will need a stack based system
            // ProjectOwner can view their own project and then navigate back to profile of projectowner
            // Skill Provider can view project and then go back to either home, browse projects, or a project owner profile
        }

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

        private async Task LoadEmployedSkillProviders()
        {
            var employedSkillProvidersList = await _unitOfWork.SkillProvider.GetAllAsync();
            IEnumerable<SkillProvider> filtered = employedSkillProvidersList
                .Where(sp => ProjectMembersId.Contains(sp.UserId))
                .ToList();
            EmployedSkillProviders = new ObservableCollection<SkillProvider>(filtered);
        }


    }
}
