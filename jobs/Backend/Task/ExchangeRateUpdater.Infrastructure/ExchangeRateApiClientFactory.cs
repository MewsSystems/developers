using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure
{
    public class ExchangeRateApiClientFactory(
        IHttpClientFactory httpClientFactory, 
        ILoggerFactory loggerFactory) : IExchangeRateApiClientFactory
    {
        public IExchangeRateApiClient CreateExchangeRateApiClient(string currencyCode)
        {
            return currencyCode switch
            {
                WellKnownCurrencyCodes.CZK => new CzechNationalBankExchangeRateApiClient(
                    httpClientFactory.CreateClient(HttpClientNames.CzechNationalBankApi), 
                    loggerFactory.CreateLogger<CzechNationalBankExchangeRateApiClient>()),

                _ => throw new NotImplementedException($"Exchange rate API client not implemented for currency {currencyCode}"),
            };
        }        
    }
}
