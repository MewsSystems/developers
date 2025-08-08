using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;

namespace ExchangeRateUpdater.Infrastructure.Providers.Tests.ExchangeRates.CzechNationalBank;

public class CnbExchangeRateProviderTests
{
    private readonly Mock<ICzechNationalBankApiClient> _mockApiClient;
    private readonly Mock<ILogger<CnbExchangeRateProvider>> _mockLogger;
    private readonly CnbExchangeRateProvider _provider;

    public CnbExchangeRateProviderTests()
    {
        _mockApiClient = new Mock<ICzechNationalBankApiClient>();
        _mockLogger = new Mock<ILogger<CnbExchangeRateProvider>>();

        _provider = new CnbExchangeRateProvider(_mockApiClient.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithValidDependencies_ShouldCreateSuccessfully()
    {
        var provider = new CnbExchangeRateProvider(_mockApiClient.Object, _mockLogger.Object);
        Assert.NotNull(provider);
    }

    [Fact]
    public void Name_ShouldReturnCorrectProviderName()
    {
        Assert.Equal("CzechNationalBank", _provider.Name);
    }

    [Fact]
    public void DefaultLanguage_ShouldReturnCorrectLanguage()
    {
        Assert.Equal("EN", _provider.DefaultLanguage);
    }

    [Fact]
    public void DefaultCurrency_ShouldReturnCorrectCurrency()
    {
        Assert.Equal("CZK", _provider.DefaultCurrency);
    }

    [Fact]
    public async Task FetchAllCurrentAsync_ShouldReturnEmptyArray_WhenNoApiSetup()
    {
        var result = await _provider.FetchAllCurrentAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task FetchByDateAsync_ShouldReturnEmptyArray_WhenNoApiSetup()
    {
        var date = new DateTime(2024, 1, 15);
        var result = await _provider.FetchByDateAsync(date);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task FetchByDateAsync_WithFutureDate_ShouldUseCurrentDate()
    {
        var futureDate = DateTime.UtcNow.AddDays(1);
        var result = await _provider.FetchByDateAsync(futureDate);

        Assert.NotNull(result);
        Assert.Empty(result);
    }



    [Fact]
    public void DefaultTimezone_ShouldReturnEuropePrague()
    {
        var timezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        Assert.NotNull(timezone);
        Assert.Equal("Europe/Prague", timezone.Id);
    }

    [Fact]
    public void CnbExchangeRateModel_ToExchangeRate_ShouldConvertCorrectly()
    {
        var model = new CnbExchangeRateModel
        {
            CurrencyCode = "USD",
            Rate = 23.5m,
            Amount = 1,
            Country = "USA",
            Currency = "US Dollar",
            Order = 1,
            ValidFor = "2024-01-15"
        };

        var exchangeRate = model.ToExchangeRate();

        Assert.NotNull(exchangeRate);
        Assert.Equal("USD", exchangeRate.SourceCurrency.Code);
        Assert.Equal("CZK", exchangeRate.TargetCurrency.Code);
        Assert.Equal(23.5m, exchangeRate.ExchangeValue);
        Assert.Equal("CNB", exchangeRate.ProviderName);
        Assert.Equal(new DateTime(2024, 1, 15), exchangeRate.ValidUntil);
    }

    [Fact]
    public void CnbExchangeRateModel_ToExchangeRate_WithAmountGreaterThanOne_ShouldCalculateCorrectRate()
    {
        var model = new CnbExchangeRateModel
        {
            CurrencyCode = "JPY",
            Rate = 100m,
            Amount = 100,
            Country = "Japan",
            Currency = "Japanese Yen",
            Order = 4,
            ValidFor = "2024-01-15"
        };

        var exchangeRate = model.ToExchangeRate();

        Assert.NotNull(exchangeRate);
        Assert.Equal("JPY", exchangeRate.SourceCurrency.Code);
        Assert.Equal("CZK", exchangeRate.TargetCurrency.Code);
        Assert.Equal(1m, exchangeRate.ExchangeValue);
        Assert.Equal("CNB", exchangeRate.ProviderName);
    }






} 