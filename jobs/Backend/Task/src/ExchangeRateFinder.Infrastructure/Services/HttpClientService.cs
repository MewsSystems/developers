namespace ExchangeRateFinder.Infrastructure.Services
{
    public interface IHttpClientService
    {
        Task<string> GetDataAsync(string url);
    }

    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetDataAsync(string url)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
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
