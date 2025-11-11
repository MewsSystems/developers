using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Queries.Providers.GetProviderConfiguration;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Providers;

public class GetProviderConfigurationQueryTests : IntegrationTestBase
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
    public async Task GetProviderConfiguration_WithValidId_ShouldReturnConfiguration()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("USD");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderConfigurationQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(providerId);
        result.Configurations.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProviderConfiguration_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var query = new GetProviderConfigurationQuery(99999);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderConfiguration_ShouldIncludeAllDetails()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("EUR");
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var query = new GetProviderConfigurationQuery(providerId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Provider");
        result.Code.Should().NotBeNullOrEmpty();
        result.Url.Should().Be("https://api.example.com/rates");
        result.BaseCurrencyCode.Should().Be("EUR");
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetProviderConfiguration_MultipleProviders_ShouldReturnCorrectConfiguration()
    {
        // Arrange
        var usdId = await CreateTestCurrencyAsync("USD");
        var eurId = await CreateTestCurrencyAsync("EUR");

        var provider1Id = await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8));
        var provider2Id = await CreateTestProviderAsync(eurId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8));

        // Act
        var query1 = new GetProviderConfigurationQuery(provider1Id);
        var query2 = new GetProviderConfigurationQuery(provider2Id);

        var result1 = await Mediator.Send(query1);
        var result2 = await Mediator.Send(query2);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1!.Id.Should().Be(provider1Id);
        result2!.Id.Should().Be(provider2Id);
        result1.BaseCurrencyCode.Should().Be("USD");
        result2.BaseCurrencyCode.Should().Be("EUR");
    }
}
