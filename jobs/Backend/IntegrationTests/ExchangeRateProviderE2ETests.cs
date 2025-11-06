using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.IntegrationTests;

/// <summary>
/// End-to-end tests that verify the entire exchange rate provider workflow
/// with real dependencies (actual CNB API calls, real caching, etc.)
/// </summary>
public class ExchangeRateProviderE2ETests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ExchangeRateProvider _provider;

    public ExchangeRateProviderE2ETests()
    {
        // Build configuration from test settings
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();

        // Build real DI container with all services
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });
        services.AddExchangeRateProvider(configuration);

        _serviceProvider = services.BuildServiceProvider();
        _provider = _serviceProvider.GetRequiredService<ExchangeRateProvider>();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithRealCnbApi_ReturnsValidRates()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("GBP")
        };

        // Act
        var rates = await _provider.GetExchangeRatesAsync(currencies);
        var ratesList = rates.ToList();

        // Assert
        ratesList.Should().HaveCount(3, "all three currencies should be returned");
        ratesList.Should().OnlyContain(r => r.TargetCurrency.Code == "CZK", "all rates should be to CZK");
        ratesList.Should().OnlyContain(r => r.Value > 0, "all exchange rates should be positive");

        // Verify specific currencies
        ratesList.Should().Contain(r => r.SourceCurrency.Code == "USD");
        ratesList.Should().Contain(r => r.SourceCurrency.Code == "EUR");
        ratesList.Should().Contain(r => r.SourceCurrency.Code == "GBP");

        // Verify realistic rate values (sanity check)
        var usdRate = ratesList.First(r => r.SourceCurrency.Code == "USD");
        usdRate.Value.Should().BeInRange(15m, 35m, "USD/CZK rate should be realistic");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithMultipleCalls_ReturnsConsistentResults()
    {
        // Arrange
        var currencies = new[] { new Currency("EUR") };

        // Act - Call twice in succession
        var firstCall = await _provider.GetExchangeRatesAsync(currencies);
        var secondCall = await _provider.GetExchangeRatesAsync(currencies);

        var firstRate = firstCall.First();
        var secondRate = secondCall.First();

        // Assert - Results should be identical (from cache)
        firstRate.SourceCurrency.Code.Should().Be(secondRate.SourceCurrency.Code);
        firstRate.TargetCurrency.Code.Should().Be(secondRate.TargetCurrency.Code);
        firstRate.Value.Should().Be(secondRate.Value);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithAllSupportedCurrencies_ReturnsAllRates()
    {
        // Arrange - Test with a comprehensive list of commonly supported currencies
        var currencies = new[]
        {
            new Currency("USD"), new Currency("EUR"), new Currency("GBP"),
            new Currency("JPY"), new Currency("CHF"), new Currency("AUD"),
            new Currency("CAD"), new Currency("SEK"), new Currency("NOK"),
            new Currency("DKK"), new Currency("PLN"), new Currency("HUF")
        };

        // Act
        var rates = await _provider.GetExchangeRatesAsync(currencies);
        var ratesList = rates.ToList();

        // Assert
        ratesList.Should().NotBeEmpty("CNB should provide rates for major currencies");
        ratesList.Should().OnlyContain(r => r.Value > 0);
        ratesList.Should().OnlyContain(r => r.TargetCurrency.Code == "CZK");

        // Log which currencies were found for debugging
        var foundCurrencies = ratesList.Select(r => r.SourceCurrency.Code).ToList();
        foundCurrencies.Should().Contain(new[] { "USD", "EUR" }, "USD and EUR should always be available");
    }

    [Fact]
    public void GetExchangeRates_Synchronous_WithRealApi_ReturnsValidRates()
    {
        // Arrange
        var currencies = new[] { new Currency("EUR"), new Currency("USD") };

        // Act
        var rates = _provider.GetExchangeRates(currencies);
        var ratesList = rates.ToList();

        // Assert
        ratesList.Should().HaveCount(2);
        ratesList.Should().OnlyContain(r => r.Value > 0);
        ratesList.Should().OnlyContain(r => r.TargetCurrency.Code == "CZK");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithInvalidCurrency_ReturnsEmpty()
    {
        // Arrange - Use a currency code that CNB definitely doesn't support
        var currencies = new[] { new Currency("XXX"), new Currency("INVALID") };

        // Act
        var rates = await _provider.GetExchangeRatesAsync(currencies);
        var ratesList = rates.ToList();

        // Assert
        ratesList.Should().BeEmpty("invalid currency codes should return no results");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithMixedValidAndInvalid_ReturnsOnlyValid()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency("USD"),    // Valid
            new Currency("XXX"),    // Invalid
            new Currency("EUR"),    // Valid
            new Currency("FAKE")    // Invalid
        };

        // Act
        var rates = await _provider.GetExchangeRatesAsync(currencies);
        var ratesList = rates.ToList();

        // Assert
        ratesList.Should().HaveCount(2, "only valid currencies should be returned");
        ratesList.Should().Contain(r => r.SourceCurrency.Code == "USD");
        ratesList.Should().Contain(r => r.SourceCurrency.Code == "EUR");
        ratesList.Should().NotContain(r => r.SourceCurrency.Code == "XXX");
        ratesList.Should().NotContain(r => r.SourceCurrency.Code == "FAKE");
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}
