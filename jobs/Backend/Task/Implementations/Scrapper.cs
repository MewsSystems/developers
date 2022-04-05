using ExchangeRateUpdater.Service;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Implementations
{
    public class Scrapper : IScrapper
    {
        private HttpClient _client;

        public async Task<string> GetData(string Url)
        {
            this._client = new HttpClient();
            HttpResponseMessage response = await this._client.GetAsync(Url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
