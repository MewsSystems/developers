using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers.ProvidersStrategies;
using System.Net.Http;

namespace ExchangeRateUpdater.Providers
{
    public class ExchangeRateProviderStrategyFactory : IExchangeRateProviderStrategyFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ExchangeRateProviderStrategyFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public IExchangeRateProviderStrategy GetStrategy(ExchangeRateProviderCountry exchangeRateProviderCountry)
        {
            switch (exchangeRateProviderCountry)
            {
                default:
                    return new CzechNationalBankExchangeRateProvider(this.httpClientFactory.CreateClient("czech_httpClient"));
            }
        }
    }
}