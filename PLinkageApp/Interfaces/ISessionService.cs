using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLinkageShared.Enums;

namespace PLinkageApp.Interfaces
{
    public interface ISessionService
    {
        void SetCurrentUser(IUser user);
        IUser? GetCurrentUser();
        void ClearSession();
        bool IsLoggedIn();
        UserRole? GetCurrentUserType();
        Guid VisitingProjectOwnerID { get; set; }
        Guid VisitingSkillProviderID { get; set; }
        Guid VisitingProjectID { get; set; }
        Guid VisitingReceiverID { get; set; }
        int VisitingSkillEducationID { get; set; }
    }
}
