using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.ClientFactories
{
    public class CnbApiClientFactory : ICnbApiClientFactory
    {
        private readonly CnbApiClientConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public CnbApiClientFactory(CnbApiClientConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _clientFactory = clientFactory; 
        }

        public ICnbApiClient CreateClient()
        {
            var httpClient = _clientFactory.CreateClient();
            return new CnbApiClient(_configuration, httpClient);
        }
    }
}
