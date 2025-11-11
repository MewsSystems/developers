using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Queries.SystemHealth.GetFetchActivity;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Queries.SystemHealth;

/// <summary>
/// Integration tests for GetFetchActivityQuery.
/// Tests fetch activity log retrieval and filtering from database views.
/// </summary>
public class GetFetchActivityQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrency(string code)
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        return result.Value;
    }

    private async Task<int> CreateTestProvider(string name, string code, int baseCurrencyId)
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

        return provider?.Id ?? result.Value;
    }

    private async Task CreateFetchLog(int providerId, string status, int ratesImported = 0, int ratesUpdated = 0, string? errorMessage = null, DateTimeOffset? timestamp = null)
    {
        var fetchStarted = timestamp ?? DateTimeOffset.UtcNow.AddMinutes(-1);
        var fetchCompleted = status == "Running" ? (DateTimeOffset?)null : fetchStarted.AddSeconds(5);

        var fetchLog = new DataLayer.Entities.ExchangeRateFetchLog
        {
            ProviderId = providerId,
            FetchStarted = fetchStarted,
            FetchCompleted = fetchCompleted,
            Status = status,
            RatesImported = ratesImported,
            RatesUpdated = ratesUpdated,
            ErrorMessage = errorMessage,
            DurationMs = fetchCompleted.HasValue ? 5000 : null
        };

        DbContext.Set<DataLayer.Entities.ExchangeRateFetchLog>().Add(fetchLog);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetFetchActivity_WithNoLogs_ShouldReturnEmptyResult()
    {
        // Arrange
        var query = new GetFetchActivityQuery(Count: 50);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetFetchActivity_WithLogs_ShouldReturnActivity()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        await CreateFetchLog(providerId, "Success", ratesImported: 10, ratesUpdated: 5);
        await CreateFetchLog(providerId, "Success", ratesImported: 8, ratesUpdated: 2);
        await CreateFetchLog(providerId, "Failed", errorMessage: "Connection timeout");

        var query = new GetFetchActivityQuery(Count: 50);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeEmpty();
        result.Items.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetFetchActivity_WithProviderIdFilter_ShouldReturnOnlyMatchingProvider()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var ecbId = await CreateTestProvider("ECB", "ECB", eurId);
        var cnbId = await CreateTestProvider("CNB", "CNB", eurId);

        await CreateFetchLog(ecbId, "Success", ratesImported: 10);
        await CreateFetchLog(ecbId, "Success", ratesImported: 8);
        await CreateFetchLog(cnbId, "Success", ratesImported: 5);

        var query = new GetFetchActivityQuery(Count: 50, ProviderId: ecbId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeEmpty();
        result.Items.Should().OnlyContain(a => a.ProviderId == ecbId);
    }

    [Fact]
    public async Task GetFetchActivity_WithFailedOnlyFilter_ShouldReturnOnlyFailures()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        await CreateFetchLog(providerId, "Success", ratesImported: 10);
        await CreateFetchLog(providerId, "Failed", errorMessage: "Connection timeout");
        await CreateFetchLog(providerId, "PartialSuccess", errorMessage: "Invalid response");

        var query = new GetFetchActivityQuery(Count: 50, FailedOnly: true);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeEmpty();
        result.Items.Should().OnlyContain(a => a.Status == "Failed");
    }

    [Fact]
    public async Task GetFetchActivity_WithCountLimit_ShouldRespectLimit()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        for (int i = 0; i < 10; i++)
        {
            await CreateFetchLog(providerId, "Success", ratesImported: i);
        }

        var query = new GetFetchActivityQuery(Count: 5);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountLessThanOrEqualTo(5);
    }

    [Fact]
    public async Task GetFetchActivity_WithMultipleProviders_ShouldReturnAllActivity()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var ecbId = await CreateTestProvider("ECB", "ECB", eurId);
        var cnbId = await CreateTestProvider("CNB", "CNB", eurId);
        var bnrId = await CreateTestProvider("BNR", "BNR", eurId);

        await CreateFetchLog(ecbId, "Success", ratesImported: 10);
        await CreateFetchLog(cnbId, "Success", ratesImported: 8);
        await CreateFetchLog(bnrId, "Failed", errorMessage: "Connection failed");

        var query = new GetFetchActivityQuery(Count: 50);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.Items.Select(a => a.ProviderId).Distinct().Should().HaveCount(3);
    }

    [Fact]
    public async Task GetFetchActivity_WithDurationCalculation_ShouldIncludeDuration()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        await CreateFetchLog(providerId, "Success", ratesImported: 10);

        var query = new GetFetchActivityQuery(Count: 50);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeEmpty();
        var activity = result.Items.First();
        activity.Duration.Should().NotBeNull();
        activity.Duration.Should().BeGreaterThan(TimeSpan.Zero);
    }

    [Fact]
    public async Task GetFetchActivity_WithErrorMessages_ShouldIncludeErrorDetails()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var errorMessage = "Connection timeout after 30 seconds";
        await CreateFetchLog(providerId, "Failed", errorMessage: errorMessage);

        var query = new GetFetchActivityQuery(Count: 50);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeEmpty();
        var activity = result.Items.First();
        activity.ErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public async Task GetFetchActivity_WithRecentTimestamps_ShouldOrderByMostRecent()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var now = DateTimeOffset.UtcNow;
        await CreateFetchLog(providerId, "Success", ratesImported: 10, timestamp: now.AddHours(-2));
        await CreateFetchLog(providerId, "Success", ratesImported: 8, timestamp: now.AddMinutes(-30));
        await CreateFetchLog(providerId, "Success", ratesImported: 5, timestamp: now.AddMinutes(-5));

        var query = new GetFetchActivityQuery(Count: 50);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);

        // Most recent should be first
        var firstActivity = result.Items.First();
        firstActivity.StartedAt.Should().BeCloseTo(now.AddMinutes(-5), TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetFetchActivity_WithRunningStatus_ShouldIncludeIncompleteFetches()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        await CreateFetchLog(providerId, "Running");
        await CreateFetchLog(providerId, "Success", ratesImported: 10);

        var query = new GetFetchActivityQuery(Count: 50);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Should().Contain(a => a.Status == "Running");
    }

    [Fact]
    public async Task GetFetchActivity_WithCombinedFilters_ShouldApplyAllFilters()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var ecbId = await CreateTestProvider("ECB", "ECB", eurId);
        var cnbId = await CreateTestProvider("CNB", "CNB", eurId);

        await CreateFetchLog(ecbId, "Success", ratesImported: 10);
        await CreateFetchLog(ecbId, "Failed", errorMessage: "ECB connection failed");
        await CreateFetchLog(cnbId, "Failed", errorMessage: "CNB connection failed");

        var query = new GetFetchActivityQuery(Count: 50, ProviderId: ecbId, FailedOnly: true);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeEmpty();
        result.Items.Should().HaveCount(1);
        result.Items.Should().OnlyContain(a => a.ProviderId == ecbId && a.Status == "Failed");
    }

    [Fact]
    public async Task GetFetchActivity_WithDefaultParameters_ShouldUseDefaults()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        await CreateFetchLog(providerId, "Success", ratesImported: 10);

        var query = new GetFetchActivityQuery(); // Uses default Count=50, ProviderId=null, FailedOnly=false

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(50);
    }

    [Fact]
    public async Task GetFetchActivity_WithRatesImportedAndUpdated_ShouldReturnCorrectCounts()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        await CreateFetchLog(providerId, "Success", ratesImported: 15, ratesUpdated: 7);

        var query = new GetFetchActivityQuery(Count: 50);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeEmpty();
        var activity = result.Items.First();
        activity.RatesImported.Should().Be(15);
        activity.RatesUpdated.Should().Be(7);
    }

    [Fact]
    public async Task GetFetchActivity_CalledMultipleTimes_ShouldReturnConsistentResults()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        await CreateFetchLog(providerId, "Success", ratesImported: 10);

        var query = new GetFetchActivityQuery(Count: 50);

        // Act
        var result1 = await Mediator.Send(query);
        var result2 = await Mediator.Send(query);

        // Assert
        result1.TotalCount.Should().Be(result2.TotalCount);
        result1.Items.Count.Should().Be(result2.Items.Count);
    }
}
