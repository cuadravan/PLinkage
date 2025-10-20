using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageAPI.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<LoginResultDto>> AuthenticateUserAsync(string email, string password);
        Task<UserRole?> DetermineUserRoleAsync(Guid userId);
        Task<ApiResponse<Guid>> RegisterUserAsync(RegisterUserDto registerUserDto);

        Task<ApiResponse<bool>> CheckEmailUniquenessAsync(string email);
        Task<ApiResponse<string>> ActivateDeactivateUserAsync(Guid userId);

        //Task<ApiResponse<string>> HashAllPasswordsAsync();
    }
}
