using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.Interfaces
{
    public interface IOfferApplicationServiceClient
    {
        Task<ApiResponse<Guid>> CreateApplicationOffer(OfferApplicationCreationDto offerApplicationCreationDto);
        Task<ApiResponse<OfferApplicationDto>> GetSpecificOfferApplication(Guid offerApplicationId);
        Task<ApiResponse<OfferApplicationPageDto>> GetOfferApplicationOfUser(Guid userId, UserRole userRole);
        Task<ApiResponse<bool>> ProcessOfferApplication(OfferApplicationProcessDto offerApplicationProcessDto);
    }
}
