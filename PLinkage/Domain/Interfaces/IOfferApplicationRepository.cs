using PLinkage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLinkage.Domain.Interfaces
{
    public interface IOfferApplicationRepository
    {
        Task<List<OfferApplication>> GetAllOfferApplicationsAsync();
        Task<OfferApplication?> GetOfferApplicationByIdAsync(Guid offerApplicationId);
        Task AddOfferApplicationAsync(OfferApplication offerApplication);
        Task UpdateOfferApplicationAsync(OfferApplication offerApplication);
        Task DeleteOfferApplicationAsync(Guid offerApplicationId);
    }
}
