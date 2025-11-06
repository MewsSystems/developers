using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using ApplicationLayer.Queries.SystemHealth.GetSystemHealth;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Queries.SystemHealth;

/// <summary>
/// Integration tests for GetSystemHealthQuery.
/// Tests system health metrics aggregation from database views.
/// </summary>
public class GetSystemHealthQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrency(string code)
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        return result.Value;
    }

    private async Task<int> CreateTestProvider(string name, string code, int baseCurrencyId, bool isActive = true)
    {
        var command = new CreateExchangeRateProviderCommand(
            Name: name,
            Code: code.ToUpper(),
            Url: $"https://example-{code.ToLower()}.com",
            BaseCurrencyId: baseCurrencyId,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );

        var result = await Mediator.Send(command);

        // Get the actual ID from the database
        var provider = await DbContext.ExchangeRateProviders
            .FirstOrDefaultAsync(p => p.Code == code.ToUpper());

        // Update IsActive if needed
        if (provider != null && !isActive)
        {
            provider.IsActive = false;
            await DbContext.SaveChangesAsync();
        }

        return provider?.Id ?? result.Value;
    }

    private async Task CreateExchangeRates(int providerId, DateOnly validDate, params (string target, decimal rate)[] rates)
    {
        var rateItems = rates.Select(r => new ExchangeRateItemDto("EUR", r.target, r.rate, 1)).ToList();

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: providerId,
            ValidDate: validDate,
            Rates: rateItems
        );

        await Mediator.Send(command);
    }

    private async Task CreateFetchLog(int providerId, DateOnly fetchDate, string status, int ratesImported = 0, int ratesUpdated = 0, string? errorMessage = null)
    {
        var fetchStarted = new DateTimeOffset(fetchDate.ToDateTime(new TimeOnly(0, 0)), TimeSpan.Zero);
        var fetchCompleted = fetchStarted.AddSeconds(5);

        var fetchLog = new DataLayer.Entities.ExchangeRateFetchLog
        {
            ProviderId = providerId,
            FetchStarted = fetchStarted,
            FetchCompleted = fetchCompleted,
            Status = status,
            RatesImported = ratesImported,
            RatesUpdated = ratesUpdated,
            ErrorMessage = errorMessage,
            DurationMs = 5000
        };

        DbContext.Set<DataLayer.Entities.ExchangeRateFetchLog>().Add(fetchLog);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetSystemHealth_WithNoData_ShouldReturnEmptyMetrics()
    {
        // Arrange
        var query = new GetSystemHealthQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.TotalProviders.Should().Be(0);
        result.ActiveProviders.Should().Be(0);
        result.QuarantinedProviders.Should().Be(0);
        result.TotalCurrencies.Should().Be(0);
        result.TotalExchangeRates.Should().Be(0);
        result.LastUpdated.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetSystemHealth_WithProviders_ShouldReturnCorrectProviderCounts()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");

        await CreateTestProvider("ECB", "ECB", eurId, isActive: true);
        await CreateTestProvider("CNB", "CNB", eurId, isActive: true);
        await CreateTestProvider("BNR", "BNR", eurId, isActive: false);

        var query = new GetSystemHealthQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.TotalProviders.Should().Be(3);
        result.ActiveProviders.Should().Be(2);
    }

    [Fact]
    public async Task GetSystemHealth_WithCurrencies_ShouldReturnCorrectCurrencyCount()
    {
        // Arrange
        await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");
        await CreateTestCurrency("GBP");
        await CreateTestCurrency("JPY");

        var query = new GetSystemHealthQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.TotalCurrencies.Should().Be(4);
    }

    [Fact]
    public async Task GetSystemHealth_WithExchangeRates_ShouldReturnCorrectRateCount()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");
        await CreateTestCurrency("GBP");
        await CreateTestCurrency("JPY");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await CreateExchangeRates(providerId, today, ("USD", 1.20m), ("GBP", 0.85m), ("JPY", 130.50m));

        var query = new GetSystemHealthQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.TotalExchangeRates.Should().Be(3);
    }

    [Fact]
    public async Task GetSystemHealth_WithFetchLogs_ShouldReturnCorrectFetchMetrics()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Create fetch logs for today
        await CreateFetchLog(providerId, today, "Success", ratesImported: 10, ratesUpdated: 5);
        await CreateFetchLog(providerId, today, "Success", ratesImported: 8, ratesUpdated: 2);
        await CreateFetchLog(providerId, today, "Failed", errorMessage: "Connection timeout");

        var query = new GetSystemHealthQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.TotalFetchesToday.Should().Be(3);
        result.SuccessfulFetchesToday.Should().Be(2);
        result.FailedFetchesToday.Should().Be(1);
    }

    [Fact]
    public async Task GetSystemHealth_WithCompleteData_ShouldReturnAllMetrics()
    {
        // Arrange - Set up comprehensive test data
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");
        await CreateTestCurrency("GBP");

        var ecbId = await CreateTestProvider("ECB", "ECB", eurId, isActive: true);
        var cnbId = await CreateTestProvider("CNB", "CNB", eurId, isActive: true);
        await CreateTestProvider("BNR", "BNR", eurId, isActive: false);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        // Create exchange rates
        await CreateExchangeRates(ecbId, today, ("USD", 1.20m), ("GBP", 0.85m));
        await CreateExchangeRates(cnbId, yesterday, ("USD", 1.19m));

        // Create fetch logs
        await CreateFetchLog(ecbId, today, "Success", ratesImported: 10, ratesUpdated: 5);
        await CreateFetchLog(cnbId, today, "Failed", errorMessage: "Connection timeout");

        var query = new GetSystemHealthQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.TotalProviders.Should().Be(3);
        result.ActiveProviders.Should().Be(2);
        result.TotalCurrencies.Should().Be(3);
        result.TotalExchangeRates.Should().BeGreaterThanOrEqualTo(3);
        result.TotalFetchesToday.Should().Be(2);
        result.SuccessfulFetchesToday.Should().Be(1);
        result.FailedFetchesToday.Should().Be(1);
        result.LastUpdated.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetSystemHealth_CalledMultipleTimes_ShouldReturnConsistentResults()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await CreateExchangeRates(providerId, today, ("USD", 1.20m));

        var query = new GetSystemHealthQuery();

        // Act
        var result1 = await Mediator.Send(query);
        var result2 = await Mediator.Send(query);

        // Assert
        result1.TotalProviders.Should().Be(result2.TotalProviders);
        result1.TotalCurrencies.Should().Be(result2.TotalCurrencies);
        result1.TotalExchangeRates.Should().Be(result2.TotalExchangeRates);
    }
}
