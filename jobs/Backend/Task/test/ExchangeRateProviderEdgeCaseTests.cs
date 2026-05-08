using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderEdgeCaseTests
{
    private Mock<ICnbExchangeRateService> CreateCnbServiceMock()
    {
        return new Mock<ICnbExchangeRateService>();
    }

    private Mock<ILogger<ExchangeRateProvider>> CreateLoggerMock()
    {
        return new Mock<ILogger<ExchangeRateProvider>>();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_DuplicateCurrencies()
    {
        // Arrange
        var cnbServiceMock = CreateCnbServiceMock();
        var loggerMock = CreateLoggerMock();
        var provider = new ExchangeRateProvider(cnbServiceMock.Object, loggerMock.Object);

        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("USD"), // Duplicate
            new Currency("EUR")
        };

        var cnbRates = new Dictionary<string, decimal>
        {
            { "USD", 24.5m },
            { "EUR", 26.0m }
        };

        cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var ratesList = result.ToList();
        Assert.True(ratesList.Count >= 2);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_AllCurrenciesNotFound_ReturnsEmpty()
    {
        // Arrange
        var cnbServiceMock = CreateCnbServiceMock();
        var loggerMock = CreateLoggerMock();
        var provider = new ExchangeRateProvider(cnbServiceMock.Object, loggerMock.Object);

        var currencies = new[]
        {
            new Currency("XYZ"),
            new Currency("ABC"),
            new Currency("DEF")
        };

        var cnbRates = new Dictionary<string, decimal>();

        cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_EmptyCnbResponse_ReturnsEmpty()
    {
        // Arrange
        var cnbServiceMock = CreateCnbServiceMock();
        var loggerMock = CreateLoggerMock();
        var provider = new ExchangeRateProvider(cnbServiceMock.Object, loggerMock.Object);

        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var cnbRates = new Dictionary<string, decimal>();

        cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_CnbReturnsOnlyOneCurrency()
    {
        // Arrange
        var cnbServiceMock = CreateCnbServiceMock();
        var loggerMock = CreateLoggerMock();
        var provider = new ExchangeRateProvider(cnbServiceMock.Object, loggerMock.Object);

        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("GBP")
        };

        var cnbRates = new Dictionary<string, decimal>
        {
            { "USD", 24.5m }
        };

        cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var ratesList = result.ToList();
        Assert.Single(ratesList);
        Assert.Equal("USD", ratesList[0].TargetCurrency.Code);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_NullCurrencyInCollection()
    {
        // Arrange
        var cnbServiceMock = CreateCnbServiceMock();
        var loggerMock = CreateLoggerMock();
        var provider = new ExchangeRateProvider(cnbServiceMock.Object, loggerMock.Object);

        var currencies = new Currency?[]
        {
            new Currency("USD"),
            null,
            new Currency("EUR")
        };

        var cnbRates = new Dictionary<string, decimal>
        {
            { "USD", 24.5m },
            { "EUR", 26.0m }
        };

        cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var validCurrencies = currencies.Where(c => c != null).Cast<Currency>();
        var result = await provider.GetExchangeRatesAsync(validCurrencies);

        // Assert
        var ratesList = result.ToList();
        Assert.Equal(2, ratesList.Count);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_OperationCanceled_ThrowsException()
    {
        // Arrange
        var cnbServiceMock = CreateCnbServiceMock();
        var loggerMock = CreateLoggerMock();
        var provider = new ExchangeRateProvider(cnbServiceMock.Object, loggerMock.Object);

        var currencies = new[] { new Currency("USD") };

        cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            provider.GetExchangeRatesAsync(currencies));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_CurrencyWithWhitespace()
    {
        // Arrange
        var cnbServiceMock = CreateCnbServiceMock();
        var loggerMock = CreateLoggerMock();
        var provider = new ExchangeRateProvider(cnbServiceMock.Object, loggerMock.Object);

        var currencies = new[] { new Currency("USD") };
        var cnbRates = new Dictionary<string, decimal>
        {
            { "USD", 24.5m }
        };

        cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cnbRates);

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ServiceReturnsNullDictionary()
    {
        // Arrange
        var cnbServiceMock = CreateCnbServiceMock();
        var loggerMock = CreateLoggerMock();
        var provider = new ExchangeRateProvider(cnbServiceMock.Object, loggerMock.Object);

        var currencies = new[] { new Currency("USD") };

        cnbServiceMock
            .Setup(s => s.GetDailyRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Dictionary<string, decimal>?)null!);

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Empty(result);
    }
}

