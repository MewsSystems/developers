using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Tests.IntegrationTests
{
    public class DummyHttpClient : ICustomHttpClient
    {
        public Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
        {
            var result = new CNBRates()
            { 
                Rates = new List<CNBRate> 
                { 
                    new CNBRate 
                    { 
                        Amount = 1,
                        CurrencyCode = "USD",
                        Rate = 12
                    } 
                }
            };

            return Task.FromResult((T)(object)result);
        }

    }
}
