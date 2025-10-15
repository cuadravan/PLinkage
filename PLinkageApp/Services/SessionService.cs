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
        private Guid visitingProjectOwnerID = Guid.Empty;
        private Guid visitingSkillProviderID = Guid.Empty;
        private Guid visitingProjectID = Guid.Empty;
        private Guid visitingReceiverID = Guid.Empty;

        public Guid VisitingProjectOwnerID
        {
            get => visitingProjectOwnerID;
            set => visitingProjectOwnerID = value;
        }
        public Guid VisitingSkillProviderID
        {
            get => visitingSkillProviderID;
            set => visitingSkillProviderID = value;
        }
        public Guid VisitingProjectID
        {
            get => visitingProjectID;
            set => visitingProjectID = value;
        }

        public Guid VisitingReceiverID
        {
            get => visitingReceiverID;
            set => visitingReceiverID = value;
        }

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
        public UserRole? GetCurrentUserRole()
        {
            return currentUserRole;
        }
        public string? GetCurrentUserName()
        {
            return currentUserName;
        }
        public CebuLocation? GetCurrentUserLocation()
        {
            return currentUserLocation;
        }

        public void ClearSession()
        {
            currentUserId = Guid.Empty;
            currentUserRole = null;
        }

        public bool IsLoggedIn()
        {
            return currentUserId != Guid.Empty;
        }

        

        public int VisitingSkillEducationID
        {
            get => visitingSkillEducationID;
            set => visitingSkillEducationID = value;
        }

        private int visitingSkillEducationID = 0;

    }
}
