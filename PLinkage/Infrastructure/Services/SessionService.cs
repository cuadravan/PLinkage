using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLinkage.Domain.Interfaces;
using PLinkage.Domain.Models;

namespace PLinkage.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private User? _currentUser;

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
            // Optional: Store in Preferences if you want persistence across restarts
            // Preferences.Set("UserId", user.UserId.ToString());
        }

        public User? GetCurrentUser() => _currentUser;

        public void ClearSession()
        {
            _currentUser = null;
            // Preferences.Remove("UserId");
        }
    }

}
