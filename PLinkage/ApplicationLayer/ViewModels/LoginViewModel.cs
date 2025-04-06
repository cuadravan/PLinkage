using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLinkage.ApplicationLayer.UseCases;

namespace PLinkage.ApplicationLayer.ViewModels
{
    public class LoginViewModel
    {
        private readonly LoginUseCase _loginUseCase;
        // In here add the properties for username/email and password
        // Add command for Login to then call the Login function placed here

        public LoginViewModel(LoginUseCase loginUseCase)
        {
            _loginUseCase = loginUseCase;
        }

        public async Task<bool> Login(string email, string password)
        {
            var user = await _loginUseCase.ExecuteAsync(email, password);
            return user != null;
        }

        public string GetCurrentUserRole()
        {
            return _loginUseCase.GetCurrentUserRole(); // from session
        }
    }

}
