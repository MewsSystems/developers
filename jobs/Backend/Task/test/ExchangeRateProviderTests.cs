using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    private readonly Mock<ICnbExchangeRateService> _cnbServiceMock;
    private readonly Mock<ILogger<ExchangeRateProvider>> _loggerMock;
    private readonly ExchangeRateProvider _provider;

    public ExchangeRateProviderTests()
    {
        _cnbServiceMock = new Mock<ICnbExchangeRateService>();
        _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
        _provider = new ExchangeRateProvider(_cnbServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ValidCurrencies_ReturnsMatchingRates()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var cnbRates = new Dictionary<string, decimal>
        {
            { "USD", 24.5m },
            { "EUR", 25.0m }
        };

        _cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var result = await _provider.GetExchangeRatesAsync(currencies);

        // Assert
        var ratesList = result.ToList();
        Assert.Equal(2, ratesList.Count);
        
        var usdRate = ratesList.FirstOrDefault(r => r.TargetCurrency.Code == "USD");
        Assert.NotNull(usdRate);
        Assert.Equal("CZK", usdRate!.SourceCurrency.Code);
        Assert.Equal(24.5m, usdRate.Value);

        var eurRate = ratesList.FirstOrDefault(r => r.TargetCurrency.Code == "EUR");
        Assert.NotNull(eurRate);
        Assert.Equal("CZK", eurRate!.SourceCurrency.Code);
        Assert.Equal(25.0m, eurRate.Value);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_CurrencyNotInCnb_IgnoresCurrency()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("XYZ") };
        var cnbRates = new Dictionary<string, decimal>
        {
            { "USD", 24.5m }
        };

        _cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var result = await _provider.GetExchangeRatesAsync(currencies);

        // Assert
        var ratesList = result.ToList();
        Assert.Single(ratesList);
        Assert.Equal("USD", ratesList[0].TargetCurrency.Code);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_CzkCurrency_SkipsCzk()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("CZK") };
        var cnbRates = new Dictionary<string, decimal>
        {
            { "USD", 24.5m }
        };

        _cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var result = await _provider.GetExchangeRatesAsync(currencies);

        // Assert
        var ratesList = result.ToList();
        Assert.Single(ratesList);
        Assert.Equal("USD", ratesList[0].TargetCurrency.Code);
        Assert.DoesNotContain(ratesList, r => r.TargetCurrency.Code == "CZK");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_EmptyCurrencies_ReturnsEmpty()
    {
        // Arrange
        var currencies = Array.Empty<Currency>();
        var cnbRates = new Dictionary<string, decimal>();

        _cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var result = await _provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_NullCurrencies_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _provider.GetExchangeRatesAsync(null!));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_CnbServiceThrows_PropagatesException()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };
        _cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CnbApiException("CNB service error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CnbApiException>(() => 
            _provider.GetExchangeRatesAsync(currencies));
        Assert.Contains("CNB service error", exception.Message);
    }

}
