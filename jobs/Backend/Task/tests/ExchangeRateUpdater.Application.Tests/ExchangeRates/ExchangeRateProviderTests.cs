using ExchangeRateUpdater.Application.Clients;
using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Application.ExchangeRates;
using ExchangeRateUpdater.Application.Models;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace ExchangeRateUpdater.Application.Tests.ExchangeRates;

public class ExchangeRateProviderTests
{
    private readonly ICzbExchangeRateClient _czbExchangeRateClient;
    private readonly IMemoryCache _memoryCache;
    private readonly ExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateProviderTests()
    {
        _czbExchangeRateClient = Substitute.For<ICzbExchangeRateClient>();
        _memoryCache = Substitute.For<IMemoryCache>();
        _exchangeRateProvider = new ExchangeRateProvider(_czbExchangeRateClient, _memoryCache);
    }

    [Fact]
    public async Task GetExchangeRates_ShouldReturnExchangeRatesFromCache()
    {
        // Arrange
        var cachedRates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 1.2m, 100, "2022-05-01"),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 1.5m, 150, "2022-05-01")
        };

        _memoryCache.TryGetValue("ExchangeRates", out Arg.Any<IEnumerable<ExchangeRate>>()).Returns(true).AndDoes(x =>
        {
            x[1] = cachedRates;
        });

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(new[] { new Currency("USD"), new Currency("EUR") });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(cachedRates);
    }

    [Fact]
    public async Task GetExchangeRates_ShouldFetchAndCacheExchangeRates()
    {
        // Arrange
        var fetchedRatesUSD = new List<ExchangeRate>
            {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 1.2m, 100, "2022-05-01"),
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 1.5m, 100, "2022-05-02"),
            };

        _czbExchangeRateClient.GetExchangeRate(Arg.Any<string>())
            .Returns(Result<IEnumerable<ExchangeRate>>.Success(fetchedRatesUSD));

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(new[] { new Currency("USD"), new Currency("EUR") });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(fetchedRatesUSD);

        // Ensure rates are cached
        _memoryCache.Received(1).Set("ExchangeRates", fetchedRatesUSD, TimeSpan.FromHours(24));
    }
}