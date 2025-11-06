using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetExchangeRateHistory;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Queries.ExchangeRates;

/// <summary>
/// Integration tests for GetExchangeRateHistoryQuery.
/// Tests retrieving historical exchange rates for a currency pair within a date range.
/// </summary>
public class GetExchangeRateHistoryQueryTests : IntegrationTestBase
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

        var provider = await DbContext.ExchangeRateProviders
            .FirstOrDefaultAsync(p => p.Code == code.ToUpper());

        return provider?.Id ?? result.Value;
    }

    private async Task InsertExchangeRates(int providerId, DateOnly validDate, params (string target, decimal rate)[] rates)
    {
        var ratesList = rates.Select(r => new ExchangeRateItemDto("EUR", r.target, r.rate, 1)).ToList();

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: providerId,
            ValidDate: validDate,
            Rates: ratesList
        );

        await Mediator.Send(command);
    }

    [Fact]
    public async Task GetExchangeRateHistory_WithDateRange_ShouldReturnRatesInRange()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var threeDaysAgo = today.AddDays(-3);
        var fiveDaysAgo = today.AddDays(-5);
        var sevenDaysAgo = today.AddDays(-7);

        // Insert rates for various dates
        await InsertExchangeRates(providerId, today, ("USD", 1.22m));
        await InsertExchangeRates(providerId, threeDaysAgo, ("USD", 1.21m));
        await InsertExchangeRates(providerId, fiveDaysAgo, ("USD", 1.20m));
        await InsertExchangeRates(providerId, sevenDaysAgo, ("USD", 1.19m));

        var query = new GetExchangeRateHistoryQuery("EUR", "USD", fiveDaysAgo, today);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        var history = result.ToList();
        history.Should().HaveCount(3); // 5 days ago, 3 days ago, today
        history.Should().Contain(r => r.ValidDate == today && r.Rate == 1.22m);
        history.Should().Contain(r => r.ValidDate == threeDaysAgo && r.Rate == 1.21m);
        history.Should().Contain(r => r.ValidDate == fiveDaysAgo && r.Rate == 1.20m);
        history.Should().NotContain(r => r.ValidDate == sevenDaysAgo);
    }

    [Fact]
    public async Task GetExchangeRateHistory_WithNoRatesInRange_ShouldReturnEmpty()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var tenDaysAgo = today.AddDays(-10);

        await InsertExchangeRates(providerId, tenDaysAgo, ("USD", 1.20m));

        // Query for last 5 days
        var fiveDaysAgo = today.AddDays(-5);
        var query = new GetExchangeRateHistoryQuery("EUR", "USD", fiveDaysAgo, today);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetExchangeRateHistory_WithSpecificProvider_ShouldOnlyReturnThatProvider()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var ecbId = await CreateTestProvider("ECB", "ECB", eurId);
        var bnrId = await CreateTestProvider("BNR", "BNR", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        await InsertExchangeRates(ecbId, today, ("USD", 1.20m));
        await InsertExchangeRates(ecbId, yesterday, ("USD", 1.19m));
        await InsertExchangeRates(bnrId, today, ("USD", 1.18m));

        var query = new GetExchangeRateHistoryQuery("EUR", "USD", yesterday, today, ProviderId: ecbId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        var history = result.ToList();
        history.Should().HaveCount(2);
        history.Should().AllSatisfy(r => r.ProviderId.Should().Be(ecbId));
    }

    [Fact]
    public async Task GetExchangeRateHistory_ShouldBeOrderedByDate()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var threeDaysAgo = today.AddDays(-3);
        var fiveDaysAgo = today.AddDays(-5);

        // Insert rates in non-chronological order
        await InsertExchangeRates(providerId, today, ("USD", 1.22m));
        await InsertExchangeRates(providerId, fiveDaysAgo, ("USD", 1.20m));
        await InsertExchangeRates(providerId, threeDaysAgo, ("USD", 1.21m));

        var query = new GetExchangeRateHistoryQuery("EUR", "USD", fiveDaysAgo, today);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        var history = result.ToList();
        history.Should().HaveCount(3);
        history[0].ValidDate.Should().Be(fiveDaysAgo);
        history[1].ValidDate.Should().Be(threeDaysAgo);
        history[2].ValidDate.Should().Be(today);
    }
}
