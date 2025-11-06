using ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;
using DomainLayer.Models.Views;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Queries;

/// <summary>
/// Unit tests for GetCurrentExchangeRatesQueryHandler.
/// Tests query flow using database views through ISystemViewQueries.
/// </summary>
public class GetCurrentExchangeRatesQueryHandlerTests : TestBase
{
    private readonly GetCurrentExchangeRatesQueryHandler _handler;

    public GetCurrentExchangeRatesQueryHandlerTests()
    {
        _handler = new GetCurrentExchangeRatesQueryHandler(
            MockViewQueries.Object,
            MockDateTimeProvider.Object,
            CreateMockLogger<GetCurrentExchangeRatesQueryHandler>().Object);
    }

    [Fact]
    public async Task Handle_WithAvailableRates_ShouldReturnMappedDtos()
    {
        // Arrange
        var query = new GetCurrentExchangeRatesQuery();

        var viewData = new List<CurrentExchangeRateView>
        {
            new()
            {
                Id = 1,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                BaseCurrencyCode = "EUR",
                TargetCurrencyCode = "USD",
                TargetCurrencyId = 1,
                Rate = 1.0850m,
                Multiplier = 1,
                RatePerUnit = 1.0850m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero)
            },
            new()
            {
                Id = 2,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                BaseCurrencyCode = "EUR",
                TargetCurrencyCode = "GBP",
                TargetCurrencyId = 2,
                Rate = 0.8450m,
                Multiplier = 1,
                RatePerUnit = 0.8450m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero)
            }
        };

        // Mock: View query returns data
        MockViewQueries
            .Setup(x => x.GetCurrentExchangeRatesAsync(It.IsAny<CancellationToken>()))
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

        // Verify second rate
        rates[1].TargetCurrencyCode.Should().Be("GBP");
        rates[1].Rate.Should().Be(0.8450m);

        // Verify view query was called once
        MockViewQueries.Verify(
            x => x.GetCurrentExchangeRatesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoRates_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetCurrentExchangeRatesQuery();

        // Mock: View query returns empty
        MockViewQueries
            .Setup(x => x.GetCurrentExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CurrentExchangeRateView>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        MockViewQueries.Verify(
            x => x.GetCurrentExchangeRatesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCalculateDaysOldCorrectly()
    {
        // Arrange
        var query = new GetCurrentExchangeRatesQuery();

        // Setup: Current time is Nov 6, 2025, 12:00 PM
        var now = new DateTimeOffset(2025, 11, 6, 12, 0, 0, TimeSpan.Zero);
        MockDateTimeProvider.Setup(x => x.UtcNow).Returns(now);

        var viewData = new List<CurrentExchangeRateView>
        {
            new()
            {
                Id = 1,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                BaseCurrencyCode = "EUR",
                TargetCurrencyCode = "USD",
                TargetCurrencyId = 1,
                Rate = 1.0850m,
                Multiplier = 1,
                RatePerUnit = 1.0850m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 3, 10, 0, 0, TimeSpan.Zero) // 3 days old
            }
        };

        MockViewQueries
            .Setup(x => x.GetCurrentExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var rate = result.Single();
        rate.DaysOld.Should().Be(3); // 3 days difference

        // Verify DateTimeProvider was used
        MockDateTimeProvider.Verify(x => x.UtcNow, Times.AtLeastOnce());
    }

    [Fact]
    public async Task Handle_WithMultipleProviders_ShouldReturnAllRates()
    {
        // Arrange
        var query = new GetCurrentExchangeRatesQuery();

        var viewData = new List<CurrentExchangeRateView>
        {
            new()
            {
                Id = 1,
                ProviderCode = "ECB",
                ProviderName = "European Central Bank",
                BaseCurrencyCode = "EUR",
                TargetCurrencyCode = "USD",
                TargetCurrencyId = 1,
                Rate = 1.0850m,
                Multiplier = 1,
                RatePerUnit = 1.0850m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero)
            },
            new()
            {
                Id = 2,
                ProviderCode = "CNB",
                ProviderName = "Czech National Bank",
                BaseCurrencyCode = "CZK",
                TargetCurrencyCode = "EUR",
                TargetCurrencyId = 3,
                Rate = 25.15m,
                Multiplier = 1,
                RatePerUnit = 25.15m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 30, 0, TimeSpan.Zero)
            },
            new()
            {
                Id = 3,
                ProviderCode = "BNR",
                ProviderName = "Romanian National Bank",
                BaseCurrencyCode = "RON",
                TargetCurrencyCode = "EUR",
                TargetCurrencyId = 4,
                Rate = 4.9750m,
                Multiplier = 1,
                RatePerUnit = 4.9750m,
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 11, 0, 0, TimeSpan.Zero)
            }
        };

        MockViewQueries
            .Setup(x => x.GetCurrentExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);

        var providers = result.Select(r => r.ProviderCode).Distinct().ToList();
        providers.Should().Contain(new[] { "ECB", "CNB", "BNR" });

        // Verify each provider's rates
        result.Single(r => r.ProviderCode == "ECB").TargetCurrencyCode.Should().Be("USD");
        result.Single(r => r.ProviderCode == "CNB").TargetCurrencyCode.Should().Be("EUR");
        result.Single(r => r.ProviderCode == "BNR").TargetCurrencyCode.Should().Be("EUR");
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToViewQuery()
    {
        // Arrange
        var query = new GetCurrentExchangeRatesQuery();
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        MockViewQueries
            .Setup(x => x.GetCurrentExchangeRatesAsync(token))
            .ReturnsAsync(new List<CurrentExchangeRateView>());

        // Act
        await _handler.Handle(query, token);

        // Assert
        MockViewQueries.Verify(
            x => x.GetCurrentExchangeRatesAsync(token),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultiplierGreaterThanOne_ShouldMapRatePerUnitCorrectly()
    {
        // Arrange
        var query = new GetCurrentExchangeRatesQuery();

        var viewData = new List<CurrentExchangeRateView>
        {
            new()
            {
                Id = 1,
                ProviderCode = "CNB",
                ProviderName = "Czech National Bank",
                BaseCurrencyCode = "CZK",
                TargetCurrencyCode = "JPY",
                TargetCurrencyId = 5,
                Rate = 10850m,
                Multiplier = 100,
                RatePerUnit = 108.50m, // Rate / Multiplier
                ValidDate = new DateOnly(2025, 11, 6),
                Created = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero)
            }
        };

        MockViewQueries
            .Setup(x => x.GetCurrentExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viewData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var rate = result.Single();
        rate.Rate.Should().Be(10850m);
        rate.Multiplier.Should().Be(100);
        rate.EffectiveRate.Should().Be(108.50m); // Pre-calculated in view
    }
}
