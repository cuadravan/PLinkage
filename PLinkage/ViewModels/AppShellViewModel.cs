using System.ComponentModel;
using System.Runtime.CompilerServices;
using PLinkage.Interfaces;

namespace PLinkage.ViewModels
{
    public class AppShellViewModel : INotifyPropertyChanged
    {
        private readonly ISessionService _sessionService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AppShellViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
            UpdateRoleProperties();
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

        private bool _isNotLoggedIn;
        public bool IsNotLoggedIn
        {
            get => _isNotLoggedIn;
            set
            {
                _isNotLoggedIn = value;
                OnPropertyChanged();
            }
        }

        public void UpdateRoleProperties()
        {
            var role = _sessionService.GetCurrentUser()?.UserRole;

            IsAdmin = role == UserRole.Admin;
            IsProjectOwner = role == UserRole.ProjectOwner;
            IsSkillProvider = role == UserRole.SkillProvider;
            IsNotLoggedIn = role == null;

        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

