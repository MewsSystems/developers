using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        exchangeRateService.Should().BeOfType<ExchangeRateService>();
    }
}
