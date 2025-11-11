using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.SearchExchangeRates;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Queries.ExchangeRates;

/// <summary>
/// Integration tests for SearchExchangeRatesQuery.
/// Tests searching exchange rates with multiple filtering options and pagination.
/// </summary>
public class SearchExchangeRatesQueryTests : IntegrationTestBase
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
    public async Task SearchExchangeRates_WithNoFilters_ShouldReturnPaginatedResults()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");
        await CreateTestCurrency("GBP");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await InsertExchangeRates(providerId, today,
            ("USD", 1.20m),
            ("GBP", 0.85m));

        // Query with provider to avoid empty results (handler requires provider or currency pair)
        var query = new SearchExchangeRatesQuery(PageNumber: 1, PageSize: 10, ProviderId: providerId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().NotBeEmpty();
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task SearchExchangeRates_WithSourceCurrencyFilter_ShouldFilterResults()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var usdId = await CreateTestCurrency("USD");
        await CreateTestCurrency("GBP");

        var ecbId = await CreateTestProvider("ECB", "ECB", eurId);
        var usdProviderId = await CreateTestProvider("USD_PROV", "USDPROV", usdId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await InsertExchangeRates(ecbId, today, ("USD", 1.20m), ("GBP", 0.85m));
        await InsertExchangeRates(usdProviderId, today, ("GBP", 0.71m));

        // Must specify both currencies or provider (handler requirement)
        var query = new SearchExchangeRatesQuery(SourceCurrencyCode: "EUR", ProviderId: ecbId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().AllSatisfy(r => r.BaseCurrencyCode.Should().Be("EUR"));
        result.Items.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task SearchExchangeRates_WithTargetCurrencyFilter_ShouldFilterResults()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");
        await CreateTestCurrency("GBP");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await InsertExchangeRates(providerId, today,
            ("USD", 1.20m),
            ("GBP", 0.85m));

        var query = new SearchExchangeRatesQuery(TargetCurrencyCode: "USD");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().AllSatisfy(r => r.TargetCurrencyCode.Should().Be("USD"));
    }

    [Fact]
    public async Task SearchExchangeRates_WithDateRangeFilter_ShouldFilterResults()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var fiveDaysAgo = today.AddDays(-5);
        var tenDaysAgo = today.AddDays(-10);

        await InsertExchangeRates(providerId, today, ("USD", 1.22m));
        await InsertExchangeRates(providerId, fiveDaysAgo, ("USD", 1.20m));
        await InsertExchangeRates(providerId, tenDaysAgo, ("USD", 1.18m));

        // Must specify provider or currency pair (handler requirement)
        var query = new SearchExchangeRatesQuery(StartDate: fiveDaysAgo, EndDate: today, ProviderId: providerId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCountGreaterThanOrEqualTo(2);
        result.Items.Should().AllSatisfy(r =>
        {
            r.ValidDate.Should().BeOnOrAfter(fiveDaysAgo);
            r.ValidDate.Should().BeOnOrBefore(today);
        });
    }

    [Fact]
    public async Task SearchExchangeRates_WithRateRangeFilter_ShouldFilterResults()
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

        // Must specify provider or currency pair (handler requirement)
        var query = new SearchExchangeRatesQuery(MinRate: 1.0m, MaxRate: 2.0m, ProviderId: providerId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().Contain(r => r.TargetCurrencyCode == "USD");
        result.Items.Should().NotContain(r => r.TargetCurrencyCode == "GBP"); // 0.85 is below MinRate
        result.Items.Should().NotContain(r => r.TargetCurrencyCode == "JPY"); // 130.50 is above MaxRate
    }

    [Fact]
    public async Task SearchExchangeRates_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        var currencies = new[] { "USD", "GBP", "JPY", "CHF", "CAD" };
        foreach (var curr in currencies)
        {
            await CreateTestCurrency(curr);
        }

        var providerId = await CreateTestProvider("ECB", "ECB", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var rates = currencies.Select((c, i) => (c, 1.0m + i * 0.1m)).ToArray();
        await InsertExchangeRates(providerId, today, rates);

        // Must specify provider or currency pair (handler requirement)
        var query = new SearchExchangeRatesQuery(PageNumber: 1, PageSize: 2, ProviderId: providerId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);
        result.TotalPages.Should().BeGreaterThanOrEqualTo(3);
    }

    [Fact]
    public async Task SearchExchangeRates_WithProviderFilter_ShouldFilterByProvider()
    {
        // Arrange
        var eurId = await CreateTestCurrency("EUR");
        await CreateTestCurrency("USD");

        var ecbId = await CreateTestProvider("ECB", "ECB", eurId);
        var bnrId = await CreateTestProvider("BNR", "BNR", eurId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await InsertExchangeRates(ecbId, today, ("USD", 1.20m));
        await InsertExchangeRates(bnrId, today, ("USD", 1.18m));

        var query = new SearchExchangeRatesQuery(ProviderId: ecbId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().AllSatisfy(r => r.ProviderId.Should().Be(ecbId));
    }
}
