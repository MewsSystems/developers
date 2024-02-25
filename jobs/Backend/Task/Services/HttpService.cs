using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HttpApiService
{
    public class HttpService
    {
        private HttpClient httpClient = null;
        private ILogger _logger = null;

        public HttpService(ILogger logger)
        {
            httpClient = new HttpClient();
            _logger = logger;
            httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<T> GetWithJsonMapping<T>(string requestUrl)
        {
            try
            {
                var httpGetResponse = await httpClient.GetAsync(requestUrl);

                if (httpGetResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Exchange rate API call successfull");
                    Stream httpGetResponseStream = await httpGetResponse.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<T>(httpGetResponseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"GET Request resulted in an exception: {ex.Message}");
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Deserializing GET Response resulted in an exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"HTTPService encountered an unhandled exception when calling GetWithJsonMapping: {ex.Message}");
                throw;
            }

            return default;
        }
    }
}
