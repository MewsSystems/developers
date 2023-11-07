using ExchangeRateUpdater.Cnb;
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
        var exchangeRatesDto = new CnbExchangeRatesDto
        {
            Rates = cnbRates
        };

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
        var exchangeRatesDto = new CnbExchangeRatesDto
        {
            Rates = ExchangeRates
        };

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
        var exchangeRatesDto = new CnbExchangeRatesDto
        {
            Rates = ExchangeRates
        };

        var transformer = new ExchangeRateTransformer(NullLogger.Instance);

        // act
        var exchangeRates = transformer.GetExchangeRatesForCurrencies(currencies, exchangeRatesDto);

        // assert
        Assert.All(
            exchangeRates,
            actual => Assert.Equal("CZK", actual.TargetCurrency.Code));
    }

    public static TheoryData<string[], IReadOnlyList<CnbExchangeRate>, int> GenerateTestData() => new()
    {
        { Array.Empty<string>(), ExchangeRates, 0 },
        { new[] { "USD", "EUR" }, new List<CnbExchangeRate>(), 0 },
        { new[] { "AAA", "XYZ" }, ExchangeRates, 0 },
        { new[] { "USD", "EUR" }, ExchangeRates, 2 },
        { new[] { "CZK", "EUR", "GBP", "HUF", "TOP", "USD" }, ExchangeRates, 6 },
    };
}