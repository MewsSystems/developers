using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Models.API;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class ExchangeRateProviderTests
{
    private Mock<IExchangeRateService> _exchangeRateServiceMock;
    private Mock<ILogger<ExchangeRateProvider>> _loggerMock;
    private ExchangeRateProvider _exchangeRateProvider;

    [SetUp]
    public void SetUp()
    {
        _exchangeRateServiceMock = new Mock<IExchangeRateService>();
        _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
        _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetExchangeRates_WhenNoRatesFound_ReturnsEmptyList()
    {
        // Arrange
        var date = new DateTime(2025, 1, 13);
        var targetCurrency = new Currency("CZK", "koruna");
        var currencies = new List<Currency> { new Currency("EUR", "Euro") };

        _exchangeRateServiceMock
            .Setup(service => service.GetExchangeRatesAsync(date, "EN"))
            .ReturnsAsync(new ExchangeRatesResponseModel { Rates = [] });

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(date, targetCurrency, currencies);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetExchangeRates_WhenNoRatesFound_LogsInformation()
    {
        // Arrange
        var date = new DateTime(2025, 1, 13);
        var targetCurrency = new Currency("CZK", "koruna");
        var currencies = new List<Currency> { new Currency("EUR", "Euro") };

        _exchangeRateServiceMock
            .Setup(service => service.GetExchangeRatesAsync(date, "EN"))
            .ReturnsAsync(new ExchangeRatesResponseModel { Rates = [] });

        // Act
        await _exchangeRateProvider.GetExchangeRates(date, targetCurrency, currencies);

        // Assert
        _loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "No exchange rates for were found for 13/01/2025"),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Test]
    public async Task GetExchangeRates_WhenNoMatchingCurrencies_ReturnsEmptyList()
    {
        // Arrange
        var date = new DateTime(2025, 1, 13);
        var targetCurrency = new Currency("CZK", "koruna");
        var currencies = new List<Currency> { new Currency("EUR", "Euro") };
        var rates = new List<ExchangeRateResponseModel>
        {
            new ExchangeRateResponseModel
            {
                CurrencyCode = "GBP",
                Currency = "British Pound",
                Rate = 1.2m,
                Amount = 1
            }
        };

        _exchangeRateServiceMock
            .Setup(service => service.GetExchangeRatesAsync(date, "EN"))
            .ReturnsAsync(new ExchangeRatesResponseModel { Rates = rates });

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(date, targetCurrency, currencies);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetExchangeRates_WhenNoMatchingCurrencies_LogsInformation()
    {
        // Arrange
        var date = new DateTime(2025, 1, 13);
        var targetCurrency = new Currency("CZK", "koruna");
        var currencies = new List<Currency> { new Currency("EUR", "Euro") };
        var rates = new List<ExchangeRateResponseModel>
        {
            new ExchangeRateResponseModel
            {
                CurrencyCode = "GBP",
                Currency = "British Pound",
                Rate = 1.2m,
                Amount = 1
            }
        };

        _exchangeRateServiceMock
            .Setup(service => service.GetExchangeRatesAsync(date, "EN"))
            .ReturnsAsync(new ExchangeRatesResponseModel { Rates = rates });

        // Act
        await _exchangeRateProvider.GetExchangeRates(date, targetCurrency, currencies);

        // Assert
        _loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "No exchange rates for specified currencies were found for 13/01/2025"),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Test]
    public async Task GetExchangeRates_WhenMatchingCurrencies_ReturnsExchangeRates()
    {
        // Arrange
        var date = new DateTime(2025, 1, 13);
        var targetCurrency = new Currency("CZK", "koruna");
        var currencies = new List<Currency> { new Currency("EUR", "Euro") };
        var rates = new List<ExchangeRateResponseModel>
        {
            new ExchangeRateResponseModel
            {
                CurrencyCode = "EUR",
                Currency = "Euro",
                Rate = 1.1m,
                Amount = 1
            }
        };

        var expected = new List<ExchangeRate>
        {
            new ExchangeRate(currencies.First(), targetCurrency, 1.1m)
        };

        _exchangeRateServiceMock
            .Setup(service => service.GetExchangeRatesAsync(date, "EN"))
            .ReturnsAsync(new ExchangeRatesResponseModel { Rates = rates });

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(date, targetCurrency, currencies);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetExchangeRates_WhenRateAmountIsNotOne_ReturnsExchangeRates()
    {
        // Arrange
        var date = new DateTime(2025, 1, 13);
        var targetCurrency = new Currency("CZK", "koruna");
        var currencies = new List<Currency> { new Currency("TRY", "Turkish Lira") };
        var rates = new List<ExchangeRateResponseModel>
        {
            new ExchangeRateResponseModel
            {
                CurrencyCode = "TRY",
                Currency = "Turkish Lira",
                Rate = 68m,
                Amount = 100
            }
        };
        
        var expected = new List<ExchangeRate>
        {
            new ExchangeRate(currencies.First(), targetCurrency, 0.68m)
        };
        
        _exchangeRateServiceMock
            .Setup(service => service.GetExchangeRatesAsync(date, "EN"))
            .ReturnsAsync(new ExchangeRatesResponseModel { Rates = rates });
        
        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(date, targetCurrency, currencies);
        
        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetExchangeRates_WhenMultipleMatchingCurrencies_ReturnsExchangeRates()
    {
        // Arrange
        var date = new DateTime(2025, 1, 13);
        var targetCurrency = new Currency("CZK", "koruna");
        
        var currencies = new List<Currency>
        {
            new Currency("EUR", "Euro"),
            new Currency("GBP", "British Pound")
        };

        var rates = new List<ExchangeRateResponseModel>
        {
            new ExchangeRateResponseModel
            {
                CurrencyCode = "EUR",
                Currency = "Euro",
                Rate = 1.1m,
                Amount = 1
            },
            new ExchangeRateResponseModel
            {
                CurrencyCode = "GBP",
                Currency = "British Pound",
                Rate = 1.3m,
                Amount = 1
            }
        };

        var expected = new List<ExchangeRate>
        {
            new ExchangeRate(currencies.First(), targetCurrency, 1.1m),
            new ExchangeRate(currencies.Last(), targetCurrency, 1.3m)
        };

        _exchangeRateServiceMock
            .Setup(service => service.GetExchangeRatesAsync(date, "EN"))
            .ReturnsAsync(new ExchangeRatesResponseModel { Rates = rates });

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(date, targetCurrency, currencies);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetExchangeRates_WhenMatchingCurrencies_LogsInformation()
    {
        // Arrange
        var date = new DateTime(2025, 1, 13);
        var targetCurrency = new Currency("CZK", "koruna");
        
        var currencies = new List<Currency>
        {
            new Currency("EUR", "Euro"),
            new Currency("GBP", "British Pound")
        };
        
        var rates = new List<ExchangeRateResponseModel>
        {
            new ExchangeRateResponseModel
            {
                CurrencyCode = "EUR",
                Currency = "Euro",
                Rate = 1.1m,
                Amount = 1
            },
            new ExchangeRateResponseModel
            {
                CurrencyCode = "GBP",
                Currency = "British Pound",
                Rate = 1.3m,
                Amount = 1
            }
        };

        _exchangeRateServiceMock
            .Setup(service => service.GetExchangeRatesAsync(date, "EN"))
            .ReturnsAsync(new ExchangeRatesResponseModel { Rates = rates });

        // Act
        await _exchangeRateProvider.GetExchangeRates(date, targetCurrency, currencies);

        // Assert
        _loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "2 exchange rates found for specified currencies"),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}
