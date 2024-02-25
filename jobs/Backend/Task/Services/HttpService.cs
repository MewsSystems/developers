using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpApiService
{
    public class HttpService
    {
        private HttpClient httpClient = null;

        public HttpService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<T> GetWithJsonMapping<T>(string requestUrl)
        {
            try
            {
                var httpGetResponse = await httpClient.GetAsync(requestUrl);

                if (httpGetResponse.IsSuccessStatusCode)
                {
                    Stream httpGetResponseStream = await httpGetResponse.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<T>(httpGetResponseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"GET Request resulted in an exception: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Deserializing GET Response resulted in an exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTPService encountered an unhandled exception when calling GetWithJsonMapping: {ex.Message}");
                throw;
            }

            return default;
        }
    }
}
