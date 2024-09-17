using Mews.ExchangeRate.Http.Abstractions;
using Mews.ExchangeRate.Storage.Abstractions;
using Mews.ExchangeRate.Updater.Services;
using Mews.ExchangeRate.Updater.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Mews.ExchangeRate.Updater.UnitTests;

public class ExchangeRateUpdateServiceShould
{
    [Fact]
    public async Task ExchangeRateUpdateService_RefreshAllExchangeRatesAsync_ReturnsTrue()
    {
        var clockMock = new Mock<IClock>();
        clockMock.Setup(c=>c.UtcNow).Returns(DateTime.UtcNow);

        var service = new ExchangeRateUpdateService(
                       Mock.Of<ILogger<ExchangeRateUpdateService>>(),
                                  clockMock.Object,
                                             Mock.Of<IExchangeRateCommandRepository>(),
                                                        Mock.Of<IExchangeRateServiceClient>()
                                                               );

        await service.RefreshAllExchangeRatesAsync();

        clockMock.Verify(c => c.UtcNow, Times.Once);
    }
}