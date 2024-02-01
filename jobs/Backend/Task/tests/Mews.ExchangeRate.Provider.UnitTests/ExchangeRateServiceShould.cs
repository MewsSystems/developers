using Mews.ExchangeRate.Domain.Models;
using Mews.ExchangeRate.Storage.Abstractions;
using Mews.ExchangeRate.Updater.Services;
using Mews.ExchangeRate.Updater.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Mews.ExchangeRate.Provider.UnitTests;

public class ExchangeRateServiceShould
{
    [Fact]
    public async Task ExchangeRateService_GetExchangeRateAsync_ReturnsExpectedRate()
    {
        var repositoryMock = new Mock<IExchangeRateQueryRepository>();
        repositoryMock
            .Setup(r => r.GetExchangeRateAsync(It.IsAny<Currency>()))
            .ReturnsAsync(new Domain.Models.ExchangeRate(new Currency("USD"), new Currency("EUR"), 1.2m));
        var service = new ExchangeRateService(
                                  Mock.Of<ILogger<ExchangeRateService>>(),
                                                                   repositoryMock.Object,
                                                                                                               Mock.Of<IExchangeRateUpdateService>()
                                                                                                                                                                  );
        var usdRate = await service.GetExchangeRateAsync(new Currency("USD"));

        Assert.NotNull(usdRate);
        Assert.Equal(new Currency("USD").ToString(), usdRate.SourceCurrency.ToString());
    }
}