using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using PLinkageShared.ApiResponse;

namespace PLinkageApp.Services.Http
{
    public abstract class BaseApiClient
    {
        protected readonly HttpClient _httpClient;

        protected BaseApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<ApiResponse<TResult>> GetAsync<TResult>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                return await HandleResponse<TResult>(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<TResult>.Fail($"An error occurred during the GET request: {ex.Message}");
            }
        }

        protected async Task<ApiResponse<TResult>> PostAsync<TRequest, TResult>(string url, TRequest payload)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, payload);
                return await HandleResponse<TResult>(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<TResult>.Fail($"An error occurred during the POST request: {ex.Message}");
            }
        }

        protected async Task<ApiResponse<TResult>> PutAsync<TRequest, TResult>(string url, TRequest payload)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(url, payload);
                return await HandleResponse<TResult>(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<TResult>.Fail($"An error occurred during the PUT request: {ex.Message}");
            }
        }

        protected async Task<ApiResponse<TResult>> DeleteAsync<TResult>(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                return await HandleResponse<TResult>(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<TResult>.Fail($"An error occurred during the DELETE request: {ex.Message}");
            }
        }

        private static async Task<ApiResponse<TResult>> HandleResponse<TResult>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            ApiResponse<TResult>? customApiResponse = null;
            if (!string.IsNullOrEmpty(content))
            {
                try
                {
                    customApiResponse = JsonSerializer.Deserialize<ApiResponse<TResult>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch (JsonException)
                {
                    // Ignore failure; the content wasn't a valid ApiResponse.
                }
            }

            if (response.IsSuccessStatusCode)
            {
                if (customApiResponse != null)
                {
                    return customApiResponse;
                }

                return ApiResponse<TResult>.Ok(default!, "Request successful with no parsable content.");
            }

            else
            {
                if (customApiResponse != null)
                {
                    return ApiResponse<TResult>.Fail(customApiResponse.Message);
                }

                string message = $"API request failed with status code {(int)response.StatusCode}.";

                if (!string.IsNullOrEmpty(content))
                {
                    
                    message += $" Details: {content.Trim()}";
                }

                return ApiResponse<TResult>.Fail(message);
            }
        }
    }
}