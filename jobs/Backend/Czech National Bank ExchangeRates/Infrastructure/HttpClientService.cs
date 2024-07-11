using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;

namespace Czech_National_Bank_ExchangeRates.Infrastructure
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<T> GetAsync<T>(string uri, string authToken, string apiKey)

        {
            try
            {
                HttpClient httpClient = CreateHttpClient(authToken, apiKey);
                string jsonResult = string.Empty;

                var responseMessage = await httpClient.GetAsync(uri);

                if (responseMessage.IsSuccessStatusCode)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                }

                else if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
                    responseMessage.StatusCode == HttpStatusCode.Unauthorized ||
                    responseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;

                }
                else
                {
                    return default;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name + " : " + e.Message}");
                return default;
            }
        }

        private HttpClient CreateHttpClient(string authToken = "", string apiKey = "", string apiKeyHeaderType = "")
        {
            HttpClientHandler clientHandler = new HttpClientHandler();

            var httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(authToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }
            if (!string.IsNullOrEmpty(apiKeyHeaderType))
            {
                if (!string.IsNullOrEmpty(apiKey))
                {
                    httpClient.DefaultRequestHeaders.Add(apiKeyHeaderType, apiKey);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(apiKey))
                {
                    httpClient.DefaultRequestHeaders.Add("ApiKey", apiKey);
                }
            }

            return httpClient;
        }
    }
}
