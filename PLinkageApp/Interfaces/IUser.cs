using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.Interfaces
{
    public interface IUser
    {
        Guid UserId { get; }
        string UserFirstName { get; }
        string UserLastName { get; }
        UserRole UserRole { get; }
        CebuLocation? UserLocation { get; }
    }
}
