using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLinkage.Domain.Models;

namespace PLinkage.Domain.Interfaces
{
    public interface ISessionService
    {
        void SetCurrentUser(User user);
        User? GetCurrentUser();
        void ClearSession();
    }
}
