using ExchangeRateUpdater.Application.Exceptions;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Application.Tests.Services;

public class ExchangeRateServiceTests
{
    private readonly Mock<IExchangeRateProvider> _providerMock;
    private readonly Mock<ILogger<ExchangeRateService>> _loggerMock;
    private readonly ExchangeRateService _service;

    public ExchangeRateServiceTests()
    {
        _providerMock = new Mock<IExchangeRateProvider>();
        _loggerMock = new Mock<ILogger<ExchangeRateService>>();
        _service = new ExchangeRateService(_providerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ReturnsExchangeRates_WhenDataExists()
    {
        // Arrange
        var date = DateTime.Today;
        var rates = new List<ExchangeRate>
        {
            new(new Currency("CZK"), new Currency("USD"), 22.50m),
            new(new Currency("CZK"), new Currency("EUR"), 25.30m)
        };
        _providerMock.Setup(p => p.GetExchangeRatesAsync(date, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(rates);

        // Act
        var response = await _service.GetExchangeRatesAsync(date, null, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.ExchangeRates.Count());
        Assert.Contains(response.ExchangeRates, r => r.TargetCurrency.Code == "USD" && r.Value == 22.50m);
        Assert.Contains(response.ExchangeRates, r => r.TargetCurrency.Code == "EUR" && r.Value == 25.30m);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ThrowsNotFoundException_WhenNoDataExists()
    {
        // Arrange
        var date = DateTime.Today;
        _providerMock.Setup(p => p.GetExchangeRatesAsync(date, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(Enumerable.Empty<ExchangeRate>());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetExchangeRatesAsync(date, null, CancellationToken.None));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_FiltersByRequestedCurrencies()
    {
        // Arrange
        var date = DateTime.Today;
        var rates = new List<ExchangeRate>
        {
            new(new Currency("CZK"), new Currency("USD"), 22.50m),
            new(new Currency("CZK"), new Currency("EUR"), 25.30m),
            new(new Currency("CZK"), new Currency("GBP"), 30.50m)
        };
        _providerMock.Setup(p => p.GetExchangeRatesAsync(date, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(rates);
        var requestedCurrencies = new List<string> { "USD", "GBP" };

        // Act
        var response = await _service.GetExchangeRatesAsync(date, requestedCurrencies, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.ExchangeRates.Count());
        Assert.Contains(response.ExchangeRates, r => r.TargetCurrency.Code == "USD" && r.Value == 22.50m);
        Assert.Contains(response.ExchangeRates, r => r.TargetCurrency.Code == "GBP" && r.Value == 30.50m);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_RecordsMissingCurrencies_WhenSomeRequestedAreUnavailable()
    {
        // Arrange
        var date = DateTime.Today;
        var rates = new List<ExchangeRate>
        {
            new(new Currency("CZK"), new Currency("USD"), 22.50m)
        };
        _providerMock.Setup(p => p.GetExchangeRatesAsync(date, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(rates);
        var requestedCurrencies = new List<string> { "USD", "GBP" };

        // Act
        var response = await _service.GetExchangeRatesAsync(date, requestedCurrencies, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.ExchangeRates);
        Assert.Contains(response.ExchangeRates, r => r.TargetCurrency.Code == "USD" && r.Value == 22.50m);
        Assert.Contains(response.MissingCurrencies!, c => c == "GBP");
    }
}
