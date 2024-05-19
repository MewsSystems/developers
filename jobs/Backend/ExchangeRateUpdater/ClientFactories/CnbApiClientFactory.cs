using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.ClientFactories
{
    public class CnbApiClientFactory : ICnbApiClientFactory
    {
        private readonly CnbApiClientConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILoggerFactory _loggerFactory;

        public CnbApiClientFactory(CnbApiClientConfiguration configuration, IHttpClientFactory clientFactory, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _loggerFactory = loggerFactory;
        }

        public IExternalApiClient CreateApiClient()
        {
            return CreateCnbApiClient();
        }

        public ICnbApiClient CreateCnbApiClient()
        {
            var httpClient = _clientFactory.CreateClient();
            ILogger<CnbApiClient> logger = _loggerFactory.CreateLogger<CnbApiClient>();
            return new CnbApiClient(_configuration, httpClient, logger);
        }
    }
}
