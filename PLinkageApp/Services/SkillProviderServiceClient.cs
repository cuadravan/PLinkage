using System.Net.Http; // Add this using for HttpClient
using System.Threading.Tasks;
using PLinkageApp.Services.Http;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using PLinkageApp.Interfaces;

namespace PLinkageApp.Services
{
    public class SkillProviderServiceClient : BaseApiClient, ISkillProviderServiceClient
    {
        public SkillProviderServiceClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<ApiResponse<IEnumerable<SkillProviderCardDto>>> GetFilteredSkillProvidersAsync(string proximity, CebuLocation? location, string status, bool? isEmployed)
        {
            string url = "api/skillprovider";

            var queryParams = new List<string>
        {
            $"proximity={proximity}",
            $"status={status}"
        };

            // 3. Add location only if it has a value.
            if (location.HasValue)
            {
                queryParams.Add($"location={(int)location.Value}");
            }

            if (isEmployed.HasValue)
            {
                queryParams.Add($"isEmployed={isEmployed}");
            }

            string finalUrl = $"{url}?{string.Join("&", queryParams)}";

            return await GetAsync<IEnumerable<SkillProviderCardDto>>(finalUrl);
        }

        public async Task<ApiResponse<SkillProviderDto>> GetSpecificAsync(Guid skillProviderId)
        {
            return await GetAsync<SkillProviderDto>($"api/skillprovider/{skillProviderId}");
        }

        public async Task<ApiResponse<bool>> UpdateSkillProviderAsync(Guid skillProviderId, UserProfileUpdateDto skillProviderUpdateDto)
        {
            return await PatchAsync<UserProfileUpdateDto, bool>($"api/skillprovider/{skillProviderId}", skillProviderUpdateDto);
        }
        public async Task<ApiResponse<bool>> AddEducationAsync(Guid skillProviderId, EducationDto educationToAdd)
        {
            return await PostAsync<EducationDto, bool>($"api/skillprovider/{skillProviderId}/educations", educationToAdd);
        }
        public async Task<ApiResponse<bool>> UpdateEducationAsync(Guid skillProviderId, int indexToUpdate, EducationDto educationToUpdate)
        {
            return await PutAsync<EducationDto, bool>($"api/skillprovider/{skillProviderId}/educations/{indexToUpdate}", educationToUpdate);
        }
        public async Task<ApiResponse<bool>> DeleteEducationAsync(Guid skillProviderId, int indexToDelete)
        {
            return await DeleteAsync<bool>($"api/skillprovider/{skillProviderId}/educations/{indexToDelete}");
        }

        public async Task<ApiResponse<bool>> AddSkillAsync(Guid skillProviderId, SkillDto skillToAdd)
        {
            return await PostAsync<SkillDto, bool>($"api/skillprovider/{skillProviderId}/skills", skillToAdd);
        }
        public async Task<ApiResponse<bool>> UpdateSkillAsync(Guid skillProviderId, int indexToUpdate, SkillDto skillToUpdate)
        {
            return await PutAsync<SkillDto, bool>($"api/skillprovider/{skillProviderId}/skills/{indexToUpdate}", skillToUpdate);
        }
        public async Task<ApiResponse<bool>> DeleteSkillAsync(Guid skillProviderId, int indexToDelete)
        {
            return await DeleteAsync<bool>($"api/skillprovider/{skillProviderId}/skills/{indexToDelete}");
        }
    }
}