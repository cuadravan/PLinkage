using PLinkageApp.Interfaces;
using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.Services
{
    public class OfferApplicationServiceClient: BaseApiClient, IOfferApplicationServiceClient
    {
        public OfferApplicationServiceClient(HttpClient httpClient) : base(httpClient)
        {
        }
        public async Task<ApiResponse<Guid>> CreateApplicationOffer(OfferApplicationCreationDto offerApplicationCreationDto)
        {
            return await PostAsync<OfferApplicationCreationDto, Guid>("api/offerapplication", offerApplicationCreationDto);
        }
        public async Task<ApiResponse<OfferApplicationDto>> GetSpecificOfferApplication(Guid offerApplicationId)
        {
            return await GetAsync<OfferApplicationDto>($"api/offerapplication/{offerApplicationId}");
        }
        public async Task<ApiResponse<OfferApplicationPageDto>> GetOfferApplicationOfUser(Guid userId, UserRole? userRole)
        {
            string url = "api/offerapplication";

            var queryParams = new List<string>
            {
                $"userid={userId}",
                $"userrole={(int)userRole}"
            };

            string finalUrl = $"{url}?{string.Join("&", queryParams)}";

            return await GetAsync<OfferApplicationPageDto>(finalUrl);
        }
        public async Task<ApiResponse<bool>> ProcessOfferApplication(OfferApplicationProcessDto offerApplicationProcessDto)
        {
            return await PostAsync<OfferApplicationProcessDto, bool>("api/offerapplication/process", offerApplicationProcessDto);
        }
    }
}
