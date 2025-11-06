using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.ExchangeRateProviders;

public class DeactivateProviderCommandTests : IntegrationTestBase
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
    public async Task DeactivateProvider_WhenProviderIsActive_ShouldDeactivateProvider()
    {
        // Arrange - Provider is active by default
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var command = new DeactivateProviderCommand(providerId);
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue(result.Error);

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeactivateProvider_WhenProviderIsAlreadyDeactivated_ShouldSucceed()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // First deactivation
        var firstCommand = new DeactivateProviderCommand(providerId);
        await Mediator.Send(firstCommand);

        // Act - Second deactivation
        var secondCommand = new DeactivateProviderCommand(providerId);
        var result = await Mediator.Send(secondCommand);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeactivateProvider_WithNonExistentProvider_ShouldFail()
    {
        // Arrange
        var command = new DeactivateProviderCommand(99999);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task DeactivateProvider_MultipleProviders_ShouldDeactivateAll()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var provider1Id = await CreateTestProviderAsync(currencyId);
        var provider2Id = await CreateTestProviderAsync(currencyId);
        var provider3Id = await CreateTestProviderAsync(currencyId);

        // Act
        var result1 = await Mediator.Send(new DeactivateProviderCommand(provider1Id));
        var result2 = await Mediator.Send(new DeactivateProviderCommand(provider2Id));
        var result3 = await Mediator.Send(new DeactivateProviderCommand(provider3Id));

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        result3.IsSuccess.Should().BeTrue();

        var providers = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Where(p => new[] { provider1Id, provider2Id, provider3Id }.Contains(p.Id))
            .ToListAsync();

        providers.Should().HaveCount(3);
        providers.Should().OnlyContain(p => !p.IsActive);
    }
}
