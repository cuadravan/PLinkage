using PLinkage.Interfaces;

namespace PLinkage.Services
{
    public class SessionService : ISessionService
    {
        private IUser? _currentUser;

        public void SetCurrentUser(IUser user)
        {
            _currentUser = user; // We need login view model now to set user to NULL then in appshell view model, if null, then set to IsNotLoggedIn
        }

        public IUser? GetCurrentUser()
        {
            return _currentUser;
        }

        public void ClearSession()
        {
            _currentUser = null;
        }

        public bool IsLoggedIn()
        {
            return _currentUser != null;
        }

        public UserRole? GetCurrentUserType()
        {
            return _currentUser?.UserRole;
        }
    }
}
