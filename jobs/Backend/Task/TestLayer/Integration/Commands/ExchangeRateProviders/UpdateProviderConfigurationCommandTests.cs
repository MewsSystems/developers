using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.UpdateProviderConfiguration;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.ExchangeRateProviders;

public class UpdateProviderConfigurationCommandTests : IntegrationTestBase
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
    public async Task UpdateProviderConfiguration_UpdateName_ShouldUpdateName()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var command = new UpdateProviderConfigurationCommand(
            ProviderId: providerId,
            Name: "Updated Provider Name",
            Url: null,
            RequiresAuthentication: null,
            ApiKeyVaultReference: null
        );
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.Name.Should().Be("Updated Provider Name");
        provider.Url.Should().Be("https://api.example.com/rates"); // Unchanged
    }

    [Fact]
    public async Task UpdateProviderConfiguration_UpdateUrl_ShouldUpdateUrl()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var command = new UpdateProviderConfigurationCommand(
            ProviderId: providerId,
            Name: null,
            Url: "https://api.newprovider.com/rates",
            RequiresAuthentication: null,
            ApiKeyVaultReference: null
        );
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.Url.Should().Be("https://api.newprovider.com/rates");
        provider.Name.Should().Be("Test Provider"); // Unchanged
    }

    [Fact]
    public async Task UpdateProviderConfiguration_UpdateAuthentication_ShouldUpdateAuthSettings()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var command = new UpdateProviderConfigurationCommand(
            ProviderId: providerId,
            Name: null,
            Url: null,
            RequiresAuthentication: true,
            ApiKeyVaultReference: "new-vault-key-789"
        );
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.RequiresAuthentication.Should().BeTrue();
        provider.ApiKeyVaultReference.Should().Be("new-vault-key-789");
    }

    [Fact]
    public async Task UpdateProviderConfiguration_UpdateMultipleFields_ShouldUpdateAll()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var command = new UpdateProviderConfigurationCommand(
            ProviderId: providerId,
            Name: "Completely Updated Provider",
            Url: "https://api.updated.com/v2/rates",
            RequiresAuthentication: true,
            ApiKeyVaultReference: "vault-key-complete-123"
        );
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.Name.Should().Be("Completely Updated Provider");
        provider.Url.Should().Be("https://api.updated.com/v2/rates");
        provider.RequiresAuthentication.Should().BeTrue();
        provider.ApiKeyVaultReference.Should().Be("vault-key-complete-123");
    }

    [Fact]
    public async Task UpdateProviderConfiguration_WithNonExistentProvider_ShouldFail()
    {
        // Arrange
        var command = new UpdateProviderConfigurationCommand(
            ProviderId: 99999,
            Name: "Does Not Matter",
            Url: null,
            RequiresAuthentication: null,
            ApiKeyVaultReference: null
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task UpdateProviderConfiguration_DisableAuthentication_ShouldDisableAuth()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var code = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);

        // Create provider with authentication
        var createCommand = new CreateExchangeRateProviderCommand(
            Name: "Authenticated Provider",
            Code: code,
            Url: "https://api.secured.com/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: true,
            ApiKeyVaultReference: "vault-key-456"
        );
        var createResult = await Mediator.Send(createCommand);
        var providerId = createResult.Value;

        // Act - Disable authentication
        var updateCommand = new UpdateProviderConfigurationCommand(
            ProviderId: providerId,
            Name: null,
            Url: null,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );
        var result = await Mediator.Send(updateCommand);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.RequiresAuthentication.Should().BeFalse();
    }
}
