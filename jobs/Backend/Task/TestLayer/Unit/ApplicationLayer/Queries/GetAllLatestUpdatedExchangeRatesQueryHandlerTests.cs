using ApplicationLayer.Queries.ExchangeRates.GetAllLatestUpdatedExchangeRates;
using DomainLayer.Models.Views;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Queries;

/// <summary>
/// Unit tests for GetAllLatestUpdatedExchangeRatesQueryHandler.
/// Tests query flow using database views through ISystemViewQueries.
/// </summary>
public class GetAllLatestUpdatedExchangeRatesQueryHandlerTests : TestBase
{
    private readonly GetAllLatestUpdatedExchangeRatesQueryHandler _handler;

    public GetAllLatestUpdatedExchangeRatesQueryHandlerTests()
    {
        _handler = new GetAllLatestUpdatedExchangeRatesQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetAllLatestUpdatedExchangeRatesQueryHandler>().Object);
    }

    [Fact]
    public async Task Handle_WithAvailableRates_ShouldReturnMappedDtos()
    {
        // Arrange
        var query = new GetAllLatestUpdatedExchangeRatesQuery();

        var viewData = new List<AllLatestUpdatedExchangeRatesView>
        {
            new()
            {
                Id = 1,
                ProviderId = 1,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                BaseCurrencyCode = "EUR",
                BaseCurrencyId = 1,
                TargetCurrencyCode = "USD",
                TargetCurrencyId = 2,
                Rate = 1.0850m,
                Multiplier = 1,
                RatePerUnit = 1.0850m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero),
                Modified = null,
                DaysOld = 0,
                MinutesSinceUpdate = 5,
                UpdateFreshness = "Fresh"
            },
            new()
            {
                Id = 2,
                ProviderId = 1,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                BaseCurrencyCode = "EUR",
                BaseCurrencyId = 1,
                TargetCurrencyCode = "GBP",
                TargetCurrencyId = 3,
                Rate = 0.8450m,
                Multiplier = 1,
                RatePerUnit = 0.8450m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero),
                Modified = null,
                DaysOld = 0,
                MinutesSinceUpdate = 5,
                UpdateFreshness = "Fresh"
            }
        };

        // Mock: View query returns data
        MockViewQueries
            .Setup(x => x.GetAllLatestUpdatedExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var rates = result.ToList();

        // Verify first rate
        rates[0].ProviderCode.Should().Be("ECB");
        rates[0].BaseCurrencyCode.Should().Be("EUR");
        rates[0].TargetCurrencyCode.Should().Be("USD");
        rates[0].Rate.Should().Be(1.0850m);
        rates[0].Multiplier.Should().Be(1);
        rates[0].EffectiveRate.Should().Be(1.0850m);
        rates[0].ValidDate.Should().Be(new DateOnly(2025, 11, 6));
        rates[0].MinutesSinceUpdate.Should().Be(5);
        rates[0].UpdateFreshness.Should().Be("Fresh");

        // Verify second rate
        rates[1].TargetCurrencyCode.Should().Be("GBP");
        rates[1].Rate.Should().Be(0.8450m);
        rates[1].UpdateFreshness.Should().Be("Fresh");

        // Verify view query was called once
        MockViewQueries.Verify(
            x => x.GetAllLatestUpdatedExchangeRatesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoRates_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetAllLatestUpdatedExchangeRatesQuery();

        // Mock: View query returns empty
        MockViewQueries
            .Setup(x => x.GetAllLatestUpdatedExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AllLatestUpdatedExchangeRatesView>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        MockViewQueries.Verify(
            x => x.GetAllLatestUpdatedExchangeRatesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithVariousUpdateFreshness_ShouldMapCorrectly()
    {
        // Arrange
        var query = new GetAllLatestUpdatedExchangeRatesQuery();

        var viewData = new List<AllLatestUpdatedExchangeRatesView>
        {
            new()
            {
                Id = 1,
                ProviderId = 1,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                BaseCurrencyCode = "EUR",
                BaseCurrencyId = 1,
                TargetCurrencyCode = "USD",
                TargetCurrencyId = 2,
                Rate = 1.0850m,
                Multiplier = 1,
                RatePerUnit = 1.0850m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero),
                Modified = null,
                DaysOld = 0,
                MinutesSinceUpdate = 30,
                UpdateFreshness = "Fresh"
            },
            new()
            {
                Id = 2,
                ProviderId = 2,
                ProviderCode = "CNB",
                ProviderName = "Czech National Bank",
                BaseCurrencyCode = "CZK",
                BaseCurrencyId = 3,
                TargetCurrencyCode = "EUR",
                TargetCurrencyId = 1,
                Rate = 25.15m,
                Multiplier = 1,
                RatePerUnit = 25.15m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 7, 0, 0, TimeSpan.Zero),
                Modified = null,
                DaysOld = 0,
                MinutesSinceUpdate = 180,
                UpdateFreshness = "Recent"
            }
        };

        MockViewQueries
            .Setup(x => x.GetAllLatestUpdatedExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        var freshRate = result.First(r => r.ProviderCode == "ECB");
        freshRate.MinutesSinceUpdate.Should().Be(30);
        freshRate.UpdateFreshness.Should().Be("Fresh");

        var recentRate = result.First(r => r.ProviderCode == "CNB");
        recentRate.MinutesSinceUpdate.Should().Be(180);
        recentRate.UpdateFreshness.Should().Be("Recent");
    }

    [Fact]
    public async Task Handle_WithMostRecentlyCreated_ShouldReturnCorrectProvider()
    {
        // Arrange - Two providers, same currency pair, TEST provider updated more recently
        var query = new GetAllLatestUpdatedExchangeRatesQuery();

        var viewData = new List<AllLatestUpdatedExchangeRatesView>
        {
            // EUR/USD from TEST provider - most recently created
            new()
            {
                Id = 2,
                ProviderId = 2,
                ProviderCode = "TEST",
                ProviderName = "Test Provider",
                BaseCurrencyCode = "EUR",
                BaseCurrencyId = 1,
                TargetCurrencyCode = "USD",
                TargetCurrencyId = 2,
                Rate = 1.0900m,
                Multiplier = 1,
                RatePerUnit = 1.0900m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 11, 0, 0, TimeSpan.Zero),
                Modified = null,
                DaysOld = 0,
                MinutesSinceUpdate = 5,
                UpdateFreshness = "Fresh"
            }
        };

        MockViewQueries
            .Setup(x => x.GetAllLatestUpdatedExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var rate = result.Single();
        rate.ProviderCode.Should().Be("TEST");
        rate.Rate.Should().Be(1.0900m);
        rate.MinutesSinceUpdate.Should().Be(5);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToViewQuery()
    {
        // Arrange
        var query = new GetAllLatestUpdatedExchangeRatesQuery();
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        MockViewQueries
            .Setup(x => x.GetAllLatestUpdatedExchangeRatesAsync(token))
            .ReturnsAsync(new List<AllLatestUpdatedExchangeRatesView>());

        // Act
        await _handler.Handle(query, token);

        // Assert
        MockViewQueries.Verify(
            x => x.GetAllLatestUpdatedExchangeRatesAsync(token),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultiplierGreaterThanOne_ShouldMapRatePerUnitCorrectly()
    {
        // Arrange
        var query = new GetAllLatestUpdatedExchangeRatesQuery();

        var viewData = new List<AllLatestUpdatedExchangeRatesView>
        {
            new()
            {
                Id = 1,
                ProviderId = 2,
                ProviderCode = "CNB",
                ProviderName = "Czech National Bank",
                BaseCurrencyCode = "CZK",
                BaseCurrencyId = 3,
                TargetCurrencyCode = "JPY",
                TargetCurrencyId = 4,
                Rate = 10850m,
                Multiplier = 100,
                RatePerUnit = 108.50m, // Rate / Multiplier
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero),
                Modified = null,
                DaysOld = 0,
                MinutesSinceUpdate = 10,
                UpdateFreshness = "Fresh"
            }
        };

        MockViewQueries
            .Setup(x => x.GetAllLatestUpdatedExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var rate = result.Single();
        rate.Rate.Should().Be(10850m);
        rate.Multiplier.Should().Be(100);
        rate.EffectiveRate.Should().Be(108.50m); // Pre-calculated in view
    }

    [Fact]
    public async Task Handle_WithModifiedRates_ShouldIncludeModifiedTimestamp()
    {
        // Arrange
        var query = new GetAllLatestUpdatedExchangeRatesQuery();

        var viewData = new List<AllLatestUpdatedExchangeRatesView>
        {
            new()
            {
                Id = 1,
                ProviderId = 1,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                BaseCurrencyCode = "EUR",
                BaseCurrencyId = 1,
                TargetCurrencyCode = "USD",
                TargetCurrencyId = 2,
                Rate = 1.0850m,
                Multiplier = 1,
                RatePerUnit = 1.0850m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero),
                Modified = new DateTimeOffset(2025, 11, 6, 14, 30, 0, TimeSpan.Zero),
                DaysOld = 0,
                MinutesSinceUpdate = 15,
                UpdateFreshness = "Fresh"
            }
        };

        MockViewQueries
            .Setup(x => x.GetAllLatestUpdatedExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var rate = result.Single();
        rate.Created.Should().Be(new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero));
        rate.Modified.Should().Be(new DateTimeOffset(2025, 11, 6, 14, 30, 0, TimeSpan.Zero));
    }

    [Fact]
    public async Task Handle_ShouldNotIncludeFreshnessStatus()
    {
        // Arrange - This view uses UpdateFreshness, not FreshnessStatus
        var query = new GetAllLatestUpdatedExchangeRatesQuery();

        var viewData = new List<AllLatestUpdatedExchangeRatesView>
        {
            new()
            {
                Id = 1,
                ProviderId = 1,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                BaseCurrencyCode = "EUR",
                BaseCurrencyId = 1,
                TargetCurrencyCode = "USD",
                TargetCurrencyId = 2,
                Rate = 1.0850m,
                Multiplier = 1,
                RatePerUnit = 1.0850m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero),
                Modified = null,
                DaysOld = 0,
                MinutesSinceUpdate = 5,
                UpdateFreshness = "Fresh"
            }
        };

        MockViewQueries
            .Setup(x => x.GetAllLatestUpdatedExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var rate = result.Single();
        rate.FreshnessStatus.Should().BeEmpty(); // Not populated in this view
        rate.UpdateFreshness.Should().Be("Fresh"); // This is what's used instead
    }
}
