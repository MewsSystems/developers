using System;
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
    public async Task RetrievesExchangeRateWithCzkAsTargetCurrencyForRequestedCurrencies(string currencyCode, int amount, decimal rate)
    {
        // Given
        var cnbRate = new CnbRateResult { CurrencyCode = currencyCode, Amount = amount, Rate = rate };
        var cnbExchangeRatesProvider = new CnbExchangeRatesProvider(StubCnbClient.WithRates(cnbRate));

        var requestedCurrency = new Currency(currencyCode);

        // When
        var exchangeRates = await cnbExchangeRatesProvider.RetrieveExchangeRatesAsync(
            new[] { requestedCurrency },
            CancellationToken.None);

        // Then
        var exchangeRate = Assert.Single(exchangeRates);
        Assert.Equal(requestedCurrency, exchangeRate.SourceCurrency);
        Assert.Equal(Currency.Czk, exchangeRate.TargetCurrency);
        Assert.Equal(rate / amount, exchangeRate.Value);
    }

    [Fact]
    public async Task DoesNotProduceExchangeRateForNotRequestedCurrencies()
    {
        // Given
        var dummyCnbRate = new CnbRateResult { CurrencyCode = "XYZ", Amount = 1, Rate = 1 };

        var cnbExchangeRatesProvider = new CnbExchangeRatesProvider(StubCnbClient.WithRates(dummyCnbRate));

        // When
        var exchangeRates = await cnbExchangeRatesProvider.RetrieveExchangeRatesAsync(Array.Empty<Currency>(), CancellationToken.None);

        // Then
        Assert.Empty(exchangeRates);
    }

    [Fact]
    public async Task DoesNotProduceExchangeRateForRequestedCurrenciesWithoutSourceRate()
    {
        // Given
        var requestedCurrency = new Currency("XYZ");
        var cnbExchangeRatesProvider = new CnbExchangeRatesProvider(StubCnbClient.WithoutRates());

        // When
        var exchangeRates = await cnbExchangeRatesProvider.RetrieveExchangeRatesAsync(new[] { requestedCurrency }, CancellationToken.None);

        // Then
        Assert.Empty(exchangeRates);
    }

    [Fact]
    public async Task DoesNotProduceExchangeRateForSourceRateWithInvalidProperties()
    {
        // Given
        var invalidCnbRateWithZeroAmount = new CnbRateResult { CurrencyCode = "XYZ", Amount = 0, Rate = 1 };

        var cnbExchangeRatesProvider = new CnbExchangeRatesProvider(StubCnbClient.WithRates(invalidCnbRateWithZeroAmount));

        // When
        var exchangeRates = await cnbExchangeRatesProvider.RetrieveExchangeRatesAsync(
            new[] { new Currency(invalidCnbRateWithZeroAmount.CurrencyCode) },
            CancellationToken.None);

        // Then
        Assert.Empty(exchangeRates);
    }

    private class StubCnbClient : ICnbClient
    {
        private readonly CnbRatesResult result;

        private StubCnbClient(CnbRatesResult result)
        {
            this.result = result;
        }

        public static StubCnbClient WithoutRates()
        {
            return WithRates();
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