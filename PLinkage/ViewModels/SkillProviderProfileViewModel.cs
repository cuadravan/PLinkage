using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PLinkage.Interfaces;
using PLinkage.Models;
using System.Globalization;

namespace PLinkage.ViewModels
{
    public partial class SkillProviderProfileViewModel : ObservableObject
    {
        // Services
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        private Guid _skillProviderId;

        // User Info
        [ObservableProperty] private string userName;
        [ObservableProperty] private string userLocation;
        [ObservableProperty] private DateTime dateJoined;
        [ObservableProperty] private string userGender;
        [ObservableProperty] private string userEmail;
        [ObservableProperty] private string userPhone;
        [ObservableProperty] private double userRating;


        // Data Collections
        [ObservableProperty] private ObservableCollection<Skill> skills = new();
        [ObservableProperty] private ObservableCollection<Education> educations = new();
        [ObservableProperty] private ObservableCollection<Project> employedProjects = new();

        public IAsyncRelayCommand OnViewAppearingCommand { get; }

        public SkillProviderProfileViewModel(
            IUnitOfWork unitOfWork,
            ISessionService sessionService,
            INavigationService navigationService)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _navigationService = navigationService;
            OnViewAppearingCommand = new AsyncRelayCommand(OnViewAppearing);
        }

        public async Task OnViewAppearing()
        {
            var currentUser = _sessionService.GetCurrentUser();
            _skillProviderId = currentUser.UserId;

            await _unitOfWork.ReloadAsync();
            await LoadProfileAsync();
            await LoadEmployedProjectsAsync();
        }


        private async Task LoadProfileAsync()
        {
            var profile = await _unitOfWork.SkillProvider.GetByIdAsync(_skillProviderId);
            if (profile == null) return;

            UserName = $"{profile.UserFirstName} {profile.UserLastName}";
            UserLocation = profile.UserLocation?.ToString() ?? "Not specified";
            DateJoined = profile.JoinedOn;
            UserGender = profile.UserGender;
            UserEmail = profile.UserEmail;
            UserPhone = profile.UserPhone;
            UserRating = profile.UserRating;

            Educations = new ObservableCollection<Education>(profile.Educations);
            Skills = new ObservableCollection<Skill>(profile.Skills);
        }

        private async Task LoadEmployedProjectsAsync()
        {
            EmployedProjects.Clear();

            var allProjects = await _unitOfWork.Projects.GetAllAsync();
            var employedIn = allProjects.Where(p =>
                p.ProjectMembers.Any(m => m.MemberId == _skillProviderId));

            foreach (var project in employedIn)
            {
                EmployedProjects.Add(project);
            }
        }

        // Commands
        [RelayCommand]
        private async Task AddEducation ()
        {
            await _navigationService.NavigateToAsync("/SkillProviderAddEducationView");
        }
        [RelayCommand]
        private async Task UpdateEducation(Education education)
        {
            if (education == null || Educations == null) return;

            int index = Educations.IndexOf(education);
            if (index >= 0)
            {
                _sessionService.VisitingSkillEducationID = index;
                await _navigationService.NavigateToAsync("/SkillProviderUpdateEducationView");
            }
        }

        [RelayCommand]
        private async Task AddSkill ()
        {
            await _navigationService.NavigateToAsync("/SkillProviderAddSkillView");
        }
        [RelayCommand]
        private async Task UpdateSkill (Skill skill)
        {
            if (skill == null || Skills == null) return;

            int index = Skills.IndexOf(skill);
            if (index >= 0)
            {
                _sessionService.VisitingSkillEducationID = index;
                await _navigationService.NavigateToAsync("/SkillProviderUpdateSkillView");
            }
        }
        [RelayCommand]
        private async Task UpdateProfile()
        {
            await _navigationService.NavigateToAsync("/ProjectOwnerUpdateProfileView");
        }
        [RelayCommand]
        private async Task ViewProject(Project project)
        {
            _sessionService.VisitingProjectID = project.ProjectId;
            await _navigationService.NavigateToAsync("/ViewProjectView");
        }

        [RelayCommand]
        private async Task ResignProject(Project project)
        {
            if (project == null)
                return;

            // 1. Ask for confirmation
            var confirm = await Shell.Current.DisplayAlert(
                "Resign from Project",
                $"Are you sure you want to resign from \"{project.ProjectName}\"? This cannot be undone.",
                "Yes",
                "No");
            if (!confirm)
                return;

            // 2. Load fresh copies
            var skillProvider = await _unitOfWork.SkillProvider.GetByIdAsync(_skillProviderId);
            var proj = await _unitOfWork.Projects.GetByIdAsync(project.ProjectId);
            if (skillProvider == null || proj == null)
                return;

            // 3. Remove member from project
            proj.ProjectMembers.RemoveAll(m => m.MemberId == _skillProviderId);
            proj.ProjectResourcesAvailable = proj.ProjectResourcesNeeded - proj.ProjectMembers.Count;
            proj.ProjectDateUpdated = DateTime.Now;
            await _unitOfWork.Projects.UpdateAsync(proj);

            // 4. Remove project from skill-provider’s employed list
            if (skillProvider.EmployedProjects.Contains(proj.ProjectId))
                skillProvider.EmployedProjects.Remove(proj.ProjectId);
            await _unitOfWork.SkillProvider.UpdateAsync(skillProvider);

            // 5. Persist both changes
            await _unitOfWork.SaveChangesAsync();

            // 6. Update your UI collections
            EmployedProjects.Remove(project);
        }

    }
}
