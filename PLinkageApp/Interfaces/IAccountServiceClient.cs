using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.Interfaces
{
    public interface IAccountServiceClient
    {
        Task<ApiResponse<LoginResultDto>> LoginAsync(LoginRequestDto request);
        Task<ApiResponse<string>> RegisterAsync(RegisterUserDto request);
        Task<ApiResponse<bool>> CheckEmailUniquenessAsync(string email);
    }
}
