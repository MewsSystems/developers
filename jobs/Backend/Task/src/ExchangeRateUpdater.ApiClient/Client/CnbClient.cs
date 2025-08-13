using ExchangeRateUpdater.ApiClient.Client.ExchangeDaily;
using ExchangeRateUpdater.ApiClient.Common;
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
            _logger.LogInformation("Doing request '{request}' with '{date}' and '{language}'",nameof(CnbClient.GetExchangesDaily)
                ,date,language);

            var query = CnbConstants.ExchangeRatesDaily(date, language.ToString());
            var request = new HttpRequestMessage(HttpMethod.Get, query);
            return await Send<ExchangeDailyCommand, ExchangeDailyResponse, ErrorDailyResponse>(request);
        }

    }
}
