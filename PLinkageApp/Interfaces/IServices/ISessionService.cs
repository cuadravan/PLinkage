using System;
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
        void UpdateCurrentUser(CebuLocation? cebuLocation, string userName);
        Guid GetCurrentUserId();
        UserRole? GetCurrentUserRole();
        CebuLocation? GetCurrentUserLocation();
        string? GetCurrentUserName();
        void ClearSession();
        bool IsLoggedIn();
    }
}
