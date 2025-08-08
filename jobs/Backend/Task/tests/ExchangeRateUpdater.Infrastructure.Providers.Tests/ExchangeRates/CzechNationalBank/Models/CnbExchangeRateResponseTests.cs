using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;

namespace ExchangeRateUpdater.Infrastructure.Providers.Tests.ExchangeRates.CzechNationalBank.Models;

public class CnbExchangeRateResponseTests
{
    [Fact]
    public void CnbExchangeRateResponse_WithValidData_ShouldCreateSuccessfully()
    {
        var response = new CnbExchangeRateResponse
        {
            Rates =
            [
                new CnbExchangeRateModel { CurrencyCode = "USD", Rate = 23.5m, Amount = 1, Country = "USA", Currency = "US Dollar", Order = 1, ValidFor = "2024-01-15" },
                new CnbExchangeRateModel { CurrencyCode = "EUR", Rate = 25.2m, Amount = 1, Country = "EU", Currency = "Euro", Order = 2, ValidFor = "2024-01-15" }
            ]
        };

        Assert.NotNull(response);
        Assert.NotNull(response.Rates);
        Assert.Equal(2, response.Rates.Length);
        Assert.Equal("USD", response.Rates[0].CurrencyCode);
        Assert.Equal(23.5m, response.Rates[0].Rate);
        Assert.Equal("EUR", response.Rates[1].CurrencyCode);
        Assert.Equal(25.2m, response.Rates[1].Rate);
    }

    [Fact]
    public void CnbExchangeRateResponse_WithEmptyRates_ShouldCreateSuccessfully()
    {
        var response = new CnbExchangeRateResponse
        {
            Rates = []
        };

        Assert.NotNull(response);
        Assert.NotNull(response.Rates);
        Assert.Empty(response.Rates);
    }

    [Fact]
    public void CnbExchangeRateModel_WithValidData_ShouldCreateSuccessfully()
    {
        var rate = new CnbExchangeRateModel
        {
            CurrencyCode = "USD",
            Rate = 23.5m,
            Amount = 1,
            Country = "USA",
            Currency = "US Dollar",
            Order = 1,
            ValidFor = "2024-01-15"
        };

        Assert.Equal("USD", rate.CurrencyCode);
        Assert.Equal(23.5m, rate.Rate);
        Assert.Equal(1, rate.Amount);
        Assert.Equal("USA", rate.Country);
        Assert.Equal("US Dollar", rate.Currency);
        Assert.Equal(1, rate.Order);
        Assert.Equal("2024-01-15", rate.ValidFor);
    }

    [Fact]
    public void CnbExchangeRateModel_WithZeroRate_ShouldCreateSuccessfully()
    {
        var rate = new CnbExchangeRateModel
        {
            CurrencyCode = "JPY",
            Rate = 0m,
            Amount = 100,
            Country = "Japan",
            Currency = "Japanese Yen",
            Order = 4,
            ValidFor = "2024-01-15"
        };

        Assert.Equal("JPY", rate.CurrencyCode);
        Assert.Equal(0m, rate.Rate);
        Assert.Equal(100, rate.Amount);
        Assert.Equal("Japan", rate.Country);
        Assert.Equal("Japanese Yen", rate.Currency);
        Assert.Equal(4, rate.Order);
        Assert.Equal("2024-01-15", rate.ValidFor);
    }

    [Fact]
    public void CnbExchangeRateModel_WithNegativeRate_ShouldCreateSuccessfully()
    {
        var rate = new CnbExchangeRateModel
        {
            CurrencyCode = "GBP",
            Rate = -1.5m,
            Amount = 1,
            Country = "UK",
            Currency = "British Pound",
            Order = 5,
            ValidFor = "2024-01-15"
        };

        Assert.Equal("GBP", rate.CurrencyCode);
        Assert.Equal(-1.5m, rate.Rate);
        Assert.Equal(1, rate.Amount);
        Assert.Equal("UK", rate.Country);
        Assert.Equal("British Pound", rate.Currency);
        Assert.Equal(5, rate.Order);
        Assert.Equal("2024-01-15", rate.ValidFor);
    }
} 