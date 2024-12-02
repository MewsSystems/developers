using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdaterTests;

public class ExchangeRateServiceTests
{
    private readonly Mock<IExchangeRateRetriever> _retrieverMock;
    private readonly Mock<IExchangeRateCache> _cache;
    private readonly Mock<ILogger<ExchangeRateService>> _loggerMock;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly ExchangeRateService _exchangeRateService;

    public ExchangeRateServiceTests()
    {
        _retrieverMock = new Mock<IExchangeRateRetriever>();
        _cache = new Mock<IExchangeRateCache>();
        _loggerMock = new Mock<ILogger<ExchangeRateService>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _exchangeRateService = new ExchangeRateService(_mockConfiguration.Object, _retrieverMock.Object, _loggerMock.Object, _cache.Object);
    }


    [Fact]
    public async Task GetExchangeRatesAsync_ShouldReturnRates_WhenCacheIsEmpty()
    {
        // Arrange
        var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };

        var retrievedRates = new[]
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 23.45m),
            new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 25.67m)
        };

        _retrieverMock.Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(Result<ExchangeRate[]>.Success(retrievedRates));

        // Act
        var result = await _exchangeRateService.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Length);
        Assert.Contains(result.Value, rate => rate.TargetCurrency.Code == "USD" && rate.Value == 23.45m);
        Assert.Contains(result.Value, rate => rate.TargetCurrency.Code == "EUR" && rate.Value == 25.67m);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ShouldReturnFailure_WhenProviderFails()
    {
        // Arrange
        var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };

        _retrieverMock.Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(Result<ExchangeRate[]>
            .Failure("Provider error"));

        // Act
        var result = await _exchangeRateService
            .GetExchangeRatesAsync(currencies)
            .ConfigureAwait(false);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Provider error", result.Error);
    }
}
