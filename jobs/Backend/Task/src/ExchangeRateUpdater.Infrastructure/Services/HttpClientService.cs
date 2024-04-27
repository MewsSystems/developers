namespace ExchangeRateFinder.Infrastructure.Services
{
    public interface IHttpClientService
    {
        Task<string> GetDataAsync(string url);
    }
    public class HttpClientService : IHttpClientService
    {
        private HttpClient _httpClient;
        public HttpClientService(IHttpClientFactory _httpClientFactory)
        {
            _httpClient = _httpClientFactory.CreateClient();
        }
        public async Task<string> GetDataAsync(string url)
        {
           
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Failed to retrieve data. Status code: {response.StatusCode}");
            }
        }
    }
}
