using System.ComponentModel;
using System.Runtime.CompilerServices;
using PLinkage.Domain.Interfaces;

namespace PLinkage.ApplicationLayer.ViewModels
{
    public class AppShellViewModel : INotifyPropertyChanged
    {
        private readonly ISessionService _sessionService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AppShellViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
            UpdateRoleProperties();
            IsAdmin = false;
            IsProjectOwner = true;
            IsSkillProvider = false;
            IsLoggedIn = false;
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        private bool _isProjectOwner;
        public bool IsProjectOwner
        {
            get => _isProjectOwner;
            set
            {
                _isProjectOwner = value;
                OnPropertyChanged();
            }
        }

        private bool _isSkillProvider;
        public bool IsSkillProvider
        {
            get => _isSkillProvider;
            set
            {
                _isSkillProvider = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoggedIn;

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        public void UpdateRoleProperties()
        {
            var role = _sessionService.GetCurrentUser()?.UserRole;

            IsAdmin = role == "Admin";
            IsProjectOwner = role == "ProjectOwner";
            IsSkillProvider = role == "SkillProvider";
            IsLoggedIn = role != null;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

