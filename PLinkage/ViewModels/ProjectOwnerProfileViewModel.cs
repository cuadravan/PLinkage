using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkage.Interfaces;
using PLinkage.Models;
using PLinkage.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PLinkage.ViewModels
{
    public partial class ProjectOwnerProfileViewModel : ObservableObject
    {
        private Guid _projectOwnerId;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;
        public IAsyncRelayCommand OnViewAppearingCommand { get; }

        public ProjectOwnerProfileViewModel(IUnitOfWork unitOfWork, ISessionService sessionService, INavigationService navigationService)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _navigationService = navigationService;
            OnViewAppearingCommand = new AsyncRelayCommand(OnViewAppearing);
        }

        [ObservableProperty] private string userName;
        [ObservableProperty] private string userLocation;
        [ObservableProperty] private DateTime dateJoined;
        [ObservableProperty] private string userGender;
        [ObservableProperty] private string userEmail;
        [ObservableProperty] private string userPhone;
        [ObservableProperty] private ObservableCollection<Project> ownedProjects;

        public ObservableCollection<string> SortOptions { get; } = new()
        {
            "Active",
            "Completed",
            "Deactivated",
            "All"
        };

        private string sortSelection = "All";
        public string SortSelection
        {
            get => sortSelection;
            set
            {
                if (SetProperty(ref sortSelection, value))
                {
                    LoadProjectsAsync();
                }
            }
        }

        public async Task OnViewAppearing()
        {
            _projectOwnerId = _sessionService.VisitingProjectOwnerID;
            if (_projectOwnerId == Guid.Empty)
            {
                _projectOwnerId = _sessionService.GetCurrentUser().UserId;
            }
            await _unitOfWork.ReloadAsync();
            await LoadProfileAsync();
            await LoadProjectsAsync();
        }

        private async Task LoadProfileAsync()
        {
            var projectOwnerProfile = await _unitOfWork.ProjectOwner.GetByIdAsync(_projectOwnerId);
            UserName = $"{projectOwnerProfile.UserFirstName} {projectOwnerProfile.UserLastName}";
            UserLocation = projectOwnerProfile.UserLocation?.ToString() ?? "Not specified";
            DateJoined = projectOwnerProfile.JoinedOn;
            UserGender = projectOwnerProfile.UserGender;
            UserEmail = projectOwnerProfile.UserEmail;
            UserPhone = projectOwnerProfile.UserPhone;
        }

        private async Task LoadProjectsAsync()
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            var ownedProjects = projects.Where(p => p.ProjectOwnerId == _projectOwnerId).ToList();

            OwnedProjects = SortSelection switch
            {
                "Active" => new ObservableCollection<Project>(ownedProjects.Where(p => p.ProjectStatus == ProjectStatus.Active)),
                "Completed" => new ObservableCollection<Project>(ownedProjects.Where(p => p.ProjectStatus == ProjectStatus.Completed)),
                "Deactivated" => new ObservableCollection<Project>(ownedProjects.Where(p => p.ProjectStatus == ProjectStatus.Deactivated)),
                _ => new ObservableCollection<Project>(ownedProjects),
            };
        }

        [RelayCommand]
        public async Task UpdateProfile()
        {
            await _navigationService.NavigateToAsync("ProjectOwnerUpdateProfileView");
        }

        [RelayCommand]
        public void AddProject()
        {
            // Add project logic
        }
    }
}
