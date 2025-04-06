using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLinkage.Domain.Interfaces;
using PLinkage.Domain.Models;

namespace PLinkage.Infrastructure.Services
{
    public class JsonAuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepo;
        private readonly ISessionService _sessionService;

        public JsonAuthenticationService(IUserRepository userRepo, ISessionService sessionService)
        {
            _userRepo = userRepo;
            _sessionService = sessionService;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var users = await _userRepo.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.UserEmail == email && u.UserPassword == password);

            if (user != null)
            {
                _sessionService.SetCurrentUser(user);
            }

            return user;
        }

        public Task LogoutAsync()
        {
            _sessionService.ClearSession();
            return Task.CompletedTask;
        }

        public bool IsUserLoggedIn() => _sessionService.GetCurrentUser() != null;
    }

}
