using System;
using System.Linq;
using System.Threading.Tasks;
using Mews.BackendDeveloperTask.ExchangeRates.Cnb;
using Moq;
using NUnit.Framework;

namespace Mews.BackendDeveloperTask.ExchangeRates.UnitTests;

public class CnbExchangeRateProviderTests
{
    [Test]
    public async Task ReturnsExchangeRatesForSpecifiedCurrenciesAsDefinedBySource()
    {
        // Should return exchange rates among the specified currencies that are defined by the source...
        // Arrange
        var mockParserResult = new[] {
            new ExchangeRate(Currency.USD, Currency.CZK, 23.49m),
            new ExchangeRate(Currency.EUR, Currency.CZK, 24.71m),
        };
        var exchangeRateProvider = CreateExchangeRateProvider(mockParserResult);

        // Act
        var specifiedCurrencies = new[] { Currency.USD, Currency.EUR };
        var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(specifiedCurrencies);

        // Assert
        var expectedExchangeRates = new[] {
            new ExchangeRate(Currency.EUR, Currency.CZK, 24.71m),
            new ExchangeRate(Currency.USD, Currency.CZK, 23.49m)
        };
        Assert.AreEqual(expectedExchangeRates.OrderBy(o => o.Source), actualExchangeRates.OrderBy(o => o.Source));
    }

    [Test]
    public async Task ReturnsEmptyWhenAllCurrenciesNotDefinedBySource()
    {
        // Should return exchange rates among the specified currencies that are defined by the source...
        // Arrange
        var exchangeRateProvider = CreateExchangeRateProvider(Array.Empty<ExchangeRate>());

        // Act
        var specifiedCurrencies = new[] { Currency.USD, Currency.EUR };
        var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(specifiedCurrencies);

        // Assert
        var expectedExchangeRates = Array.Empty<ExchangeRate>();
        Assert.AreEqual(expectedExchangeRates.OrderBy(o => o.Source), actualExchangeRates.OrderBy(o => o.Source));
    }

    [Test]
    public async Task ReturnsExchangeRatesForDefinedSpecifiedCurrenciesWhenSomeNotDefinedBySource()
    {
        // Should return exchange rates among the specified currencies that are defined by the source. But only those defined by the source
        // Arrange
        var exchangeRates = new[] { new ExchangeRate(Currency.USD, Currency.CZK, 23.49m) };
        var exchangeRateProvider = CreateExchangeRateProvider(exchangeRates);

        // Act
        var specifiedCurrencies = new[] { Currency.USD, Currency.EUR };
        var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(specifiedCurrencies);

        // Assert
        var expectedExchangeRates = new[] {
            new ExchangeRate(Currency.USD, Currency.CZK, 23.49m)
        };
        Assert.AreEqual(expectedExchangeRates.OrderBy(o => o.Source), actualExchangeRates.OrderBy(o => o.Source));
    }

    [Test]
    public async Task ReturnsEmptyWhenCurrencyIsTargetInSourceCurrenciesButNotItselfASource()
    {
        // Do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK" do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD".
        // Arrange
        var exchangeRates = new[] { new ExchangeRate(Currency.CZK, Currency.USD, 1 / 23.49m) };
        var exchangeRateProvider = CreateExchangeRateProvider(exchangeRates);

        // Act
        var specifiedCurrencies = new[] { Currency.USD, Currency.EUR };
        var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(specifiedCurrencies);

        // Assert
        var expectedExchangeRates = Array.Empty<ExchangeRate>();
        Assert.AreEqual(expectedExchangeRates.OrderBy(o => o.Source), actualExchangeRates.OrderBy(o => o.Source));
    }

    private static CnbExchangeRateProvider CreateExchangeRateProvider(ExchangeRate[] mockParserResult)
    {
        var mockText = "CNB File";
        var mockRetriever = new Mock<ICnbTextExchangeRateRetriever>();
        mockRetriever.Setup(r => r.GetDailyRatesAsync()).ReturnsAsync(mockText);
        var mockParser = new Mock<ICnbTextExchangeRateParser>();
        mockParser.Setup(p => p.Parse(mockText)).Returns(mockParserResult);
        var exchangeRateProvider = new CnbExchangeRateProvider(mockRetriever.Object, mockParser.Object);
        return exchangeRateProvider;
    }
}