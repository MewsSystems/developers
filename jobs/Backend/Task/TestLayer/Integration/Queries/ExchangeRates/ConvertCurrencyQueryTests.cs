using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Queries.ExchangeRates;

/// <summary>
/// Integration tests for ConvertCurrencyQuery.
/// Tests currency conversion using exchange rates.
/// </summary>
public class ConvertCurrencyQueryTests : IntegrationTestBase
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
    public async Task ConvertCurrency_WithValidRate_ShouldConvertCorrectly()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await InsertExchangeRates(providerId, today, ("USD", 1.20m));

        var query = new ConvertCurrencyQuery("EUR", "USD", 100m);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.SourceAmount.Should().Be(100m);
        result.SourceCurrencyCode.Should().Be("EUR");
        result.TargetCurrencyCode.Should().Be("USD");
        result.TargetAmount.Should().Be(120m); // 100 * 1.20
        result.Rate.Should().Be(1.20m);
        result.EffectiveRate.Should().Be(1.20m);
    }

    [Fact]
    public async Task ConvertCurrency_WithMultiplier_ShouldCalculateCorrectly()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("JPY");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // JPY rate with multiplier 100 (meaning 100 EUR = 13050 JPY, so 1 EUR = 130.50 JPY)
        var ratesList = new List<ExchangeRateItemDto>
        {
            new("EUR", "JPY", 13050m, 100)
        };

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: providerId,
            ValidDate: today,
            Rates: ratesList
        );

        await Mediator.Send(command);

        var query = new ConvertCurrencyQuery("EUR", "JPY", 100m);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.SourceAmount.Should().Be(100m);
        result.TargetAmount.Should().Be(13050m); // 100 * (13050 / 100)
        result.Rate.Should().Be(13050m);
        result.Multiplier.Should().Be(100);
        result.EffectiveRate.Should().Be(130.50m);
    }

    [Fact]
    public async Task ConvertCurrency_WithSpecificDate_ShouldUseRateFromThatDate()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        await InsertExchangeRates(providerId, today, ("USD", 1.22m));
        await InsertExchangeRates(providerId, yesterday, ("USD", 1.20m));

        var query = new ConvertCurrencyQuery("EUR", "USD", 100m, Date: yesterday);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.TargetAmount.Should().Be(120m); // Uses yesterday's rate
        result.ValidDate.Should().Be(yesterday);
    }

    [Fact]
    public async Task ConvertCurrency_WithSpecificProvider_ShouldUseOnlyThatProvider()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var ecbId = await CreateTestProvider("ECB", "ECB", eurId);
        var bnrId = await CreateTestProvider("BNR", "BNR", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        await InsertExchangeRates(ecbId, today, ("USD", 1.20m));
        await InsertExchangeRates(bnrId, today, ("USD", 1.18m));

        var query = new ConvertCurrencyQuery("EUR", "USD", 100m, ProviderId: ecbId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.TargetAmount.Should().Be(120m); // Uses ECB rate
        result.ProviderId.Should().Be(ecbId);
        result.ProviderName.Should().Be("ECB");
    }

    [Fact]
    public async Task ConvertCurrency_WithNoMatchingRate_ShouldThrowException()
    {
        // Arrange
        await CreateTestCurrency("EUR");
        await CreateTestCurrency("JPY");

        var query = new ConvertCurrencyQuery("EUR", "JPY", 100m);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.NotFoundException>(
            async () => await Mediator.Send(query)
        );

        exception.Message.Should().Contain("exchange rate");
    }

    [Fact]
    public async Task ConvertCurrency_WithZeroAmount_ShouldFailValidation()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await InsertExchangeRates(providerId, today, ("USD", 1.20m));

        var query = new ConvertCurrencyQuery("EUR", "USD", 0m);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );
    }

    [Fact]
    public async Task ConvertCurrency_WithLargeAmount_ShouldCalculateCorrectly()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await InsertExchangeRates(providerId, today, ("USD", 1.20m));

        var query = new ConvertCurrencyQuery("EUR", "USD", 1000000m);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.SourceAmount.Should().Be(1000000m);
        result.TargetAmount.Should().Be(1200000m);
    }
}
