using ExchangeRateUpdater.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Fetch
{
    public class ExchangeRatesTxtFetcher : IExchangeRatesTxtFetcher
    {
        private readonly Config configuration;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ExchangeRatesTxtFetcher> logger;

        public ExchangeRatesTxtFetcher(Config configuration, IHttpClientFactory httpClientFactory, ILogger<ExchangeRatesTxtFetcher> logger)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> FetchExchangeRates()
        {
            var httpClient = httpClientFactory.CreateClient();
            try
            {
                return await httpClient.GetStringAsync(configuration.ExchangeRatesUrl);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Couldn't fetch rate txt: {message}", e.Message);
                throw;
            }
        }
    }
}
