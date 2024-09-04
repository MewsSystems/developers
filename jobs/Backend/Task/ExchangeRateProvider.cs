using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return Enumerable.Empty<ExchangeRate>();
        }
        
        private async Task<DailyExchangeRateResponse?> GetDailyExchangeRates()
        {
            DailyExchangeRateResponse? result = null;
            
            try
            {
                using var client = new HttpClient();

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://api.cnb.cz/cnbapi/exrates/daily")
                };

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<DailyExchangeRateResponse>(responseBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve daily exchange rates: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while retrieving daily exchange rates: {e}");
            }

            return result;
        }
    }
}
