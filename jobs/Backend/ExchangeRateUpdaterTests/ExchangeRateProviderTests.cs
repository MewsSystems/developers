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

namespace ExchangeRateUpdaterTests;

public class ExchangeRateProviderTests
{
    private Mock<IExchangeRateService> _exchangeRateServiceMock;
    private ExchangeRateProvider _exchangeRateProvider;

    [SetUp]
    public void Setup()
    {
        _exchangeRateServiceMock = new Mock<IExchangeRateService>();
        _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateServiceMock.Object);
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
    }

}
