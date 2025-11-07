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

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}
