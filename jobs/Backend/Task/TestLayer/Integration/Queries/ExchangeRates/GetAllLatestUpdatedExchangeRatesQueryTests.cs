using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetAllLatestUpdatedExchangeRates;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.ExchangeRates;

public class GetAllLatestUpdatedExchangeRatesQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrencyAsync(string code)
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    private async Task<int> CreateTestProviderAsync(int currencyId, string code, string name)
    {
        var command = new CreateExchangeRateProviderCommand(
            Name: name,
            Code: code,
            Url: $"https://api.{code.ToLower()}.com/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    private async Task CreateTestRatesAsync(int providerId, DateOnly validDate, params (string source, string target, decimal rate)[] rates)
    {
        var rateItems = rates.Select(r => new ExchangeRateItemDto(r.source, r.target, r.rate, 1)).ToList();
        var command = new BulkUpsertExchangeRatesCommand(providerId, validDate, rateItems);
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllLatestUpdatedExchangeRates_WithNoRates_ShouldReturnEmptyList()
    {
        // Act
        var query = new GetAllLatestUpdatedExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllLatestUpdatedExchangeRates_WithSingleProvider_ShouldReturnLatestRates()
    {
        // Arrange
        var eurId = await CreateTestCurrencyAsync("EUR");
        var usdId = await CreateTestCurrencyAsync("USD");
        var gbpId = await CreateTestCurrencyAsync("GBP");
        var ecbId = await CreateTestProviderAsync(eurId, "ECB", "European Central Bank");

        var today = DateOnly.FromDateTime(DateTime.Today);
        await CreateTestRatesAsync(ecbId, today,
            ("EUR", "USD", 1.0850m),
            ("EUR", "GBP", 0.8650m));

        // Act
        var query = new GetAllLatestUpdatedExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(r => r.ProviderCode == "ECB");
        result.Should().OnlyContain(r => r.ValidDate == today);

        var usdRate = result.First(r => r.TargetCurrencyCode == "USD");
        usdRate.Rate.Should().Be(1.0850m);
        usdRate.BaseCurrencyCode.Should().Be("EUR");
        usdRate.UpdateFreshness.Should().Be("Fresh");
        usdRate.MinutesSinceUpdate.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetAllLatestUpdatedExchangeRates_WithMultipleProvidersSameValidDate_ShouldReturnMostRecentlyCreated()
    {
        // Arrange
        var eurId = await CreateTestCurrencyAsync("EUR");
        var usdId = await CreateTestCurrencyAsync("USD");

        var ecbId = await CreateTestProviderAsync(eurId, "ECB", "European Central Bank");
        var provider2Id = await CreateTestProviderAsync(eurId, "TEST", "Test Provider");

        var today = DateOnly.FromDateTime(DateTime.Today);

        // ECB publishes first
        await CreateTestRatesAsync(ecbId, today, ("EUR", "USD", 1.0800m));

        // Wait to ensure different Created timestamps
        await Task.Delay(100);

        // TEST provider publishes later (same ValidDate, but more recent Created timestamp)
        await CreateTestRatesAsync(provider2Id, today, ("EUR", "USD", 1.0850m));

        // Act
        var query = new GetAllLatestUpdatedExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(1);
        var rate = result.First();
        rate.Rate.Should().Be(1.0850m);
        rate.ProviderCode.Should().Be("TEST"); // Most recently created wins
        rate.ValidDate.Should().Be(today);
    }

    [Fact]
    public async Task GetAllLatestUpdatedExchangeRates_WithDifferentValidDates_ShouldReturnMostRecentlyCreated()
    {
        // Arrange
        var eurId = await CreateTestCurrencyAsync("EUR");
        var usdId = await CreateTestCurrencyAsync("USD");
        var ecbId = await CreateTestProviderAsync(eurId, "ECB", "European Central Bank");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var yesterday = today.AddDays(-1);

        // Create older ValidDate first
        await CreateTestRatesAsync(ecbId, yesterday, ("EUR", "USD", 1.0800m));

        // Wait to ensure different Created timestamps
        await Task.Delay(100);

        // Create newer ValidDate later
        await CreateTestRatesAsync(ecbId, today, ("EUR", "USD", 1.0850m));

        // Act
        var query = new GetAllLatestUpdatedExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(1);
        var rate = result.First();
        rate.Rate.Should().Be(1.0850m);
        rate.ValidDate.Should().Be(today); // Latest Created, which happens to be today's ValidDate
    }

    [Fact]
    public async Task GetAllLatestUpdatedExchangeRates_WhenNewerCreatedHasOlderValidDate_ShouldReturnNewerCreated()
    {
        // Arrange - This tests the key difference between the two queries
        // Scenario: Provider publishes yesterday's rate AFTER today's rate (perhaps a correction)
        var eurId = await CreateTestCurrencyAsync("EUR");
        var usdId = await CreateTestCurrencyAsync("USD");
        var ecbId = await CreateTestProviderAsync(eurId, "ECB", "European Central Bank");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var yesterday = today.AddDays(-1);

        // First, publish today's rate
        await CreateTestRatesAsync(ecbId, today, ("EUR", "USD", 1.0850m));

        // Wait to ensure different Created timestamps
        await Task.Delay(100);

        // Later, publish yesterday's corrected rate (newer Created, older ValidDate)
        await CreateTestRatesAsync(ecbId, yesterday, ("EUR", "USD", 1.0900m));

        // Act
        var query = new GetAllLatestUpdatedExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(1);
        var rate = result.First();
        rate.Rate.Should().Be(1.0900m); // Most recently CREATED
        rate.ValidDate.Should().Be(yesterday); // Even though ValidDate is older!
        rate.ProviderCode.Should().Be("ECB");
    }

    [Fact]
    public async Task GetAllLatestUpdatedExchangeRates_WithMultipleCurrencyPairs_ShouldReturnLatestForEach()
    {
        // Arrange
        var eurId = await CreateTestCurrencyAsync("EUR");
        var usdId = await CreateTestCurrencyAsync("USD");
        var gbpId = await CreateTestCurrencyAsync("GBP");
        var jpyId = await CreateTestCurrencyAsync("JPY");

        var ecbId = await CreateTestProviderAsync(eurId, "ECB", "European Central Bank");
        var provider2Id = await CreateTestProviderAsync(eurId, "TEST", "Test Provider");

        var today = DateOnly.FromDateTime(DateTime.Today);

        // ECB publishes EUR/USD and EUR/GBP
        await CreateTestRatesAsync(ecbId, today,
            ("EUR", "USD", 1.0800m),
            ("EUR", "GBP", 0.8600m));

        await Task.Delay(100);

        // TEST provider publishes EUR/USD and EUR/JPY (EUR/USD is more recent)
        await CreateTestRatesAsync(provider2Id, today,
            ("EUR", "USD", 1.0850m),
            ("EUR", "JPY", 130.00m));

        // Act
        var query = new GetAllLatestUpdatedExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(3);

        // EUR/USD: TEST provider (most recently created)
        var eurUsd = result.First(r => r.TargetCurrencyCode == "USD");
        eurUsd.Rate.Should().Be(1.0850m);
        eurUsd.ProviderCode.Should().Be("TEST");

        // EUR/GBP: ECB (only one available)
        var eurGbp = result.First(r => r.TargetCurrencyCode == "GBP");
        eurGbp.Rate.Should().Be(0.8600m);
        eurGbp.ProviderCode.Should().Be("ECB");

        // EUR/JPY: TEST provider (only one available)
        var eurJpy = result.First(r => r.TargetCurrencyCode == "JPY");
        eurJpy.Rate.Should().Be(130.00m);
        eurJpy.ProviderCode.Should().Be("TEST");
    }

    [Fact]
    public async Task GetAllLatestUpdatedExchangeRates_ShouldShowUpdateFreshnessStatus()
    {
        // Arrange
        var eurId = await CreateTestCurrencyAsync("EUR");
        var usdId = await CreateTestCurrencyAsync("USD");
        var ecbId = await CreateTestProviderAsync(eurId, "ECB", "European Central Bank");

        var today = DateOnly.FromDateTime(DateTime.Today);
        await CreateTestRatesAsync(ecbId, today, ("EUR", "USD", 1.0850m));

        // Act
        var query = new GetAllLatestUpdatedExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(1);
        var rate = result.First();

        // Just created, should be fresh
        rate.UpdateFreshness.Should().Be("Fresh");
        rate.MinutesSinceUpdate.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetAllLatestUpdatedExchangeRates_ShouldCalculateEffectiveRate()
    {
        // Arrange
        var czkId = await CreateTestCurrencyAsync("CZK");
        var eurId = await CreateTestCurrencyAsync("EUR");
        var cnbId = await CreateTestProviderAsync(czkId, "CNB", "Czech National Bank");

        var today = DateOnly.FromDateTime(DateTime.Today);
        await CreateTestRatesAsync(cnbId, today, ("CZK", "EUR", 25.00m));

        // Act
        var query = new GetAllLatestUpdatedExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(1);
        var rate = result.First();
        rate.Rate.Should().Be(25.00m);
        rate.Multiplier.Should().Be(1);
        rate.EffectiveRate.Should().Be(25.00m);
    }
}
