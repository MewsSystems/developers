using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateService : IExchangeRateService
    {
        private static readonly string ExchangeRateUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=08.07.2024";

        public async Task<string> FetchExchangeRateDataAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(ExchangeRateUrl);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    // Log the exception or handle it as needed
                    Console.WriteLine($"Request error: {e.Message}");
                    throw new Exception("Error fetching exchange rate data from CNB", e);
                }
            }
        }
    }
}
