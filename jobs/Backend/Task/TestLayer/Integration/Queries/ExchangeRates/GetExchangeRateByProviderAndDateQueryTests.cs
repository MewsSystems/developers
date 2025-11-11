using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetExchangeRateByProviderAndDate;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Queries.ExchangeRates;

/// <summary>
/// Integration tests for GetExchangeRateByProviderAndDateQuery.
/// Tests retrieving all exchange rates from a specific provider for a given date.
/// </summary>
public class GetExchangeRateByProviderAndDateQueryTests : IntegrationTestBase
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
    public async Task GetExchangeRateByProviderAndDate_WithValidData_ShouldReturnRates()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");
        await CreateTestCurrency("GBP");
        await CreateTestCurrency("JPY");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await InsertExchangeRates(providerId, today,
            ("USD", 1.20m),
            ("GBP", 0.85m),
            ("JPY", 130.50m));

        var query = new GetExchangeRateByProviderAndDateQuery(providerId, today);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        var rates = result.ToList();
        rates.Should().HaveCount(3);
        rates.Should().Contain(r => r.TargetCurrencyCode == "USD" && r.Rate == 1.20m);
        rates.Should().Contain(r => r.TargetCurrencyCode == "GBP" && r.Rate == 0.85m);
        rates.Should().Contain(r => r.TargetCurrencyCode == "JPY" && r.Rate == 130.50m);
    }

    [Fact]
    public async Task GetExchangeRateByProviderAndDate_WithNonExistentDate_ShouldReturnEmpty()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        await InsertExchangeRates(providerId, yesterday, ("USD", 1.20m));

        var query = new GetExchangeRateByProviderAndDateQuery(providerId, today);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetExchangeRateByProviderAndDate_WithDifferentProvider_ShouldNotReturnOtherProviderRates()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var ecbId = await CreateTestProvider("ECB", "ECB", eurId);
        var bnrId = await CreateTestProvider("BNR", "BNR", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        await InsertExchangeRates(ecbId, today, ("USD", 1.20m));
        await InsertExchangeRates(bnrId, today, ("USD", 1.18m));

        var query = new GetExchangeRateByProviderAndDateQuery(ecbId, today);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        var rates = result.ToList();
        rates.Should().HaveCount(1);
        rates.First().Rate.Should().Be(1.20m);
        rates.First().ProviderId.Should().Be(ecbId);
    }

    [Fact]
    public async Task GetExchangeRateByProviderAndDate_WithNonExistentProvider_ShouldReturnEmpty()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var query = new GetExchangeRateByProviderAndDateQuery(999, today);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeEmpty();
    }
}
