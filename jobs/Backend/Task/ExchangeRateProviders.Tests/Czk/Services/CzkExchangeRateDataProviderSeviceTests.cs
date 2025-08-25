using ExchangeRateProviders.Czk.Clients;
using ExchangeRateProviders.Czk.Model;
using ExchangeRateProviders.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using ZiggyCreatures.Caching.Fusion;
using ExchangeRateProviders.Czk.Config;
using ExchangeRateProviders.Czk;

namespace ExchangeRateProviders.Tests.Czk.Services;

[TestFixture]
public class CzkExchangeRateDataProviderSeviceTests
{
    [Test]
    public async Task GetDailyRatesAsync_FirstCall_FetchesAndMaps()
    {
        // Arrange
        var cache = new FusionCache(new FusionCacheOptions());
        var apiClient = Substitute.For<ICzkCnbApiClient>();
        var logger = Substitute.For<ILogger<CzkExchangeRateDataProvider>>();
        var service = new CzkExchangeRateDataProvider(cache, apiClient, logger);

        var raw = new List<CnbApiExchangeRateDto>
        {
            new() { CurrencyCode = "USD", Amount = 1, Rate = 22.50m, ValidFor = DateTime.UtcNow },
            new() { CurrencyCode = "EUR", Amount = 1, Rate = 24.00m, ValidFor = DateTime.UtcNow }
        };

        apiClient.GetDailyRatesRawAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CnbApiExchangeRateDto>>(raw));

        // Act
        var result = (await service.GetDailyRatesAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].SourceCurrency.Code, Is.EqualTo("USD"));
            Assert.That(result[1].SourceCurrency.Code, Is.EqualTo("EUR"));
            apiClient.Received(1).GetDailyRatesRawAsync(Arg.Any<CancellationToken>());
            logger.VerifyLogInformation(1, "Cache miss for CNB daily rates. Fetching and mapping.");
            logger.VerifyLogInformation(1, $"Mapped 2 CNB exchange rates (target currency {Constants.ExchangeRateProviderCurrencyCode}).");
        });
    }

    [Test]
    public async Task GetDailyRatesAsync_SubsequentCall_UsesCache()
    {
        // Arrange
        var cache = new FusionCache(new FusionCacheOptions());
        var apiClient = Substitute.For<ICzkCnbApiClient>();
        var logger = Substitute.For<ILogger<CzkExchangeRateDataProvider>>();
        var service = new CzkExchangeRateDataProvider(cache, apiClient, logger);

        var raw = new List<CnbApiExchangeRateDto>
        {
            new() { CurrencyCode = "JPY", Amount = 100, Rate = 17.00m, ValidFor = DateTime.UtcNow }
        };

        apiClient.GetDailyRatesRawAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CnbApiExchangeRateDto>>(raw));

        // Act
        var first = (await service.GetDailyRatesAsync()).ToList();
        var second = (await service.GetDailyRatesAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(first, Is.EqualTo(second)); // same cached content
            Assert.That(first.Single().SourceCurrency.Code, Is.EqualTo("JPY"));
            Assert.That(first.Single().Value, Is.EqualTo(0.17m));
            apiClient.Received(1).GetDailyRatesRawAsync(Arg.Any<CancellationToken>()); // only once due to caching
            logger.VerifyLogInformation(1, "Cache miss for CNB daily rates. Fetching and mapping.");
            logger.VerifyLogInformation(1, $"Mapped 1 CNB exchange rates (target currency {Constants.ExchangeRateProviderCurrencyCode}).");
        });
    }
}
