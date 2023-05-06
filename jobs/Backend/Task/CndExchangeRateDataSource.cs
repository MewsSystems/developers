using System;
using System.Net.Http;

using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateDataSource : IExchangeRateDataSource
    {
        private const string ExchangeRateUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date={0:dd.MM.yyyy}";
        private readonly HttpClient _httpClient;

        public CnbExchangeRateDataSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetExchangeRateDataAsync(DateTime date)
        {
            var url = string.Format(ExchangeRateUrl, date);
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve exchange rates: {response.StatusCode}");
            }
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
