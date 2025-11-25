using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageApp.Services
{
    public class SessionService : ISessionService
    {
        private Guid currentUserId = Guid.Empty;
        private UserRole? currentUserRole = null;
        private string? currentUserName = null;
        private CebuLocation? currentUserLocation = null;

        public void SetCurrentUser(LoginResultDto loginResultDto)
        {
            currentUserId = loginResultDto.UserId;
            currentUserRole = loginResultDto.UserRole;
            currentUserName = loginResultDto.UserName;
            currentUserLocation = loginResultDto.UserLocation;
        }

        public Guid GetCurrentUserId()
        {
            return currentUserId;
        }
        public string? GetCurrentUserName()
        {
            return currentUserName;
        }
        public UserRole? GetCurrentUserRole()
        {
            return currentUserRole;
        }
        public CebuLocation? GetCurrentUserLocation()
        {
            return currentUserLocation;
        }

        public void ClearSession()
        {
            currentUserId = Guid.Empty;
            currentUserName = string.Empty;
            currentUserRole = null;
            currentUserLocation = null;
        }
        public bool IsLoggedIn()
        {
            return currentUserId != Guid.Empty; 
        }
    }
}
