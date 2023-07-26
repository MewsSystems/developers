using ExchangeRateUpdater.ApiClient.Client.ExchangeDaily;
using ExchangeRateUpdater.ApiClient.Infrastructure;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.ApiClient.Client
{
    public class CnbClient : BaseHttpClient, ICnbClient
    {
        public CnbClient(
            ILogger<CnbClient> logger,
            HttpClient httpClient) : base(logger, httpClient) { }


        public async Task<ExchangeDailyCommand> GetExchangesDaily(DateTime date, Language language)
        {
            var query = CnbConstants.ExchangeRatesDaily(date, language.ToString());
            var request = new HttpRequestMessage(HttpMethod.Get, query);
            return await Send<ExchangeDailyCommand, ExchangeResponse, ErrorResponse>(request);
        }
    }
}
