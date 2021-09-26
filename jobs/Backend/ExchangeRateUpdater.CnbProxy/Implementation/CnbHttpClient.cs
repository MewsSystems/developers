using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using ExchangeRateUpdater.Utilities.Extensions;
using ExchangeRateUpdater.CnbProxy.Configuration;
using ExchangeRateUpdater.Utilities.Logging;

namespace ExchangeRateUpdater.CnbProxy.Implementation
{
    class CnbHttpClient : ICnbHttpClient
    {
        private readonly ICnbConfiguration _cnbConfiguration;
        private readonly HttpClient _httpClient;
        private readonly IAppLogger _logger;

        public CnbHttpClient(ICnbConfiguration cnbConfiguration, IAppLogger logger)
        {
            Guard.ArgumentNotNull(nameof(cnbConfiguration), cnbConfiguration);

            _cnbConfiguration = cnbConfiguration;
            _logger = logger;

            _httpClient = new HttpClient();
        }

        public async Task<string> GetXmlExchangeRatesAsync()
        {
            // TODO: Depending on use cases and desired resilency level
            // I would consider adding other polices, i.e. caching

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10) }, (ex, timeSpan, retryCount, context) =>
                {
                    _logger.Error($"Error getting rates - try retry (count: {retryCount}, timeSpan: {timeSpan}, error: {ex.Message})");
                })
                .ExecuteAsync(() => _httpClient.GetAsync(_cnbConfiguration.UrlToXmlExchangeRates));
             
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
