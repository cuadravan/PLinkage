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


        private Guid visitingProjectOwnerID = Guid.Empty;
        private Guid visitingSkillProviderID = Guid.Empty;
        private Guid visitingProjectID = Guid.Empty;
        private Guid visitingReceiverID = Guid.Empty;
        private int visitingSkillEducationID = 0;

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
        public int VisitingSkillEducationID
        {
            get => visitingSkillEducationID;
            set => visitingSkillEducationID = value;
        }

    }
}
