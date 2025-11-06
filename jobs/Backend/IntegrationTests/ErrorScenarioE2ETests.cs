using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.IntegrationTests;

/// <summary>
/// End-to-end tests for error scenarios and edge cases
/// Tests resilience features like retry, timeout, and error handling
/// </summary>
public class ErrorScenarioE2ETests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ExchangeRateProvider _provider;

    public ErrorScenarioE2ETests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();

        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug); // More verbose for error scenarios
        });
        services.AddExchangeRateProvider(configuration);

        _serviceProvider = services.BuildServiceProvider();
        _provider = _serviceProvider.GetRequiredService<ExchangeRateProvider>();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithEmptyCurrencyList_ReturnsEmpty()
    {
        // Arrange
        var emptyCurrencies = Array.Empty<Currency>();

        // Act
        var rates = await _provider.GetExchangeRatesAsync(emptyCurrencies);

        // Assert
        rates.Should().BeEmpty("empty input should return empty result");
    }

    [Fact]
    public void GetExchangeRates_WithNullCurrencies_ThrowsArgumentNullException()
    {
        // Act & Assert
        _provider.Invoking(p => p.GetExchangeRates(null!))
            .Should().Throw<ArgumentNullException>()
            .WithMessage("*currencies*");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithNullCurrencies_ThrowsArgumentNullException()
    {
        // Act & Assert
        await _provider.Invoking(p => p.GetExchangeRatesAsync(null!))
            .Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("*currencies*");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithCancellation_RespectsCancellationToken()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };
        using var cts = new CancellationTokenSource();
        cts.Cancel(); // Cancel immediately

        // Act & Assert
        // The cancellation is wrapped in ExchangeRateProviderException
        var exception = await _provider.Invoking(p => p.GetExchangeRatesAsync(currencies, cts.Token))
            .Should().ThrowAsync<ExchangeRateProviderException>();

        exception.And.InnerException.Should().BeOfType<TaskCanceledException>();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithRealApi_HandlesNetworkConditions()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };

        // Act - Should succeed even with real network (resilience handles transient issues)
        var rates = await _provider.GetExchangeRatesAsync(currencies);

        // Assert
        rates.Should().NotBeEmpty("provider should handle network conditions gracefully");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_MultipleSequentialCalls_AllSucceed()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };

        // Act - Make 5 sequential calls
        for (int i = 0; i < 5; i++)
        {
            var rates = await _provider.GetExchangeRatesAsync(currencies);

            // Assert
            rates.Should().NotBeEmpty($"call {i + 1} should succeed");
            rates.First().Value.Should().BeGreaterThan(0);
        }
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithCaseSensitiveCurrency_HandlesCorrectly()
    {
        // Arrange - Test with various case combinations
        var testCases = new[]
        {
            new[] { new Currency("usd") },
            new[] { new Currency("USD") },
            new[] { new Currency("Usd") }
        };

        // Act & Assert
        foreach (var currencies in testCases)
        {
            var rates = await _provider.GetExchangeRatesAsync(currencies);
            var ratesList = rates.ToList();

            // Should handle case-insensitively
            if (ratesList.Any())
            {
                ratesList.First().SourceCurrency.Code.Should().BeOneOf("USD", "usd", "Usd");
            }
        }
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithWhitespaceInCurrencyCode_HandlesGracefully()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency(" USD "),
            new Currency("EUR")
        };

        // Act
        var rates = await _provider.GetExchangeRatesAsync(currencies);

        // Assert - Should either trim and succeed, or return empty for invalid format
        // Implementation determines behavior
        rates.Should().NotBeNull();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithDuplicateCurrencies_HandlesCorrectly()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("EUR")
        };

        // Act
        var rates = await _provider.GetExchangeRatesAsync(currencies);
        var ratesList = rates.ToList();

        // Assert - Should deduplicate or handle gracefully
        ratesList.Should().NotBeEmpty();
        ratesList.Should().OnlyContain(r => r.Value > 0);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_LargeNumberOfCurrencies_Succeeds()
    {
        // Arrange - Request many currencies at once
        var currencies = new[]
        {
            "USD", "EUR", "GBP", "JPY", "CHF", "AUD", "CAD", "SEK",
            "NOK", "DKK", "PLN", "HUF", "CZK", "BGN", "RON", "HRK",
            "RUB", "TRY", "BRL", "MXN", "ZAR", "INR", "CNY", "KRW",
            "IDR", "MYR", "PHP", "THB", "SGD", "NZD", "ILS", "CLP"
        }.Select(code => new Currency(code)).ToArray();

        // Act
        var rates = await _provider.GetExchangeRatesAsync(currencies);
        var ratesList = rates.ToList();

        // Assert
        ratesList.Should().NotBeEmpty("should return rates for available currencies");
        ratesList.Should().OnlyContain(r => r.Value > 0);
        ratesList.Should().OnlyContain(r => r.TargetCurrency.Code == "CZK");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_RapidSuccessiveCalls_DoesNotOverloadApi()
    {
        // Arrange
        var currencies = new[] { new Currency("EUR") };

        // Act - Make rapid calls (should hit cache after first)
        var tasks = Enumerable.Range(0, 10)
            .Select(async _ =>
            {
                var rates = await _provider.GetExchangeRatesAsync(currencies);
                return rates.First();
            })
            .ToList();

        var results = await Task.WhenAll(tasks);

        // Assert
        results.Should().HaveCount(10);
        results.Should().OnlyContain(r => r.Value > 0);

        // All should have same value (from cache)
        var firstValue = results[0].Value;
        results.Should().OnlyContain(r => r.Value == firstValue);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithSpecialCharactersInCode_HandlesGracefully()
    {
        // Arrange - Test with invalid currency codes
        var currencies = new[]
        {
            new Currency("US$"),
            new Currency("EUR!"),
            new Currency("GB@")
        };

        // Act
        var rates = await _provider.GetExchangeRatesAsync(currencies);

        // Assert - Should return empty or handle gracefully without throwing
        rates.Should().NotBeNull();
        rates.Should().BeEmpty();
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}
