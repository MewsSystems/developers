using Mews.ExchangeRate.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Text.Json;
using Xunit.Abstractions;

namespace Mews.ExchangeRate.Storage.DistributedCache.UnitTests;

public class ExchangeRateStorageQueryRepositorytShould
{
    private readonly ITestOutputHelper _output;

    public ExchangeRateStorageQueryRepositorytShould(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task GetCurrencyExchangeRatesAsync_WithNotValidDateOrLanguage_ThrowExchangeRateServiceResponseException()
    {
        var currency = Currency.Default;
        var expectedRate = new Domain.Models.ExchangeRate(currency, currency, 1);
        var rateJson = JsonSerializer.Serialize(expectedRate);
        var rateBytes = Encoding.UTF8.GetBytes(rateJson);

        var distributedCacheMock = new Mock<IDistributedCache>();
        distributedCacheMock
            .Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rateBytes)
            ;

        var repository = new ExchangeRateStorageQueryRepository(
            Mock.Of<ILogger<ExchangeRateStorageQueryRepository>>(),
            distributedCacheMock.Object
            );

        var rate = await repository.GetExchangeRateAsync(Currency.Default);

        Assert.Equal(expectedRate.ToString(), rate.ToString());
    }
}