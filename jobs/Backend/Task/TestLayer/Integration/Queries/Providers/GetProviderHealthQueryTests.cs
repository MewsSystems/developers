using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Queries.Providers.GetProviderHealth;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Providers;

public class GetProviderHealthQueryTests : IntegrationTestBase
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
    public async Task GetProviderHealth_WithValidId_ShouldReturnHealth()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("USD");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderHealthQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.ProviderId.Should().Be(providerId);
        result.ProviderName.Should().Be("Test Provider");
        result.ConsecutiveFailures.Should().Be(0);
        // New providers that have never fetched are not considered healthy
        result.IsHealthy.Should().BeFalse();
        result.Status.Should().Be("Never Fetched");
    }

    [Fact]
    public async Task GetProviderHealth_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var query = new GetProviderHealthQuery(99999);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderHealth_NewProvider_ShouldHaveNoFetchHistory()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("EUR");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderHealthQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.LastSuccessfulFetch.Should().BeNull();
        result.LastFailedFetch.Should().BeNull();
        result.TimeSinceLastSuccess.Should().BeNull();
        result.TimeSinceLastFailure.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderHealth_ShouldIncludeProviderDetails()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("GBP");
        var uniqueCode = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);
        var providerId = await CreateTestProviderAsync(currencyId, uniqueCode);

        // Act
        var query = new GetProviderHealthQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.ProviderId.Should().Be(providerId);
        result.ProviderCode.Should().Be(uniqueCode);
        result.ProviderName.Should().Be("Test Provider");
        result.Status.Should().NotBeNullOrEmpty();
    }
}
