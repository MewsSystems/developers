using Xunit;
using ExchangeRateUpdater.Models;

namespace ExchangeRatesTests.Services
{
    public class ExchangeRateProviderTests
    {
    
    [Fact]
    public async Task GetExchangeRates_ShouldReturnExchangeRates_WhenCurrenciesAreProvided()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var cnbRates = new CnbRates
        {
            Rates = new List<CnbRate>
            {
                new CnbRate { CurrencyCode = "USD", Rate = 22.5m, Amount = 1 },
                new CnbRate { CurrencyCode = "EUR", Rate = 25.5m, Amount = 1 }
            }
        };
    }
    }    
}
