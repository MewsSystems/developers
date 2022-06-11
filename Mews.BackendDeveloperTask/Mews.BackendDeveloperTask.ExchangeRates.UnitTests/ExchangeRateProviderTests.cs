using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Mews.BackendDeveloperTask.ExchangeRates.UnitTests;

public class ExchangeRateProviderTests
{
    [Test]
    public async Task ReturnsExchangeRatesForSpecifiedCurrenciesAsDefinedBySource()
    {
        // Should return exchange rates among the specified currencies that are defined by the source...
        // Arrange
        var mockDataSource = new Mock<IExchangeRateDataSource>();
        mockDataSource.Setup(s => s.GetExchangeRatesAsync()).ReturnsAsync(new[] {
            new ExchangeRate(Currency.USD, Currency.CZK, 23.49f),
            new ExchangeRate(Currency.EUR, Currency.CZK, 24.71f),
        });
        var exchangeRateProvider = new ExchangeRateProvider(mockDataSource.Object);

        // Act
        var specifiedCurrencies = new[] { Currency.USD, Currency.EUR };
        var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(specifiedCurrencies);

        // Assert
        var expectedExchangeRates = new[] {
            new ExchangeRate(Currency.EUR, Currency.CZK, 24.71f),
            new ExchangeRate(Currency.USD, Currency.CZK, 23.49f)
        };
        Assert.AreEqual(expectedExchangeRates.OrderBy(o => o.Source), actualExchangeRates.OrderBy(o => o.Source));
    }

    [Test]
    public async Task ReturnsEmptyWhenAllCurrenciesNotDefinedBySource()
    {
        // Should return exchange rates among the specified currencies that are defined by the source...
        // Arrange
        var mockDataSource = new Mock<IExchangeRateDataSource>();
        mockDataSource.Setup(s => s.GetExchangeRatesAsync()).ReturnsAsync(Array.Empty<ExchangeRate>());
        var exchangeRateProvider = new ExchangeRateProvider(mockDataSource.Object);

        // Act
        var specifiedCurrencies = new[] { Currency.USD, Currency.EUR };
        var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(specifiedCurrencies);

        // Assert
        var expectedExchangeRates = Array.Empty<ExchangeRate>();
        Assert.AreEqual(expectedExchangeRates.OrderBy(o => o.Source), actualExchangeRates.OrderBy(o => o.Source));
    }

    [Test]
    public void ReturnsExchangeRatesForDefinedSpecifiedCurrenciesWhenSomeNotDefinedBySource()
    {
        // Should return exchange rates among the specified currencies that are defined by the source. But only those defined by the source
        throw new NotImplementedException();
    }

    [Test]
    public void ReturnsEmptyWhenCurrencyIsTargetInSourceCurrenciesButNotItselfASource()
    {
        // Do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK" do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD".
        throw new NotImplementedException();
    }
}