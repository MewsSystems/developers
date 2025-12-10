
using ExchangeRateUpdater.Common.Models;
using ExchangeRateUpdater.Features.ExchangeRates.Models;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater.Unit.Tests;
public class ExchangeRateProviderTests
{
    [Fact]
    public void GetExchangeRates_FiltersAndMapsCorrectly()
    {
        // Arrange
        var provider = new ExchangeRateProvider();
        var currencies = new List<Currency> { new("USD"), new("EUR"), new("CZK") };

        var sourceRates = new List<ExchangeRateResponse>
        {
            new() { CurrencyCode = "USD", Rate = 22.5m },
            new() { CurrencyCode = "EUR", Rate = 25.0m },
            new() { CurrencyCode = "XYZ", Rate = 99m } // should be ignored
        };

        // Act
        var results = provider.GetExchangeRates(currencies, sourceRates).ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Contains(results, r => r.SourceCurrency.Code == "USD" && r.TargetCurrency.Code == "CZK" && r.Value == 22.5m);
        Assert.Contains(results, r => r.SourceCurrency.Code == "EUR" && r.TargetCurrency.Code == "CZK" && r.Value == 25.0m);
        Assert.DoesNotContain(results, r => r.SourceCurrency.Code == "XYZ");
    }

    [Fact]
    public void GetExchangeRates_EmptySourceRates_ReturnsEmpty()
    {
        // Arrange
        var provider = new ExchangeRateProvider();
        var currencies = new List<Currency> { new("USD"), new("CZK") };
        var sourceRates = new List<ExchangeRateResponse>();

        // Act
        var results = provider.GetExchangeRates(currencies, sourceRates);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void GetExchangeRates_EmptyCurrencies_ReturnsEmpty()
    {
        // Arrange
        var provider = new ExchangeRateProvider();
        var currencies = new List<Currency>();
        var sourceRates = new List<ExchangeRateResponse>()
        {
            new()
            {
                CurrencyCode = "USD",
                Rate = 22.5m
            }
        };

        // Act
        var results = provider.GetExchangeRates(currencies, sourceRates);

        // Assert
        Assert.Empty(results);
    }
}
