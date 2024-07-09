using System;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateService : IExchangeRateService
    {
        private static readonly string ExchangeRateUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        public HttpClient HttpClient { get; set; }
        public ILogger Logger { get; set; }

        public CnbExchangeRateService()
        {
            Logger = Log.ForContext<CnbExchangeRateService>();
            HttpClient = new HttpClient();
        }

        public async Task<string> FetchExchangeRateDataAsync()
        {
            try
            {
                Logger.Information("Fetching exchange rate data from {ExchangeRateUrl}", ExchangeRateUrl);
                HttpResponseMessage response = await HttpClient.GetAsync(ExchangeRateUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Logger.Error(e, "Request error while fetching exchange rate data from {ExchangeRateUrl}", ExchangeRateUrl);
                throw new Exception("Error fetching exchange rate data from CNB", e);
            }
        }
    }
}
