using ExchangeRate.Application;
using ExchangeRate.Application.Services;
using ExchangeRate.Domain;
using ExchangeRate.Infrastructure.Cnb.Mappers;
using FluentAssertions;
using Moq;

namespace ExchangeRate.Tests.Application;

public class ExchangeRateProviderTests : TestsBase
{
    private Mock<IExchangeRatesService> _mockExchangeRatesService = null!;
    private ExchangeRateProvider _exchangeRateProvider = null!;
    
    [SetUp]
    public void Setup()
    {
        _mockExchangeRatesService = new Mock<IExchangeRatesService>();
        _exchangeRateProvider = new ExchangeRateProvider(_mockExchangeRatesService.Object);
    }
    
    [Test]
    public async Task GetExchangeRate_ReturnsCorrectExchangeRate()
    {
        // Arrange
        var exchangeRates = GenerateExchangeRates(DateTime.Today, 3).ToDomain().ToList();
        var expectedRate = exchangeRates.First();
        var requestedCurrency = new Currency(expectedRate.SourceCurrency.Code);
        
        _mockExchangeRatesService.Setup(x => x.GetCurrentExchangeRates()).ReturnsAsync(exchangeRates);

        // Act
        var result = await _exchangeRateProvider.GetExchangeRate(requestedCurrency);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedRate);
    }
    
    [Test]
    public async Task GetExchangeRate_ReturnsNullWhenNoRateFound()
    {
        // Arrange
        var exchangeRates = GenerateExchangeRates(DateTime.Today, 3).ToDomain().ToList();
        var requestedCurrency = new Currency("XXX"); //non existing currency
        
        _mockExchangeRatesService.Setup(x => x.GetCurrentExchangeRates()).ReturnsAsync(exchangeRates);

        // Act
        var result = await _exchangeRateProvider.GetExchangeRate(requestedCurrency);

        // Assert
        result.Should().BeNull();
    }
}