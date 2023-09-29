using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.CnbRates;
using ExchangeRateUpdater.Contracts;
using Xunit;

namespace ExchangeRateUpdater.Tests.CnbRates;

public class CnbExchangeRatesProviderTests
{
    [Theory]
    [InlineData("EUR", 1, 24.465)]
    [InlineData("JPY", 100, 15.557)]
    public async Task RetrievesExchangeRateWithCzkAsTargetCurrencyForRequestedCurrenciesWhileIgnoringRequestedCurrenciesWithoutSourceRate(
        string currencyCode,
        int amount,
        decimal rate)
    {
        // Given
        var dummyCnbRate = new CnbRateResult { CurrencyCode = "XYZ", Amount = 0, Rate = 0 };
        var requestedCnbRate = new CnbRateResult { CurrencyCode = currencyCode, Amount = amount, Rate = rate };
        var cnbExchangeRatesProvider = new CnbExchangeRatesProvider(StubCnbClient.WithRates(dummyCnbRate, requestedCnbRate));

        var requestedCurrency = new Currency(currencyCode);
        var currencyWithoutSourceRate = new Currency("ABC");

        // When
        var exchangeRates = await cnbExchangeRatesProvider.RetrieveExchangeRatesAsync(
            new[] { requestedCurrency, currencyWithoutSourceRate },
            CancellationToken.None);

        // Then
        var exchangeRate = Assert.Single(exchangeRates);

        Assert.Equal(new Currency(currencyCode), exchangeRate.SourceCurrency);
        Assert.Equal(Currency.Czk, exchangeRate.TargetCurrency);
        Assert.Equal(rate / amount, exchangeRate.Value);
    }

    private class StubCnbClient : ICnbClient
    {
        private readonly CnbRatesResult result;

        private StubCnbClient(CnbRatesResult result)
        {
            this.result = result;
        }

        public static StubCnbClient WithRates(params CnbRateResult[] cnbRates)
        {
            return new StubCnbClient(new() { Rates = cnbRates });
        }

        public Task<CnbRatesResult> RetrieveExchangeRatesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(result);
        }
    }
}