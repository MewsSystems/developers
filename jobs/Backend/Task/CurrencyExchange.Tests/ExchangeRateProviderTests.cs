using CurrencyExchange.Clients;
using CurrencyExchange.Model;
using CurrencyExchange.Services;

namespace CurrencyExchange.Tests;

public class ExchangeRateProviderTests
{
    [Fact]
    public async Task ReturnsOnlyRequestedCurrencyExchangeRates()
    {
        // Given
        var sourceExchangeRates = new List<DailyRate>
        {
            new()
            {
                CurrencyCode = "USD",
                Amount = 1,
                Rate = 22.50M,
                ValidFor = new DateTimeOffset(2024, 06, 07, 00, 00, 00, TimeSpan.Zero )
            },
            new()
            {
                CurrencyCode = "GBP",
                Amount = 1,
                Rate = 38.20M,
                ValidFor = new DateTimeOffset(2024, 06, 07, 00, 00, 00, TimeSpan.Zero )
            },
            new()
            {
                CurrencyCode = "AUD",
                Amount = 1,
                Rate = 15.03M,
                ValidFor = new DateTimeOffset(2024, 06, 07, 00, 00, 00, TimeSpan.Zero )
            }
        };

        var requestedCurrencies = new List<Currency>
        {
            TestCurrencies.GBP,
            TestCurrencies.USD
        };

        var expectedExchangeRates = new List<ExchangeRate>
        {
            new() { SourceCurrency = TestCurrencies.CZK, TargetCurrency = TestCurrencies.USD, Value = 22.50M, ValidFor = new DateTimeOffset(2024, 06, 07, 00, 00, 00, TimeSpan.Zero ) },
            new() { SourceCurrency = TestCurrencies.CZK, TargetCurrency = TestCurrencies.GBP, Value = 38.20M, ValidFor = new DateTimeOffset(2024, 06, 07, 00, 00, 00, TimeSpan.Zero ) },
        };

        var exchangeRateProvider = InstantiateExchangeRateProvider(sourceExchangeRates);

        // When
        var actualExchangeRates = await exchangeRateProvider.GetExchangeRates(requestedCurrencies, CancellationToken.None);

        // Then
        Assert.Equivalent(expectedExchangeRates, actualExchangeRates);
    }

    private static IExchangeRateProvider InstantiateExchangeRateProvider(List<DailyRate> sourceExchangeRates)
    {
        return new ExchangeRateProvider(new StubCurrencyExchangeClient(sourceExchangeRates));
    }

    [Fact]
    public async Task SkipsExchangeRatesForCurrenciesMissingInSource()
    {
        // Given
        var sourceExchangeRates = new List<DailyRate>
        {
            new()
            {
                CurrencyCode = "USD",
                Amount = 1,
                Rate = 22.50M,
                ValidFor = new DateTimeOffset(2024, 06, 07, 00, 00, 00, TimeSpan.Zero )
            }
        };

        var requestedCurrencies = new List<Currency>
        {
            TestCurrencies.GBP,
            TestCurrencies.USD
        };

        var expectedExchangeRates = new List<ExchangeRate>
        {
            new() { SourceCurrency = TestCurrencies.CZK, TargetCurrency = TestCurrencies.USD, Value = 22.50M, ValidFor = new DateTimeOffset(2024, 06, 07, 00, 00, 00, TimeSpan.Zero ) }
        };

        var exchangeRateProvider = InstantiateExchangeRateProvider(sourceExchangeRates);

        // When
        var actualExchangeRates = await exchangeRateProvider.GetExchangeRates(requestedCurrencies, CancellationToken.None);

        // Then
        Assert.Equivalent(expectedExchangeRates, actualExchangeRates);
    }

        [Fact]
    public async Task AlwaysReturnsExchangeRatesForSingleUnitOfCurrency()
    {
        // Given
        var sourceExchangeRates = new List<DailyRate>
        {
            new()
            {
                CurrencyCode = "JPY",
                Amount = 100,
                Rate = 14.50M,
                ValidFor = new DateTimeOffset(2024, 06, 07, 00, 00, 00, TimeSpan.Zero )
            }
        };

        var requestedCurrencies = new List<Currency> { TestCurrencies.JPY };

        var expectedExchangeRates = new List<ExchangeRate>
        {
            new() { SourceCurrency = TestCurrencies.CZK, TargetCurrency = TestCurrencies.JPY, Value = 0.145M, ValidFor = new DateTimeOffset(2024, 06, 07, 00, 00, 00, TimeSpan.Zero ) }
        };

        var exchangeRateProvider = InstantiateExchangeRateProvider(sourceExchangeRates);

        // When
        var actualExchangeRates = await exchangeRateProvider.GetExchangeRates(requestedCurrencies, CancellationToken.None);

        // Then
        Assert.Equivalent(expectedExchangeRates, actualExchangeRates);
    }

    public static class TestCurrencies
    {
        public static Currency AUD { get; } = new Currency("AUD");

        public static Currency CZK { get; } = new Currency("CZK");

        public static Currency GBP { get; } = new Currency("GBP");

        public static Currency JPY { get; } = new Currency("JPY");

        public static Currency USD { get; } = new Currency("USD");
    }

    public class StubCurrencyExchangeClient(IEnumerable<DailyRate> sourceExchangeRates) : ICurrencyExchangeClient
    {
        private readonly IEnumerable<DailyRate> _sourceExchangeRates = sourceExchangeRates;

        public Task<DailyRatesResponse> GetDailyRates(CancellationToken cancellationToken)
        {
            return Task.FromResult(new DailyRatesResponse { Rates = _sourceExchangeRates });
        }
    }

}