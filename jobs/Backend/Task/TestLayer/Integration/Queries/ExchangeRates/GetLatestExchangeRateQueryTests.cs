using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetLatestExchangeRate;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Queries.ExchangeRates;

/// <summary>
/// Integration tests for GetLatestExchangeRateQuery.
/// Tests retrieving the most recent exchange rate for a currency pair.
/// </summary>
public class GetLatestExchangeRateQueryTests : IntegrationTestBase
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
    public async Task GetLatestExchangeRate_WithExistingRate_ShouldReturnLatest()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        // Insert rates for both days
        await InsertExchangeRates(providerId, yesterday, ("USD", 1.19m));
        await InsertExchangeRates(providerId, today, ("USD", 1.20m));

        var query = new GetLatestExchangeRateQuery("EUR", "USD");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Rate.Should().Be(1.20m);
        result.ValidDate.Should().Be(today);
    }

    [Fact]
    public async Task GetLatestExchangeRate_WithNonExistentPair_ShouldReturnNull()
    {
        // Arrange
        await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var query = new GetLatestExchangeRateQuery("EUR", "JPY");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetLatestExchangeRate_WithSpecificProvider_ShouldReturnOnlyThatProvider()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var ecbId = await CreateTestProvider("ECB", "ECB", eurId);
        var bnrId = await CreateTestProvider("BNR", "BNR", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        await InsertExchangeRates(ecbId, today, ("USD", 1.20m));
        await InsertExchangeRates(bnrId, today, ("USD", 1.18m));

        var query = new GetLatestExchangeRateQuery("EUR", "USD", ProviderId: ecbId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Rate.Should().Be(1.20m);
        result.ProviderId.Should().Be(ecbId);
    }

    [Fact]
    public async Task GetLatestExchangeRate_WithMultipleDates_ShouldReturnMostRecent()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var threeDaysAgo = today.AddDays(-3);
        var fiveDaysAgo = today.AddDays(-5);

        // Insert rates in non-chronological order
        await InsertExchangeRates(providerId, threeDaysAgo, ("USD", 1.21m));
        await InsertExchangeRates(providerId, fiveDaysAgo, ("USD", 1.19m));
        await InsertExchangeRates(providerId, today, ("USD", 1.22m));

        var query = new GetLatestExchangeRateQuery("EUR", "USD");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Rate.Should().Be(1.22m);
        result.ValidDate.Should().Be(today);
    }
}
