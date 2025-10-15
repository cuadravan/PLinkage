using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using PLinkageShared.ApiResponse;

namespace PLinkageApp.Services.Http
{
    // The type parameter TResult is used for the expected data type within ApiResponse<TResult>
    public abstract class BaseApiClient
    {
        protected readonly HttpClient _httpClient;

        protected BaseApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // --- HTTP GET Operation ---
        protected async Task<ApiResponse<TResult>> GetAsync<TResult>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                return await HandleResponse<TResult>(response);
            }
            catch (Exception ex)
            {
                // Handle lower-level network or serialization errors
                return ApiResponse<TResult>.Fail($"An error occurred during the GET request: {ex.Message}");
            }
        }

        // --- HTTP POST Operation ---
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

        // --- HTTP PUT Operation ---
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

        // --- HTTP DELETE Operation (Returns a success/fail for the operation, TResult is usually a placeholder or 'object') ---
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
            // 1. Read the raw content once.
            var content = await response.Content.ReadAsStringAsync();

            // 2. Attempt to parse the content directly into your custom ApiResponse<TResult>
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

            // -------------------------------------------------------------------
            // --- HTTP SUCCESS (200-299) ---
            // -------------------------------------------------------------------
            if (response.IsSuccessStatusCode)
            {
                // Use the parsed custom ApiResponse if available (standard success path)
                if (customApiResponse != null)
                {
                    return customApiResponse;
                }

                // Fallback for 204 No Content or unparsable success response
                return ApiResponse<TResult>.Ok(default!, "Request successful with no parsable content.");
            }

            // -------------------------------------------------------------------
            // --- HTTP FAILURE (400-599) ---
            // -------------------------------------------------------------------
            else
            {
                // Case A: The content was successfully parsed as your custom ApiResponse. (PRIORITY)
                // This handles errors where the server returns a 4xx/5xx status code but includes a clean
                // message within your ApiResponse structure (e.g., "Account does not exist.").
                if (customApiResponse != null)
                {
                    // We trust the message from the structured ApiResponse, regardless of the Success property
                    // (since the HTTP status already indicates failure).
                    return ApiResponse<TResult>.Fail(customApiResponse.Message);
                }

                // Case B: Final Fallback for unparsable content or simple errors (e.g., raw text error, unhandled exception).
                string message = $"API request failed with status code {(int)response.StatusCode}.";

                if (!string.IsNullOrEmpty(content))
                {
                    // Append the raw content as details
                    message += $" Details: {content.Trim()}";
                }

                return ApiResponse<TResult>.Fail(message);
            }
        }
    }
}