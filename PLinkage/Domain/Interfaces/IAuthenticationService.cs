using PLinkage.Domain.Models;

namespace PLinkage.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User?> LoginAsync(string email, string password);
        Task LogoutAsync();
        bool IsUserLoggedIn();
    }

}
