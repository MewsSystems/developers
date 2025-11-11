using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.ExchangeRates;

public class GetAllLatestExchangeRatesQueryTests : IntegrationTestBase
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
    public async Task GetAllLatestExchangeRates_WithNoRates_ShouldReturnEmptyList()
    {
        // Act
        var query = new GetAllLatestExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllLatestExchangeRates_WithSingleProvider_ShouldReturnLatestRates()
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
        var query = new GetAllLatestExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(r => r.ProviderCode == "ECB");
        result.Should().OnlyContain(r => r.ValidDate == today);

        var usdRate = result.First(r => r.TargetCurrencyCode == "USD");
        usdRate.Rate.Should().Be(1.0850m);
        usdRate.BaseCurrencyCode.Should().Be("EUR");
        usdRate.FreshnessStatus.Should().Be("Current");
        usdRate.DaysOld.Should().Be(0);
    }

    [Fact]
    public async Task GetAllLatestExchangeRates_WithMultipleProviders_ShouldReturnLatestByValidDate()
    {
        // Arrange
        var eurId = await CreateTestCurrencyAsync("EUR");
        var usdId = await CreateTestCurrencyAsync("USD");
        var czkId = await CreateTestCurrencyAsync("CZK");

        var ecbId = await CreateTestProviderAsync(eurId, "ECB", "European Central Bank");
        var cnbId = await CreateTestProviderAsync(czkId, "CNB", "Czech National Bank");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var yesterday = today.AddDays(-1);

        // ECB has today's rate
        await CreateTestRatesAsync(ecbId, today, ("EUR", "USD", 1.0850m));

        // CNB has yesterday's rate
        await CreateTestRatesAsync(cnbId, yesterday, ("CZK", "USD", 0.0450m));

        // Wait a bit to ensure Created timestamps are different
        await Task.Delay(100);

        // ECB also has yesterday's rate (should be ignored in favor of today's)
        await CreateTestRatesAsync(ecbId, yesterday, ("EUR", "USD", 1.0800m));

        // Act
        var query = new GetAllLatestExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(2);

        // EUR/USD should be from today (ECB)
        var eurUsd = result.First(r => r.BaseCurrencyCode == "EUR" && r.TargetCurrencyCode == "USD");
        eurUsd.ValidDate.Should().Be(today);
        eurUsd.Rate.Should().Be(1.0850m);
        eurUsd.ProviderCode.Should().Be("ECB");

        // CZK/USD should be from yesterday (CNB - only one available)
        var czkUsd = result.First(r => r.BaseCurrencyCode == "CZK" && r.TargetCurrencyCode == "USD");
        czkUsd.ValidDate.Should().Be(yesterday);
        czkUsd.Rate.Should().Be(0.0450m);
        czkUsd.ProviderCode.Should().Be("CNB");
    }

    [Fact]
    public async Task GetAllLatestExchangeRates_WithSameCurrencyPairMultipleProviders_ShouldReturnLatestByValidDate()
    {
        // Arrange
        var eurId = await CreateTestCurrencyAsync("EUR");
        var usdId = await CreateTestCurrencyAsync("USD");

        var ecbId = await CreateTestProviderAsync(eurId, "ECB", "European Central Bank");
        var provider2Id = await CreateTestProviderAsync(eurId, "TEST", "Test Provider");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var yesterday = today.AddDays(-1);

        // ECB has yesterday's rate
        await CreateTestRatesAsync(ecbId, yesterday, ("EUR", "USD", 1.0800m));

        // TEST provider has today's rate (should win)
        await CreateTestRatesAsync(provider2Id, today, ("EUR", "USD", 1.0900m));

        // Act
        var query = new GetAllLatestExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(1);
        var rate = result.First();
        rate.ValidDate.Should().Be(today);
        rate.Rate.Should().Be(1.0900m);
        rate.ProviderCode.Should().Be("TEST"); // Latest ValidDate wins
    }

    [Fact]
    public async Task GetAllLatestExchangeRates_WithOldRates_ShouldShowCorrectFreshnessStatus()
    {
        // Arrange
        var eurId = await CreateTestCurrencyAsync("EUR");
        var usdId = await CreateTestCurrencyAsync("USD");
        var gbpId = await CreateTestCurrencyAsync("GBP");
        var jpyId = await CreateTestCurrencyAsync("JPY");
        var ecbId = await CreateTestProviderAsync(eurId, "ECB", "European Central Bank");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var twoDaysAgo = today.AddDays(-2);
        var fiveDaysAgo = today.AddDays(-5);
        var tenDaysAgo = today.AddDays(-10);

        await CreateTestRatesAsync(ecbId, today, ("EUR", "USD", 1.0850m));
        await CreateTestRatesAsync(ecbId, twoDaysAgo, ("EUR", "GBP", 0.8650m));
        await CreateTestRatesAsync(ecbId, fiveDaysAgo, ("EUR", "JPY", 130.50m));

        // Act
        var query = new GetAllLatestExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(3);

        var usdRate = result.First(r => r.TargetCurrencyCode == "USD");
        usdRate.FreshnessStatus.Should().Be("Current");
        usdRate.DaysOld.Should().Be(0);

        var gbpRate = result.First(r => r.TargetCurrencyCode == "GBP");
        gbpRate.FreshnessStatus.Should().Be("Week Old");
        gbpRate.DaysOld.Should().Be(2);

        var jpyRate = result.First(r => r.TargetCurrencyCode == "JPY");
        jpyRate.FreshnessStatus.Should().Be("Week Old");
        jpyRate.DaysOld.Should().Be(5);
    }

    [Fact]
    public async Task GetAllLatestExchangeRates_ShouldCalculateEffectiveRate()
    {
        // Arrange
        var czkId = await CreateTestCurrencyAsync("CZK");
        var eurId = await CreateTestCurrencyAsync("EUR");
        var cnbId = await CreateTestProviderAsync(czkId, "CNB", "Czech National Bank");

        var today = DateOnly.FromDateTime(DateTime.Today);

        // CNB uses multipliers (e.g., 1 EUR = 25 CZK, so rate=25, multiplier=1)
        await CreateTestRatesAsync(cnbId, today, ("CZK", "EUR", 25.00m));

        // Act
        var query = new GetAllLatestExchangeRatesQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().HaveCount(1);
        var rate = result.First();
        rate.Rate.Should().Be(25.00m);
        rate.Multiplier.Should().Be(1);
        rate.EffectiveRate.Should().Be(25.00m); // 25/1 = 25
    }
}
