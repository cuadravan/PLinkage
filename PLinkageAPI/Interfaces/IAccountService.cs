using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageAPI.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<LoginResultDto>> AuthenticateUserAsync(string email, string password);
        Task<ApiResponse<string>> RegisterUserAsync(RegisterUserDto registerUserDto);

        Task<ApiResponse<string>> HashAllPasswordsAsync();
    }
}
