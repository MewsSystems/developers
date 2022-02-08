using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CzechNationalBankClient : ICzechNationalBankClient
    {
        private readonly HttpClient httpClient;

        public CzechNationalBankClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetExchangeRates()
        {
            var response = await httpClient.GetAsync("en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
