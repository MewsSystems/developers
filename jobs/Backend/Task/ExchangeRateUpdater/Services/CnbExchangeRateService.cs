using System;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateService : IExchangeRateService
    {
        private static readonly string ExchangeRateUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private readonly ILogger _logger;

        public CnbExchangeRateService()
        {
            _logger = Log.ForContext<CnbExchangeRateService>();
        }

        public async Task<string> FetchExchangeRateDataAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    _logger.Information("Fetching exchange rate data from {ExchangeRateUrl}", ExchangeRateUrl);
                    HttpResponseMessage response = await client.GetAsync(ExchangeRateUrl);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    _logger.Error(e, "Request error while fetching exchange rate data from {ExchangeRateUrl}", ExchangeRateUrl);
                    throw new Exception("Error fetching exchange rate data from CNB", e);
                }
            }
        }
    }
}
