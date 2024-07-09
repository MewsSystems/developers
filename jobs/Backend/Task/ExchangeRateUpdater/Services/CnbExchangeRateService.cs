using System;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateService : IExchangeRateService
    {
        private readonly string ExchangeRateUrl;
        public HttpClient HttpClient { get; set; }
        public ILogger Logger { get; set; }

        public CnbExchangeRateService()
        {
            Logger = Log.ForContext<CnbExchangeRateService>();
            HttpClient = new HttpClient();
            ExchangeRateUrl = EnvironmentHelper.GetEnvironmentVariable("CNB_EXCHANGE_RATE_URL");
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
