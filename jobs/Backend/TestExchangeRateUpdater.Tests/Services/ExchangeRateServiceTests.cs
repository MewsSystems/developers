using ExchangeRateUpdater.Services;

namespace TestExchangeRateUpdater.Tests.Services;

[TestFixture]
public class ExchangeRateServiceTests
{
    [Test]
    public void ExchangeRateService_ShouldBe_InstanceOfExchangeRateService()
    {
        // Arrange
        var exchangeRateService = new ExchangeRateService();

        // Assert
        Assert.That(exchangeRateService, Is.InstanceOf<ExchangeRateService>());
    }
}
