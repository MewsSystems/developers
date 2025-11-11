using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Queries.Providers.GetProviderStatistics;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Providers;

public class GetProviderStatisticsQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrencyAsync(string code = "USD")
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    private async Task<int> CreateTestProviderAsync(int currencyId, string? code = null)
    {
        code ??= Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);
        var command = new CreateExchangeRateProviderCommand(
            Name: "Test Provider",
            Code: code,
            Url: "https://api.example.com/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    [Fact]
    public async Task GetProviderStatistics_WithValidId_ShouldReturnStatistics()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("USD");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderStatisticsQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.ProviderId.Should().Be(providerId);
        result.ProviderName.Should().Be("Test Provider");
    }

    [Fact]
    public async Task GetProviderStatistics_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var query = new GetProviderStatisticsQuery(99999);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderStatistics_NewProvider_ShouldHaveZeroStats()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("EUR");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderStatisticsQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.TotalRatesProvided.Should().Be(0);
        result.SuccessRate.Should().BeGreaterThanOrEqualTo(0);
        result.OldestRateDate.Should().BeNull();
        result.NewestRateDate.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderStatistics_ShouldIncludeProviderDetails()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("GBP");
        var uniqueCode = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);
        var providerId = await CreateTestProviderAsync(currencyId, uniqueCode);

        // Act
        var query = new GetProviderStatisticsQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.ProviderId.Should().Be(providerId);
        result.ProviderCode.Should().Be(uniqueCode);
        result.ProviderName.Should().Be("Test Provider");
    }

    [Fact]
    public async Task GetProviderStatistics_ShouldIncludeAllFields()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("JPY");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderStatisticsQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.ProviderId.Should().BeGreaterThan(0);
        result.ProviderCode.Should().NotBeNullOrEmpty();
        result.ProviderName.Should().NotBeNullOrEmpty();
        result.TotalRatesProvided.Should().BeGreaterThanOrEqualTo(0);
        result.TotalFetchAttempts.Should().BeGreaterThanOrEqualTo(0);
        result.SuccessfulFetches.Should().BeGreaterThanOrEqualTo(0);
        result.FailedFetches.Should().BeGreaterThanOrEqualTo(0);
        result.SuccessRate.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetProviderStatistics_MultipleProviders_ShouldReturnCorrectStats()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("CHF");
        var provider1Id = await CreateTestProviderAsync(currencyId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8));
        var provider2Id = await CreateTestProviderAsync(currencyId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8));

        // Act
        var query1 = new GetProviderStatisticsQuery(provider1Id);
        var query2 = new GetProviderStatisticsQuery(provider2Id);

        var result1 = await Mediator.Send(query1);
        var result2 = await Mediator.Send(query2);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1!.ProviderId.Should().Be(provider1Id);
        result2!.ProviderId.Should().Be(provider2Id);
    }
}
