using System.Net.Http; // Add this using for HttpClient
using System.Threading.Tasks;
using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;

namespace PLinkageApp.Services
{
    public interface IAccountServiceClient
    {
        Task<ApiResponse<LoginResultDto>> LoginAsync(LoginRequestDto request);
        Task<ApiResponse<string>> RegisterAsync(RegisterUserDto request);
    }

    public class AccountServiceClient : BaseApiClient, IAccountServiceClient
    {
        public AccountServiceClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<ApiResponse<LoginResultDto>> LoginAsync(LoginRequestDto request)
        {
            return await PostAsync<LoginRequestDto, LoginResultDto>("api/account/login", request);
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterUserDto request)
        {
            return await PostAsync<RegisterUserDto, string>("api/account/register", request);
        }
    }
}