namespace ExchangeRateFinder.Infrastructure.Services
{
    public interface IHttpClientService
    {
        Task<string> GetDataFromUrl(string url);
    }
    public class HttpClientService : IHttpClientService
    {
        public async Task<string> GetDataFromUrl(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
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
}
