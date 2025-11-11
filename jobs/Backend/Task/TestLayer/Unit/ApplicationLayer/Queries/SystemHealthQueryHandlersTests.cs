using ApplicationLayer.Queries.SystemHealth.GetFetchActivity;
using ApplicationLayer.Queries.SystemHealth.GetRecentErrors;
using ApplicationLayer.Queries.SystemHealth.GetSystemHealth;
using DomainLayer.Models.Views;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Queries;

/// <summary>
/// Unit tests for SystemHealth query handlers.
/// Tests system monitoring queries: GetSystemHealth, GetFetchActivity, GetRecentErrors.
/// </summary>
public class SystemHealthQueryHandlersTests : TestBase
{
    #region GetSystemHealthQueryHandler Tests

    [Fact]
    public async Task GetSystemHealth_ShouldReturnAggregatedMetrics()
    {
        // Arrange
        var handler = new GetSystemHealthQueryHandler(
            MockViewQueries.Object,
            MockDateTimeProvider.Object,
            CreateMockLogger<GetSystemHealthQueryHandler>().Object);

        var query = new GetSystemHealthQuery();

        var metrics = new List<SystemHealthDashboardView>
        {
            new() { Metric = "TotalProviders", Value = "5", Status = "OK" },
            new() { Metric = "ActiveProviders", Value = "4", Status = "OK" },
            new() { Metric = "QuarantinedProviders", Value = "1", Status = "Warning" },
            new() { Metric = "TotalCurrencies", Value = "50", Status = "OK" },
            new() { Metric = "TotalExchangeRates", Value = "1250", Status = "OK" },
            new() { Metric = "LatestRateDate", Value = "2025-11-06", Status = "OK" },
            new() { Metric = "OldestRateDate", Value = "2025-10-01", Status = "OK" },
            new() { Metric = "TotalFetchesToday", Value = "10", Status = "OK" },
            new() { Metric = "SuccessfulFetchesToday", Value = "9", Status = "OK" },
            new() { Metric = "FailedFetchesToday", Value = "1", Status = "Warning" },
            new() { Metric = "SuccessRateToday", Value = "90.0", Status = "OK" }
        };

        MockViewQueries
            .Setup(x => x.GetSystemHealthDashboardAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(metrics);

        var now = new DateTimeOffset(2025, 11, 6, 12, 0, 0, TimeSpan.Zero);
        MockDateTimeProvider
            .Setup(x => x.UtcNow)
            .Returns(now);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalProviders.Should().Be(5);
        result.ActiveProviders.Should().Be(4);
        result.QuarantinedProviders.Should().Be(1);
        result.TotalCurrencies.Should().Be(50);
        result.TotalExchangeRates.Should().Be(1250);
        result.LatestRateDate.Should().Be(new DateOnly(2025, 11, 6));
        result.OldestRateDate.Should().Be(new DateOnly(2025, 10, 1));
        result.TotalFetchesToday.Should().Be(10);
        result.SuccessfulFetchesToday.Should().Be(9);
        result.FailedFetchesToday.Should().Be(1);
        result.SuccessRateToday.Should().Be(90.0m);
        result.LastUpdated.Should().Be(now);
    }

    [Fact]
    public async Task GetSystemHealth_WithMissingMetrics_ShouldUseDefaults()
    {
        // Arrange
        var handler = new GetSystemHealthQueryHandler(
            MockViewQueries.Object,
            MockDateTimeProvider.Object,
            CreateMockLogger<GetSystemHealthQueryHandler>().Object);

        var query = new GetSystemHealthQuery();

        // Minimal metrics - missing most values
        var metrics = new List<SystemHealthDashboardView>
        {
            new() { Metric = "TotalProviders", Value = "5", Status = "OK" }
        };

        MockViewQueries
            .Setup(x => x.GetSystemHealthDashboardAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(metrics);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalProviders.Should().Be(5);
        result.ActiveProviders.Should().Be(0); // Default
        result.TotalCurrencies.Should().Be(0); // Default
        result.LatestRateDate.Should().BeNull(); // Default
        result.SuccessRateToday.Should().Be(0m); // Default
    }

    [Fact]
    public async Task GetSystemHealth_WithInvalidMetricValues_ShouldUseDefaults()
    {
        // Arrange
        var handler = new GetSystemHealthQueryHandler(
            MockViewQueries.Object,
            MockDateTimeProvider.Object,
            CreateMockLogger<GetSystemHealthQueryHandler>().Object);

        var query = new GetSystemHealthQuery();

        var metrics = new List<SystemHealthDashboardView>
        {
            new() { Metric = "TotalProviders", Value = "invalid", Status = "Error" },
            new() { Metric = "SuccessRateToday", Value = "not-a-number", Status = "Error" },
            new() { Metric = "LatestRateDate", Value = "invalid-date", Status = "Error" }
        };

        MockViewQueries
            .Setup(x => x.GetSystemHealthDashboardAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(metrics);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalProviders.Should().Be(0); // Parsed as default
        result.SuccessRateToday.Should().Be(0m); // Parsed as default
        result.LatestRateDate.Should().BeNull(); // Parsed as default
    }

    #endregion

    #region GetFetchActivityQueryHandler Tests

    [Fact]
    public async Task GetFetchActivity_WithoutFilters_ShouldReturnAllActivity()
    {
        // Arrange
        var handler = new GetFetchActivityQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetFetchActivityQueryHandler>().Object);

        var query = new GetFetchActivityQuery(Count: 10);

        var activities = new List<RecentFetchActivityView>
        {
            new()
            {
                Id = 1,
                ProviderId = 1,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                FetchStarted = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero),
                FetchCompleted = new DateTimeOffset(2025, 11, 6, 10, 0, 30, TimeSpan.Zero),
                Status = "Success",
                RatesImported = 10,
                RatesUpdated = 5,
                DurationMs = 30000,
                ErrorMessage = null
            },
            new()
            {
                Id = 2,
                ProviderId = 2,
                ProviderCode = "CNB",
                ProviderName = "Czech National Bank",
                FetchStarted = new DateTimeOffset(2025, 11, 6, 9, 0, 0, TimeSpan.Zero),
                FetchCompleted = new DateTimeOffset(2025, 11, 6, 9, 1, 0, TimeSpan.Zero),
                Status = "Failed",
                RatesImported = 0,
                RatesUpdated = 0,
                DurationMs = 60000,
                ErrorMessage = "Connection timeout"
            }
        };

        MockViewQueries
            .Setup(x => x.GetRecentFetchActivityAsync(20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activities);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.First().ProviderCode.Should().Be("ECB");
        result.Items.First().Status.Should().Be("Success");
        result.Items.Last().Status.Should().Be("Failed");
        result.Items.Last().ErrorMessage.Should().Be("Connection timeout");
    }

    [Fact]
    public async Task GetFetchActivity_WithFailedOnlyFilter_ShouldReturnOnlyFailures()
    {
        // Arrange
        var handler = new GetFetchActivityQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetFetchActivityQueryHandler>().Object);

        var query = new GetFetchActivityQuery(Count: 10, FailedOnly: true);

        var activities = new List<RecentFetchActivityView>
        {
            new() { Id = 1, ProviderId = 1, ProviderCode = "ECB", ProviderName = "ECB", FetchStarted = DateTimeOffset.UtcNow, Status = "Success" },
            new() { Id = 2, ProviderId = 2, ProviderCode = "CNB", ProviderName = "CNB", FetchStarted = DateTimeOffset.UtcNow, Status = "Failed", ErrorMessage = "Error 1" },
            new() { Id = 3, ProviderId = 3, ProviderCode = "RNB", ProviderName = "RNB", FetchStarted = DateTimeOffset.UtcNow, Status = "Error", ErrorMessage = "Error 2" }
        };

        MockViewQueries
            .Setup(x => x.GetRecentFetchActivityAsync(20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activities);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.Items.All(a => a.Status == "Failed" || a.Status == "Error").Should().BeTrue();
    }

    [Fact]
    public async Task GetFetchActivity_WithProviderFilter_ShouldReturnOnlyProviderActivity()
    {
        // Arrange
        var handler = new GetFetchActivityQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetFetchActivityQueryHandler>().Object);

        var query = new GetFetchActivityQuery(Count: 10, ProviderId: 1);

        var activities = new List<RecentFetchActivityView>
        {
            new() { Id = 1, ProviderId = 1, ProviderCode = "ECB", ProviderName = "ECB", FetchStarted = DateTimeOffset.UtcNow, Status = "Success" },
            new() { Id = 2, ProviderId = 2, ProviderCode = "CNB", ProviderName = "CNB", FetchStarted = DateTimeOffset.UtcNow, Status = "Success" },
            new() { Id = 3, ProviderId = 1, ProviderCode = "ECB", ProviderName = "ECB", FetchStarted = DateTimeOffset.UtcNow, Status = "Failed" }
        };

        MockViewQueries
            .Setup(x => x.GetRecentFetchActivityAsync(20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activities);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.Items.All(a => a.ProviderId == 1).Should().BeTrue();
    }

    [Fact]
    public async Task GetFetchActivity_WithDurationCalculation_ShouldCalculateDuration()
    {
        // Arrange
        var handler = new GetFetchActivityQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetFetchActivityQueryHandler>().Object);

        var query = new GetFetchActivityQuery(Count: 10);

        var started = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero);
        var completed = new DateTimeOffset(2025, 11, 6, 10, 2, 30, TimeSpan.Zero); // 2 minutes 30 seconds

        var activities = new List<RecentFetchActivityView>
        {
            new()
            {
                Id = 1,
                ProviderId = 1,
                ProviderCode = "ECB",
                ProviderName = "ECB",
                FetchStarted = started,
                FetchCompleted = completed,
                Status = "Success",
                DurationMs = null // Test fallback calculation
            }
        };

        MockViewQueries
            .Setup(x => x.GetRecentFetchActivityAsync(20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activities);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().Duration.Should().NotBeNull();
        result.Items.First().Duration!.Value.TotalSeconds.Should().Be(150); // 2 minutes 30 seconds
    }

    #endregion

    #region GetRecentErrorsQueryHandler Tests

    [Fact]
    public async Task GetRecentErrors_WithoutFilters_ShouldReturnGroupedErrors()
    {
        // Arrange
        var handler = new GetRecentErrorsQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetRecentErrorsQueryHandler>().Object);

        var query = new GetRecentErrorsQuery(Count: 10);

        var errors = new List<ErrorSummaryView>
        {
            new()
            {
                Id = 1,
                Timestamp = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero),
                Severity = "Error",
                Source = "ProviderFetch",
                Message = "Connection timeout",
                MinutesAgo = 60
            },
            new()
            {
                Id = 2,
                Timestamp = new DateTimeOffset(2025, 11, 6, 10, 30, 0, TimeSpan.Zero),
                Severity = "Error",
                Source = "ProviderFetch",
                Message = "Connection timeout", // Same error, different occurrence
                MinutesAgo = 30
            },
            new()
            {
                Id = 3,
                Timestamp = new DateTimeOffset(2025, 11, 6, 11, 0, 0, TimeSpan.Zero),
                Severity = "Warning",
                Source = "RateFetch",
                Message = "No rates found",
                MinutesAgo = 10
            }
        };

        MockViewQueries
            .Setup(x => x.GetErrorSummaryAsync(24, It.IsAny<CancellationToken>()))
            .ReturnsAsync(errors);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2); // Two unique error types (grouped by Source + Message)

        var connectionError = result.Items.FirstOrDefault(e => e.ErrorMessage == "Connection timeout");
        connectionError.Should().NotBeNull();
        connectionError!.OccurrenceCount.Should().Be(2); // Two occurrences
        connectionError.FirstOccurrence.Should().Be(new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero));
        connectionError.LastOccurrence.Should().Be(new DateTimeOffset(2025, 11, 6, 10, 30, 0, TimeSpan.Zero));
    }

    [Fact]
    public async Task GetRecentErrors_WithSeverityFilter_ShouldReturnOnlyMatchingSeverity()
    {
        // Arrange
        var handler = new GetRecentErrorsQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetRecentErrorsQueryHandler>().Object);

        var query = new GetRecentErrorsQuery(Count: 10, Severity: "Error");

        var errors = new List<ErrorSummaryView>
        {
            new() { Id = 1, Timestamp = DateTimeOffset.UtcNow, Severity = "Error", Source = "Source1", Message = "Error message 1", MinutesAgo = 5 },
            new() { Id = 2, Timestamp = DateTimeOffset.UtcNow, Severity = "Warning", Source = "Source2", Message = "Warning message", MinutesAgo = 10 },
            new() { Id = 3, Timestamp = DateTimeOffset.UtcNow, Severity = "Error", Source = "Source3", Message = "Error message 2", MinutesAgo = 15 }
        };

        MockViewQueries
            .Setup(x => x.GetErrorSummaryAsync(24, It.IsAny<CancellationToken>()))
            .ReturnsAsync(errors);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.Items.All(e => e.ErrorType != "Source2").Should().BeTrue(); // Warning excluded
    }

    [Fact]
    public async Task GetRecentErrors_WithNoErrors_ShouldReturnEmptyResult()
    {
        // Arrange
        var handler = new GetRecentErrorsQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetRecentErrorsQueryHandler>().Object);

        var query = new GetRecentErrorsQuery(Count: 10);

        MockViewQueries
            .Setup(x => x.GetErrorSummaryAsync(24, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ErrorSummaryView>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetRecentErrors_ShouldOrderByLastOccurrence()
    {
        // Arrange
        var handler = new GetRecentErrorsQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetRecentErrorsQueryHandler>().Object);

        var query = new GetRecentErrorsQuery(Count: 10);

        var errors = new List<ErrorSummaryView>
        {
            new() { Id = 1, Timestamp = new DateTimeOffset(2025, 11, 6, 9, 0, 0, TimeSpan.Zero), Severity = "Error", Source = "Old", Message = "Old error", MinutesAgo = 120 },
            new() { Id = 2, Timestamp = new DateTimeOffset(2025, 11, 6, 11, 0, 0, TimeSpan.Zero), Severity = "Error", Source = "New", Message = "New error", MinutesAgo = 10 }
        };

        MockViewQueries
            .Setup(x => x.GetErrorSummaryAsync(24, It.IsAny<CancellationToken>()))
            .ReturnsAsync(errors);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.Items.First().ErrorType.Should().Be("New"); // Most recent first
        result.Items.Last().ErrorType.Should().Be("Old");
    }

    #endregion
}
