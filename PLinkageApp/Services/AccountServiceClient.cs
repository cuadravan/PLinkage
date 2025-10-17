﻿using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageApp.Interfaces;

namespace PLinkageApp.Services
{
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

        public async Task<ApiResponse<bool>> CheckEmailUniquenessAsync(string email)
        {
            return await PostAsync<string, bool>("api/account/checkemail", email);
        }
    }
}