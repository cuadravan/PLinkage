﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageApp.Interfaces
{
    public interface ISessionService
    {
        void SetCurrentUser(LoginResultDto loginResultDto);
        Guid GetCurrentUserId();
        UserRole? GetCurrentUserRole();
        CebuLocation? GetCurrentUserLocation();
        string? GetCurrentUserName();
        void ClearSession();
        bool IsLoggedIn();
        
        Guid VisitingProjectOwnerID { get; set; }
        Guid VisitingSkillProviderID { get; set; }
        Guid VisitingProjectID { get; set; }
        Guid VisitingReceiverID { get; set; }
        int VisitingSkillEducationID { get; set; }
    }
}
