using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Abstractions;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    [Fact]
    public async Task GetExchangeRatesAsync_ValidInput_ReturnsCorrectCombinations()
    {
        // Arrange
        var mockDataProvider = new Mock<IExchangeRateDataProvider>();

        // Define test currencies.
        var currencies = new List<Currency>
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK")
        };

        // Define normalized rates using easy-to-compute values:
        // USD -> 20, EUR -> 10, CZK -> 1.
        var normalizedRates = new Dictionary<string, (Currency Currency, decimal Rate)>(StringComparer.OrdinalIgnoreCase)
        {
            { "USD", (new Currency("USD"), 20m) },
            { "EUR", (new Currency("EUR"), 10m) },
            { "CZK", (new Currency("CZK"), 1m) }
        };

        mockDataProvider
            .Setup(provider => provider.GetNormalizedRatesAsync(currencies))
            .ReturnsAsync(normalizedRates);

        var exchangeRateProvider = new ExchangeRateProvider(mockDataProvider.Object);

        // Act
        var result = await exchangeRateProvider.GetExchangeRatesAsync(currencies);
        var resultList = result.ToList();

        // Should be n*(n-1) = 6 combinations for 3 currencies.
        Assert.Equal(6, resultList.Count);

        // Verify USD/EUR: 20 / 10 = 2.000
        Assert.Contains(resultList, r =>
            r.SourceCurrency.Code.Equals("USD", StringComparison.OrdinalIgnoreCase) &&
            r.TargetCurrency.Code.Equals("EUR", StringComparison.OrdinalIgnoreCase) &&
            Math.Abs(r.Value - 2.000m) < 0.001m);

        // Verify USD/CZK: 20 / 1 = 20.000
        Assert.Contains(resultList, r =>
            r.SourceCurrency.Code.Equals("USD", StringComparison.OrdinalIgnoreCase) &&
            r.TargetCurrency.Code.Equals("CZK", StringComparison.OrdinalIgnoreCase) &&
            Math.Abs(r.Value - 20.000m) < 0.001m);

        // Verify EUR/USD: 10 / 20 = 0.500
        Assert.Contains(resultList, r =>
            r.SourceCurrency.Code.Equals("EUR", StringComparison.OrdinalIgnoreCase) &&
            r.TargetCurrency.Code.Equals("USD", StringComparison.OrdinalIgnoreCase) &&
            Math.Abs(r.Value - 0.500m) < 0.001m);

        // Verify EUR/CZK: 10 / 1 = 10.000
        Assert.Contains(resultList, r =>
            r.SourceCurrency.Code.Equals("EUR", StringComparison.OrdinalIgnoreCase) &&
            r.TargetCurrency.Code.Equals("CZK", StringComparison.OrdinalIgnoreCase) &&
            Math.Abs(r.Value - 10.000m) < 0.001m);

        // Verify CZK/USD: 1 / 20 = 0.050
        Assert.Contains(resultList, r =>
            r.SourceCurrency.Code.Equals("CZK", StringComparison.OrdinalIgnoreCase) &&
            r.TargetCurrency.Code.Equals("USD", StringComparison.OrdinalIgnoreCase) &&
            Math.Abs(r.Value - 0.050m) < 0.001m);

        // Verify CZK/EUR: 1 / 10 = 0.100
        Assert.Contains(resultList, r =>
            r.SourceCurrency.Code.Equals("CZK", StringComparison.OrdinalIgnoreCase) &&
            r.TargetCurrency.Code.Equals("EUR", StringComparison.OrdinalIgnoreCase) &&
            Math.Abs(r.Value - 0.100m) < 0.001m);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_EmptyInput_ReturnsEmpty()
    {
        // Arrange
        var mockDataProvider = new Mock<IExchangeRateDataProvider>();
        var currencies = new List<Currency>();

        mockDataProvider
            .Setup(provider => provider.GetNormalizedRatesAsync(currencies))
            .ReturnsAsync(new Dictionary<string, (Currency, decimal)>());

        var exchangeRateProvider = new ExchangeRateProvider(mockDataProvider.Object);

        // Act
        var result = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_InsufficientCurrencies_ReturnsEmpty()
    {
        // Arrange
        var mockDataProvider = new Mock<IExchangeRateDataProvider>();
        var currencies = new List<Currency> { new Currency("USD") };

        var normalizedRates = new Dictionary<string, (Currency, decimal)>
        {
            { "USD", (new Currency("USD"), 20m) }
        };

        mockDataProvider
            .Setup(provider => provider.GetNormalizedRatesAsync(currencies))
            .ReturnsAsync(normalizedRates);

        var exchangeRateProvider = new ExchangeRateProvider(mockDataProvider.Object);

        // Act
        var result = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetExchangeRatesAsync_NullInput_ReturnsEmpty()
    {
        // Arrange
        var mockDataProvider = new Mock<IExchangeRateDataProvider>();
        mockDataProvider
            .Setup(provider => provider.GetNormalizedRatesAsync(null))
            .ReturnsAsync(new Dictionary<string, (Currency, decimal)>());

        var exchangeRateProvider = new ExchangeRateProvider(mockDataProvider.Object);

        // Act
        var result = await exchangeRateProvider.GetExchangeRatesAsync(null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
