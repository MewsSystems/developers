using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Queries.ExchangeRates;

/// <summary>
/// Integration tests for GetCurrentExchangeRatesQuery.
/// Tests retrieving current (today's) exchange rates.
/// </summary>
public class GetCurrentExchangeRatesQueryTests : IntegrationTestBase
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

        // Query back to get the actual database-generated ID
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
    public async Task GetCurrentExchangeRates_WithTodayRates_ShouldReturnRates()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");
        await CreateTestCurrency("GBP");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        // Use local date to match server's GETDATE() in view
        var today = DateOnly.FromDateTime(DateTime.Now);
        await InsertExchangeRates(providerId, today,
            ("USD", 1.20m),
            ("GBP", 0.85m));

        var query = new GetCurrentExchangeRatesQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        var rates = result.ToList();
        rates.Should().HaveCountGreaterThanOrEqualTo(2);
        rates.Should().Contain(r => r.TargetCurrencyCode == "USD" && r.Rate == 1.20m);
        rates.Should().Contain(r => r.TargetCurrencyCode == "GBP" && r.Rate == 0.85m);
        rates.Should().AllSatisfy(r => r.ValidDate.Should().Be(today));
    }

    [Fact]
    public async Task GetCurrentExchangeRates_WithNoTodayRates_ShouldReturnEmpty()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        // Insert rates for yesterday (use local date to match server's GETDATE())
        var yesterday = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        await InsertExchangeRates(providerId, yesterday, ("USD", 1.20m));

        var query = new GetCurrentExchangeRatesQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCurrentExchangeRates_WithInactiveProvider_ShouldNotReturnThoseRates()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        // Use local date to match server's GETDATE() in view
        var today = DateOnly.FromDateTime(DateTime.Now);
        await InsertExchangeRates(providerId, today, ("USD", 1.20m));

        // Deactivate the provider
        var provider = await DbContext.ExchangeRateProviders.FindAsync(providerId);
        provider!.IsActive = false;
        await DbContext.SaveChangesAsync();

        var query = new GetCurrentExchangeRatesQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCurrentExchangeRates_WithMultipleProviders_ShouldReturnAllActiveProviderRates()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var usdId = await CreateTestCurrency("USD");
        await CreateTestCurrency("GBP");

        var ecbProviderId = await CreateTestProvider("ECB", "ECB", eurId);
        var usdProviderId = await CreateTestProvider("US Provider", "USD_PROV", usdId);

        // Use local date to match server's GETDATE() in view
        var today = DateOnly.FromDateTime(DateTime.Now);
        await InsertExchangeRates(ecbProviderId, today, ("USD", 1.20m));
        await InsertExchangeRates(usdProviderId, today, ("GBP", 0.75m));

        var query = new GetCurrentExchangeRatesQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        var rates = result.ToList();
        rates.Should().HaveCountGreaterThanOrEqualTo(2);
        rates.Should().Contain(r => r.TargetCurrencyCode == "USD");
        rates.Should().Contain(r => r.TargetCurrencyCode == "GBP");
    }
}
