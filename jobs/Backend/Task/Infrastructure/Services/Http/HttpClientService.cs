using Domain.Abstractions;
using System.Text.Json;

namespace Infrastructure.Services.Http;

internal class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T> GetJsonAsync<T>(string uri)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<T>(responseString, options);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Error parsing the response JSON.", ex);
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request-specific errors
            throw new ApplicationException("Error during HTTP request.", ex);
        }
        catch (TaskCanceledException ex)
        {
            // Handle timeout errors
            throw new ApplicationException("Request timed out.", ex);
        }
        catch (Exception ex)
        {
            // Catch all other exceptions
            throw new ApplicationException("An unexpected error occurred.", ex);
        }
    }
}
