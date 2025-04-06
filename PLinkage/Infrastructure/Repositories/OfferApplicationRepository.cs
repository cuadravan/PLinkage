using Newtonsoft.Json;
using PLinkage.Domain.Interfaces;
using PLinkage.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace PLinkage.Infrastructure.Repositories
{
    public class OfferApplicationRepository : IOfferApplicationRepository
    {
        private readonly string _offerApplicationFilePath;

        public OfferApplicationRepository()
        {
            // Get project base path
            string _projectPath = AppDomain.CurrentDomain.BaseDirectory;

            // Get the full path to the JSON folder
            string _jsonFolderPath = Path.GetFullPath(Path.Combine(_projectPath, @"..\..\..\..\..\json"));

            // Combine folder path with the file name for offer applications
            _offerApplicationFilePath = Path.Combine(_jsonFolderPath, "OfferApplications.txt");
        }

        public async Task<List<OfferApplication>> GetAllOfferApplicationsAsync()
        {
            if (File.Exists(_offerApplicationFilePath))
            {
                var json = await File.ReadAllTextAsync(_offerApplicationFilePath);
                return JsonConvert.DeserializeObject<List<OfferApplication>>(json) ?? new List<OfferApplication>();
            }
            return new List<OfferApplication>();
        }

        public async Task<OfferApplication?> GetOfferApplicationByIdAsync(Guid offerApplicationId)
        {
            var offerApplications = await GetAllOfferApplicationsAsync();
            return offerApplications.Find(o => o.OfferApplicationId == offerApplicationId);
        }

        public async Task AddOfferApplicationAsync(OfferApplication offerApplication)
        {
            var offerApplications = await GetAllOfferApplicationsAsync();
            offerApplications.Add(offerApplication);
            await SaveToFileAsync(offerApplications);
        }

        public async Task UpdateOfferApplicationAsync(OfferApplication offerApplication)
        {
            var offerApplications = await GetAllOfferApplicationsAsync();
            var existingOfferApplication = offerApplications.Find(o => o.OfferApplicationId == offerApplication.OfferApplicationId);
            if (existingOfferApplication != null)
            {
                offerApplications.Remove(existingOfferApplication);
                offerApplications.Add(offerApplication);
                await SaveToFileAsync(offerApplications);
            }
        }

        public async Task DeleteOfferApplicationAsync(Guid offerApplicationId)
        {
            var offerApplications = await GetAllOfferApplicationsAsync();
            var offerApplication = offerApplications.Find(o => o.OfferApplicationId == offerApplicationId);
            if (offerApplication != null)
            {
                offerApplications.Remove(offerApplication);
                await SaveToFileAsync(offerApplications);
            }
        }

        private async Task SaveToFileAsync(List<OfferApplication> offerApplications)
        {
            var json = JsonConvert.SerializeObject(offerApplications, Newtonsoft.Json.Formatting.Indented);
            await File.WriteAllTextAsync(_offerApplicationFilePath, json);
        }
    }
}
