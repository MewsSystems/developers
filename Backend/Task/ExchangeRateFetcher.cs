using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateFetcher
    {
        private IHttpClientFactory HttpClientFactory { get; set; }
        private const string RemoteServiceBaseUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=19.05.2021";

        public ExchangeRateFetcher(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }
        
        public async Task<string> Run()
        {
            var client = HttpClientFactory.CreateClient();
            return await client.GetStringAsync(RemoteServiceBaseUrl);
        }
    }
}