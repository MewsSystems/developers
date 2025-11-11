using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using DomainLayer.Models.Views;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Queries;

/// <summary>
/// Unit tests for GetAllLatestExchangeRatesQueryHandler.
/// Tests query flow using database views through ISystemViewQueries.
/// </summary>
public class GetAllLatestExchangeRatesQueryHandlerTests : TestBase
{
    private readonly GetAllLatestExchangeRatesQueryHandler _handler;

    public GetAllLatestExchangeRatesQueryHandlerTests()
    {
        _handler = new GetAllLatestExchangeRatesQueryHandler(
            MockViewQueries.Object,
            CreateMockLogger<GetAllLatestExchangeRatesQueryHandler>().Object);
    }

    [Fact]
    public async Task Handle_WithAvailableRates_ShouldReturnMappedDtos()
    {
        // Arrange
        var query = new GetAllLatestExchangeRatesQuery();

        var viewData = new List<AllLatestExchangeRatesView>
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
                FreshnessStatus = "Current"
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
                FreshnessStatus = "Current"
            }
        };

        // Mock: View query returns data
        MockViewQueries
            .Setup(x => x.GetAllLatestExchangeRatesAsync(It.IsAny<CancellationToken>()))
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
        rates[0].DaysOld.Should().Be(0);
        rates[0].FreshnessStatus.Should().Be("Current");

        // Verify second rate
        rates[1].TargetCurrencyCode.Should().Be("GBP");
        rates[1].Rate.Should().Be(0.8450m);
        rates[1].FreshnessStatus.Should().Be("Current");

        // Verify view query was called once
        MockViewQueries.Verify(
            x => x.GetAllLatestExchangeRatesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoRates_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetAllLatestExchangeRatesQuery();

        // Mock: View query returns empty
        MockViewQueries
            .Setup(x => x.GetAllLatestExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AllLatestExchangeRatesView>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        MockViewQueries.Verify(
            x => x.GetAllLatestExchangeRatesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithOldRates_ShouldMapFreshnessStatusCorrectly()
    {
        // Arrange
        var query = new GetAllLatestExchangeRatesQuery();

        var viewData = new List<AllLatestExchangeRatesView>
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
                ValidDate = new DateOnly(2025, 11, 1),
                Created = new DateTimeOffset(2025, 11, 1, 10, 0, 0, TimeSpan.Zero),
                Modified = null,
                DaysOld = 5,
                FreshnessStatus = "Week Old"
            }
        };

        MockViewQueries
            .Setup(x => x.GetAllLatestExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var rate = result.Single();
        rate.DaysOld.Should().Be(5);
        rate.FreshnessStatus.Should().Be("Week Old");
    }

    [Fact]
    public async Task Handle_WithMultipleProviders_ShouldReturnLatestForEachCurrencyPair()
    {
        // Arrange
        var query = new GetAllLatestExchangeRatesQuery();

        var viewData = new List<AllLatestExchangeRatesView>
        {
            // Latest EUR/USD from ECB
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
                FreshnessStatus = "Current"
            },
            // Latest CZK/EUR from CNB
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
                Created = new DateTimeOffset(2025, 11, 6, 10, 30, 0, TimeSpan.Zero),
                Modified = null,
                DaysOld = 0,
                FreshnessStatus = "Current"
            }
        };

        MockViewQueries
            .Setup(x => x.GetAllLatestExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        var eurUsd = result.First(r => r.BaseCurrencyCode == "EUR" && r.TargetCurrencyCode == "USD");
        eurUsd.ProviderCode.Should().Be("ECB");
        eurUsd.Rate.Should().Be(1.0850m);

        var czkEur = result.First(r => r.BaseCurrencyCode == "CZK" && r.TargetCurrencyCode == "EUR");
        czkEur.ProviderCode.Should().Be("CNB");
        czkEur.Rate.Should().Be(25.15m);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToViewQuery()
    {
        // Arrange
        var query = new GetAllLatestExchangeRatesQuery();
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        MockViewQueries
            .Setup(x => x.GetAllLatestExchangeRatesAsync(token))
            .ReturnsAsync(new List<AllLatestExchangeRatesView>());

        // Act
        await _handler.Handle(query, token);

        // Assert
        MockViewQueries.Verify(
            x => x.GetAllLatestExchangeRatesAsync(token),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultiplierGreaterThanOne_ShouldMapRatePerUnitCorrectly()
    {
        // Arrange
        var query = new GetAllLatestExchangeRatesQuery();

        var viewData = new List<AllLatestExchangeRatesView>
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
                FreshnessStatus = "Current"
            }
        };

        MockViewQueries
            .Setup(x => x.GetAllLatestExchangeRatesAsync(It.IsAny<CancellationToken>()))
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
        var query = new GetAllLatestExchangeRatesQuery();

        var viewData = new List<AllLatestExchangeRatesView>
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
                FreshnessStatus = "Current"
            }
        };

        MockViewQueries
            .Setup(x => x.GetAllLatestExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var rate = result.Single();
        rate.Created.Should().Be(new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero));
        rate.Modified.Should().Be(new DateTimeOffset(2025, 11, 6, 14, 30, 0, TimeSpan.Zero));
    }
}
