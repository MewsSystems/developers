using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Queries.Providers.GetProviderById;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Providers;

public class GetProviderByIdQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrencyAsync(string code = "USD")
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    private async Task<int> CreateTestProviderAsync(int currencyId, string? code = null, bool requiresAuth = false)
    {
        code ??= Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);
        var command = new CreateExchangeRateProviderCommand(
            Name: "Test Provider",
            Code: code,
            Url: "https://api.example.com/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: requiresAuth,
            ApiKeyVaultReference: requiresAuth ? "vault-key-123" : null
        );
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    [Fact]
    public async Task GetProviderById_WithValidId_ShouldReturnProvider()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("USD");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderByIdQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(providerId);
        result.Name.Should().Be("Test Provider");
        result.Url.Should().Be("https://api.example.com/rates");
        result.BaseCurrencyCode.Should().Be("USD");
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetProviderById_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var query = new GetProviderByIdQuery(99999);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderById_WithAuthentication_ShouldIncludeAuthDetails()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("EUR");
        var providerId = await CreateTestProviderAsync(currencyId, requiresAuth: true);

        // Act
        var query = new GetProviderByIdQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.RequiresAuthentication.Should().BeTrue();
        result.ApiKeyVaultReference.Should().Be("vault-key-123");
    }

    [Fact]
    public async Task GetProviderById_WithoutAuthentication_ShouldHaveNullApiKey()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("GBP");
        var providerId = await CreateTestProviderAsync(currencyId, requiresAuth: false);

        // Act
        var query = new GetProviderByIdQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.RequiresAuthentication.Should().BeFalse();
        result.ApiKeyVaultReference.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderById_ShouldIncludeTimestamps()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("JPY");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderByIdQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Created.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        result.LastSuccessfulFetch.Should().BeNull(); // No fetches yet
        result.LastFailedFetch.Should().BeNull(); // No failures yet
    }

    [Fact]
    public async Task GetProviderById_ShouldIncludeConfigurations()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("CHF");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderByIdQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Configurations.Should().NotBeNull();
        result.Configurations.Should().BeOfType<List<ApplicationLayer.DTOs.ExchangeRateProviders.ProviderConfigurationDto>>();
    }
}
