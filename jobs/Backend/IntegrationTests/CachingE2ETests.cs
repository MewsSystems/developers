using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.IntegrationTests;

/// <summary>
/// End-to-end tests specifically focused on caching behavior
/// with real MemoryCache and real CNB API integration
/// </summary>
public class CachingE2ETests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ExchangeRateProvider _provider;
    private readonly IExchangeRateCache _cache;

    public CachingE2ETests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        services.AddExchangeRateProvider(configuration);

        _serviceProvider = services.BuildServiceProvider();
        _provider = _serviceProvider.GetRequiredService<ExchangeRateProvider>();
        _cache = _serviceProvider.GetRequiredService<IExchangeRateCache>();
    }

    [Fact]
    public async Task CacheMissFollowedByCacheHit_WorksCorrectly()
    {
        // Arrange
        _cache.Clear(); // Ensure clean state
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };

        // Act - First call should be cache miss (fetch from API)
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var firstCallRates = await _provider.GetExchangeRatesAsync(currencies);
        var firstCallTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Restart();

        // Second call should be cache hit (much faster)
        var secondCallRates = await _provider.GetExchangeRatesAsync(currencies);
        var secondCallTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Stop();

        var firstList = firstCallRates.ToList();
        var secondList = secondCallRates.ToList();

        // Assert
        firstList.Should().HaveCount(2);
        secondList.Should().HaveCount(2);

        // Results should be identical
        for (int i = 0; i < firstList.Count; i++)
        {
            firstList[i].SourceCurrency.Code.Should().Be(secondList[i].SourceCurrency.Code);
            firstList[i].Value.Should().Be(secondList[i].Value);
        }

        // Second call should be significantly faster (cache hit)
        secondCallTime.Should().BeLessThan(firstCallTime / 2,
            "cached call should be at least 2x faster than API call");
    }

    [Fact]
    public async Task DifferentCurrencySets_HaveSeparateCacheEntries()
    {
        // Arrange
        _cache.Clear();
        var usdOnly = new[] { new Currency("USD") };
        var eurOnly = new[] { new Currency("EUR") };
        var both = new[] { new Currency("USD"), new Currency("EUR") };

        // Act
        var usdRates = await _provider.GetExchangeRatesAsync(usdOnly);
        var eurRates = await _provider.GetExchangeRatesAsync(eurOnly);
        var bothRates = await _provider.GetExchangeRatesAsync(both);

        // Assert
        usdRates.Should().HaveCount(1).And.Contain(r => r.SourceCurrency.Code == "USD");
        eurRates.Should().HaveCount(1).And.Contain(r => r.SourceCurrency.Code == "EUR");
        bothRates.Should().HaveCount(2);
    }

    [Fact]
    public async Task CacheOrderIndependence_SameSetDifferentOrder_HitsCache()
    {
        // Arrange
        _cache.Clear();
        var order1 = new[] { new Currency("USD"), new Currency("EUR"), new Currency("GBP") };
        var order2 = new[] { new Currency("GBP"), new Currency("USD"), new Currency("EUR") };

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var firstCall = await _provider.GetExchangeRatesAsync(order1);
        var firstCallTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Restart();

        var secondCall = await _provider.GetExchangeRatesAsync(order2);
        var secondCallTime = stopwatch.ElapsedMilliseconds;

        // Assert
        secondCallTime.Should().BeLessThan(firstCallTime / 2,
            "second call with different order should hit cache");

        var firstCodes = firstCall.Select(r => r.SourceCurrency.Code).OrderBy(c => c).ToList();
        var secondCodes = secondCall.Select(r => r.SourceCurrency.Code).OrderBy(c => c).ToList();
        firstCodes.Should().BeEquivalentTo(secondCodes);
    }

    [Fact]
    public async Task CacheClear_RemovesAllCachedData()
    {
        // Arrange
        _cache.Clear();
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };

        // Populate cache
        await _provider.GetExchangeRatesAsync(currencies);

        // Verify cache hit
        var cachedData = _cache.GetCachedRates(currencies.Select(c => c.Code));
        cachedData.Should().NotBeNull("data should be cached");

        // Act
        _cache.Clear();

        // Assert
        var afterClear = _cache.GetCachedRates(currencies.Select(c => c.Code));
        afterClear.Should().BeNull("cache should be empty after clear");
    }

    [Fact]
    public async Task ConcurrentRequests_BothHitCache()
    {
        // Arrange
        _cache.Clear();
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };

        // First call to populate cache
        await _provider.GetExchangeRatesAsync(currencies);

        // Act - Make concurrent requests
        var tasks = Enumerable.Range(0, 5)
            .Select(_ => _provider.GetExchangeRatesAsync(currencies))
            .ToList();

        var results = await Task.WhenAll(tasks);

        // Assert
        results.Should().HaveCount(5);
        foreach (var result in results)
        {
            result.Should().HaveCount(2);
            result.Should().OnlyContain(r => r.Value > 0);
        }

        // All results should be identical (from cache)
        var firstResult = results[0].ToList();
        foreach (var result in results.Skip(1))
        {
            var resultList = result.ToList();
            for (int i = 0; i < firstResult.Count; i++)
            {
                resultList[i].Value.Should().Be(firstResult[i].Value);
            }
        }
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}
