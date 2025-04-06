using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLinkage.Domain.Interfaces;
using PLinkage.Domain.Models;

namespace PLinkage.ApplicationLayer.UseCases
{
    public class LoginUseCase
    {
        private readonly IAuthenticationService _authService;
        private readonly ISessionService _sessionService;

        public LoginUseCase(IAuthenticationService authService, ISessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;
        }

        public async Task<User?> ExecuteAsync(string email, string password)
        {
            return await _authService.LoginAsync(email, password);
        }

        public string? GetCurrentUserRole()
        {
            return _sessionService.GetCurrentUser()?.UserRole;
        }
    }

}
