using System.Net.Http; // Add this using for HttpClient
using System.Threading.Tasks;
using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.Services
{
    public interface IAccountServiceClient
    {
        // 1. Remove the nullable '?' from the return types
        Task<ApiResponse<LoginResultDto>> LoginAsync(LoginRequestDto request);
        Task<ApiResponse<string>> RegisterAsync(RegisterUserDto request);
    }

    public class AccountServiceClient : BaseApiClient, IAccountServiceClient
    {
        // The HttpClient using is needed here if not inherited/available
        public AccountServiceClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<ApiResponse<LoginResultDto>> LoginAsync(LoginRequestDto request)
        {
            // 2. The second generic argument is the type of the 'Data' field
            //    in the ApiResponse, not the ApiResponse itself.
            return await PostAsync<LoginRequestDto, LoginResultDto>("api/account/login", request);
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterUserDto request)
        {
            // 2. The second generic argument is the type of the 'Data' field (string)
            return await PostAsync<RegisterUserDto, string>("api/account/register", request);
        }
    }
}