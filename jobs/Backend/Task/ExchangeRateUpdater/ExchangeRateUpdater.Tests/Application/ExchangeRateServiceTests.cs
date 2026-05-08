using Moq;
using Xunit;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Tests.Application;

public class ExchangeRateServiceTests
{
    private readonly Mock<IExchangeRateRepository> _mockRepo = new();
    private readonly ExchangeRateService _service;

    public ExchangeRateServiceTests()
    {
        _service = new ExchangeRateService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllRatesAsync_MapsDomainToDto()
    {
        const string currencyCode = "EUR";
        const decimal rateToCZK = 24.170m;
        // Arrange
        _mockRepo.Setup(r => r.GetCurrentRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, ExchangeRate>
            {
                [currencyCode] = new ExchangeRate(currencyCode, rateToCZK, DateTime.UtcNow)
            });

        // Act
        var result = await _service.GetAllRatesAsync();

        // Assert
        Assert.Single(result.Rates);
        Assert.Equal(currencyCode, result.Rates[0].CurrencyCode);
        Assert.Equal(rateToCZK, result.Rates[0].RateToCZK);
    }

    [Fact]
    public async Task GetRateByCurrencyAsync_NotFound_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetCurrentRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, ExchangeRate>());

        var result = await _service.GetRateByCurrencyAsync("XXX");

        Assert.Null(result);
    }
}
