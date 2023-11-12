using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExchangeRateUpdater.Tests;

[Trait("Category", "Unit")]
public class ExchangeRateTransformerShould
{
    private static readonly List<CnbExchangeRate> ExchangeRates = new()
    {
        new CnbExchangeRate
        {
            CurrencyCode = "CZK",
            Amount = 1,
            ExchangeRate = 0.99m
        },
        new CnbExchangeRate
        {
            CurrencyCode = "USD",
            Amount = 1,
            ExchangeRate = 22.7m
        },
        new CnbExchangeRate
        {
            CurrencyCode = "GBP",
            Amount = 1,
            ExchangeRate = 28.13m
        },
        new CnbExchangeRate
        {
            CurrencyCode = "EUR",
            Amount = 1,
            ExchangeRate = 24.41m
        },
        new CnbExchangeRate
        {
            CurrencyCode = "TOP",
            Amount = 1,
            ExchangeRate = 9.50m
        },
        new CnbExchangeRate
        {
            CurrencyCode = "HUF",
            Amount = 100,
            ExchangeRate = 6.43m
        },
    };

    [Theory]
    [MemberData(nameof(GenerateTestData))]
    public void ReturnOnlySelectedCurrencies(string[] currencyCodes, IReadOnlyList<CnbExchangeRate> cnbRates, int expectedCount)
    {
        // arrange
        var currencies = currencyCodes.Select(cc => new Currency(cc)).ToList();
        var exchangeRatesDto = new CnbExchangeRatesDto { Rates = cnbRates };

        var transformer = new ExchangeRateTransformer(NullLogger.Instance);

        // act
        var exchangeRates = transformer.GetExchangeRatesForCurrencies(currencies, exchangeRatesDto);

        // assert
        Assert.Equal(expectedCount, exchangeRates.Count);
        Assert.All(
            exchangeRates,
            actual => Assert.Contains(actual.SourceCurrency.Code, currencyCodes));
    }

    [Fact]
    public void MapDtoToDomain()
    {
        // arrange
        var currencies = new[] { new Currency("EUR"), new Currency("HUF") };
        var exchangeRatesDto = new CnbExchangeRatesDto { Rates = ExchangeRates };

        var transformer = new ExchangeRateTransformer(NullLogger.Instance);

        // act
        var exchangeRates = transformer.GetExchangeRatesForCurrencies(currencies, exchangeRatesDto);

        // assert
        var eur = exchangeRates.First(r => r.SourceCurrency.Code == "EUR");
        Assert.Equal(24.41m, eur.Value);

        var huf = exchangeRates.First(r => r.SourceCurrency.Code == "HUF");
        Assert.Equal(0.0643m, huf.Value);
    }

    [Fact]
    public void SetTargetCurrencyToCzk()
    {
        // arrange
        var currencies = ExchangeRates.Select(r => new Currency(r.CurrencyCode)).ToList();
        var exchangeRatesDto = new CnbExchangeRatesDto { Rates = ExchangeRates };

        var transformer = new ExchangeRateTransformer(NullLogger.Instance);

        // act
        var exchangeRates = transformer.GetExchangeRatesForCurrencies(currencies, exchangeRatesDto);

        // assert
        Assert.All(
            exchangeRates,
            actual => Assert.Equal("CZK", actual.TargetCurrency.Code));
    }

    [Theory]
    [MemberData(nameof(GenerateLoggingTestData))]
    public void LogMissingCurrencies(string[] currencyCodes, string[] expectedLoggedCurrencies)
    {
        // arrange
        var currencies = currencyCodes.Select(cc => new Currency(cc)).ToList();
        var exchangeRatesDto = new CnbExchangeRatesDto { Rates = ExchangeRates };

        var logger = new TestLogger();
        var transformer = new ExchangeRateTransformer(logger);

        // act
        transformer.GetExchangeRatesForCurrencies(currencies, exchangeRatesDto);

        // assert
        // NB! Implementation detail - currently used transformer relies on data being sorted,
        //     if we change algorithm, this assertion needs to change as well
        Assert.Equal(expectedLoggedCurrencies.Length, logger.Messages.Count);
        for (var i = 0; i < expectedLoggedCurrencies.Length; i++)
        {
            Assert.Contains(expectedLoggedCurrencies[i], logger.Messages[i]);
        }
    }

    public static TheoryData<string[], IReadOnlyList<CnbExchangeRate>, int> GenerateTestData() => new()
    {
        // no input currencies -> no output (we filter all)
        { Array.Empty<string>(), ExchangeRates, 0 },

        // USD, EUR but no exchange rates -> no output (we have no data to pick from)
        { new[] { "USD", "EUR" }, new List<CnbExchangeRate>(), 0 },

        // AAA, XYZ not in exchange rates -> no output (we have no data to pick from)
        { new[] { "AAA", "XYZ" }, ExchangeRates, 0 },

        // USD, EUR in exchange rates -> expect both rates in output
        { new[] { "USD", "EUR" }, ExchangeRates, 2 },

        // all currencies in exchange rates -> expect all rates in output
        { new[] { "CZK", "EUR", "GBP", "HUF", "TOP", "USD" }, ExchangeRates, 6 },

        // pick first currency (sorted alphabetically) -> expect only first rate in output
        { new[] { "CZK" }, ExchangeRates, 1 },

        // pick last currency (sorted alphabetically) -> expect only last rate in output
        { new[] { "USD" }, ExchangeRates, 1 },
    };

    public static TheoryData<string[], string[]> GenerateLoggingTestData() => new()
    {
        // first currency (alphabetically sorted) -> no missing currency is logged
        { new[] { "CZK" }, Array.Empty<string>() },

        // more currencies before first one (alphabetically sorted) -> missing currencies should be logged
        { new[] { "BAD", "BOP", "CZK" }, new[] { "BAD", "BOP" } },

        // last currency (alphabetically sorted) -> no missing currency is logged
        { new[] { "USD" }, Array.Empty<string>() },

        // more currencies after last one (alphabetically sorted) -> missing currencies should be logged
        { new[] { "USD", "UYU", "VND" }, new[] { "UYU", "VND" } },

        // additional currencies before/after one present in rates (EUR, alphabetically sorted -> missing currencies should be logged
        { new[] { "ERN", "ETB", "EUR", "FJD", "FKP" }, new[] { "ERN", "ETB", "FJD", "FKP" } },

        // all currencies in exchange rates -> no missing currency is logged
        { new[] { "CZK", "EUR", "GBP", "HUF", "TOP", "USD" }, Array.Empty<string>() },

        // single currency not in rates -> currency is logged as missing
        { new[] { "RON" }, new[] { "RON" } },

        // multiple currencies not in rates -> currencies are logged if missing
        { new[] { "RON", "HUF", "CHF", "EUR", "ALL" }, new[] { "ALL", "CHF", "RON" } },
    };

    private class TestLogger : ILogger
    {
        private readonly List<string> _messages = new();

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            _messages.Add(formatter(state, null));
        }

        public IReadOnlyList<string> Messages => _messages;

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull =>
            throw new NotSupportedException();
    }
}