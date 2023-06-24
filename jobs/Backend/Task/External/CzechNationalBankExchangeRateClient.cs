using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateProvider.External
{
    public class CzechNationalBankExchangeRateClient : ICzechNationalBankExchangeRateClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public CzechNationalBankExchangeRateClient()
        {
            _httpClient = new HttpClient();
            _baseUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market";
        }

        public async Task<string> GetExchangeRateFixingAsync()
        {
            var url =
                $"{_baseUrl}/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetFixRatesOfOtherCurrenciesAsync()
        {
            var url =
                $"{_baseUrl}/fx-rates-of-other-currencies/fx-rates-of-other-currencies/";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
