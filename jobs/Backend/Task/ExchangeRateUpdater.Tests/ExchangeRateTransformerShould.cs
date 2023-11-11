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
    public void Log(string[] currencyCodes, string[] expectedLoggedCurrencies)
    {
        // arrange
        var currencies = currencyCodes.Select(cc => new Currency(cc)).ToList();
        var exchangeRatesDto = new CnbExchangeRatesDto { Rates = ExchangeRates };

        var logger = new TestLogger();
        var transformer = new ExchangeRateTransformer(logger);

        // act
        transformer.GetExchangeRatesForCurrencies(currencies, exchangeRatesDto);

        // assert
        Assert.Equal(expectedLoggedCurrencies.Length, logger.Messages.Count);
        for (var i = 0; i < expectedLoggedCurrencies.Length; i++)
        {
            Assert.Contains(expectedLoggedCurrencies[i], logger.Messages[i]);
        }
    }

    public static TheoryData<string[], IReadOnlyList<CnbExchangeRate>, int> GenerateTestData() => new()
    {
        { Array.Empty<string>(), ExchangeRates, 0 },
        { new[] { "USD", "EUR" }, new List<CnbExchangeRate>(), 0 },
        { new[] { "AAA", "XYZ" }, ExchangeRates, 0 },
        { new[] { "USD", "EUR" }, ExchangeRates, 2 },
        {
            new[]
            {
                "CZK",
                "EUR",
                "GBP",
                "HUF",
                "TOP",
                "USD",
            },
            ExchangeRates, 6
        },
        { new[] { "CZK" }, ExchangeRates, 1 },
        { new[] { "USD" }, ExchangeRates, 1 },
    };

    public static TheoryData<string[], string[]> GenerateLoggingTestData() => new()
    {
        { new[] { "CZK" }, Array.Empty<string>() },
        { new[] { "USD" }, Array.Empty<string>() },
        {
            new[]
            {
                "CZK",
                "EUR",
                "GBP",
                "HUF",
                "TOP",
                "USD",
            },
            Array.Empty<string>()
        },
        { new[] { "RON" }, new[] { "RON" } },
        {
            new[]
            {
                "RON", // 3
                "GBP", // 2: present in exchange rates
                "CHF", // 1
                "USD", // 4: present in exchange rates
                "ALL", // 0
            },
            new[]
            {
                "ALL",
                "CHF",
                "RON",
            }
        },
        {
            new[]
            {
                "RON", // 4
                "HUF", // 3: present in exchange rates
                "CHF", // 1
                "EUR", // 2: present in exchange rates
                "ALL", // 0
            },
            new[]
            {
                "ALL",
                "CHF",
                "RON",
            }
        },
        {
            new[]
            {
                "ERN",
                "ETB",
                "EUR", // 2: present in exchange rates
                "FJD",
                "FKP",
            },
            new[]
            {
                "ERN",
                "ETB",
                "FJD",
                "FKP"
            }
        },
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
            throw new NotImplementedException();
    }
}