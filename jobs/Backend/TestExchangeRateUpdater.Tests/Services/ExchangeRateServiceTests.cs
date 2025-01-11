using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Services;

namespace TestExchangeRateUpdater.Tests.Services;

[TestFixture]
public class ExchangeRateServiceTests
{
    [Test]
    public void CallingGetExchangeRates_ShouldReturn_ExchangeRatesDTO()
    {
        // Arrange
        var exchangeRateService = new ExchangeRateService();

        // Act
        var actual = exchangeRateService.GetExchangeRates();

        // Assert
        Assert.That(actual, Is.InstanceOf<ExchangeRatesDTO>());
    }
}
