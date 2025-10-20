using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using PLinkageAPI.Entities;

namespace PLinkageAPI.Interfaces
{
    public interface IOfferApplicationService
    {
        Task<ApiResponse<Guid>> CreateApplicationOffer(OfferApplicationCreationDto offerApplicationCreationDto);
        Task<ApiResponse<OfferApplication>> GetSpecificOfferApplication(Guid offerApplicationId);
        Task<ApiResponse<OfferApplicationPageDto>> GetOfferApplicationOfUser(Guid userId, UserRole userRole);
        Task<ApiResponse<bool>> ProcessOfferApplication(OfferApplicationProcessDto offerApplicationProcessDto);
    }
}
