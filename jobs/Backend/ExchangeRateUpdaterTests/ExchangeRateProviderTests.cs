using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.DTOs;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdaterTests;

public class ExchangeRateProviderTests
{
    private Mock<IExchangeRateService> _exchangeRateServiceMock;
    private Mock<ILogger<ExchangeRateProvider>> _loggerMock;
    private ExchangeRateProvider _exchangeRateProvider;

    [SetUp]
    public void Setup()
    {
        _exchangeRateServiceMock = new Mock<IExchangeRateService>();
        _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
        _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetExchangeRatesAsync_ShouldReturnEmptyList_WhenServiceReturnsNull()
    {
        // Arrange
        _exchangeRateServiceMock.Setup(s => s.GetExchangeRateListAsync()).ReturnsAsync((ExchangeRateListDto)null);

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(new List<Currency>());

        // Assert
        result.Should().BeEmpty();
        _loggerMock.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("No exchange rates available from the service.")), null, It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Test]
    public async Task GetExchangeRatesAsync_ShouldReturnEmptyList_WhenServiceReturnsNullRates()
    {
        // Arrange
        _exchangeRateServiceMock.Setup(s => s.GetExchangeRateListAsync()).ReturnsAsync(new ExchangeRateListDto { Rates = null });

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(new List<Currency>());

        // Assert
        result.Should().BeEmpty();
        _loggerMock.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("No exchange rates available from the service.")), null, It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnFilteredRates_WhenCurrenciesMatch()
    {
        var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
        var exchangeRatesDto = new ExchangeRateListDto
        {
            Rates = new List<ExchangeRateDto>
            {
                new ExchangeRateDto { CurrencyCode = "USD", Rate = 1.0m, Amount = 1 },
                new ExchangeRateDto { CurrencyCode = "EUR", Rate = 1.2m, Amount = 1 },
                new ExchangeRateDto { CurrencyCode = "GBP", Rate = 1.4m, Amount = 1 }
            }
        };
        _exchangeRateServiceMock.Setup(s => s.GetExchangeRateListAsync()).ReturnsAsync(exchangeRatesDto);

        var result = await _exchangeRateProvider.GetExchangeRates(currencies);

        result.Should().HaveCount(2);
        result.Should().Contain(rate =>
        rate.SourceCurrency.Code == "USD" && rate.TargetCurrency.Code == "CZK" && rate.Value == 1.0m);
        result.Should().Contain(rate =>
            rate.SourceCurrency.Code == "EUR" && rate.TargetCurrency.Code == "CZK" && rate.Value == 1.2m);
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnEmptyList_WhenNoCurrenciesMatch()
    {
        var currencies = new List<Currency> { new Currency("JPY") };
        var exchangeRatesDto = new ExchangeRateListDto
        {
            Rates = new List<ExchangeRateDto>
            {
                new ExchangeRateDto { CurrencyCode = "USD", Rate = 1.0m, Amount = 1 },
                new ExchangeRateDto { CurrencyCode = "EUR", Rate = 1.2m, Amount = 1 }
            }
        };
        _exchangeRateServiceMock.Setup(s => s.GetExchangeRateListAsync()).ReturnsAsync(exchangeRatesDto);

        var result = await _exchangeRateProvider.GetExchangeRates(currencies);

        result.Should().BeEmpty();
        _loggerMock.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("No matching rates found for chosen currency codes.")), null, It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);

    }

}
