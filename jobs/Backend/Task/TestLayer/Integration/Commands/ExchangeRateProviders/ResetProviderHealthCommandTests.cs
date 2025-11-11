using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.ResetProviderHealth;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.ExchangeRateProviders;

public class ResetProviderHealthCommandTests : IntegrationTestBase
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
    public async Task ResetProviderHealth_WithFailedProvider_ShouldResetHealth()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Simulate failed fetches by updating the provider directly
        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);
        provider!.ConsecutiveFailures = 5;
        provider.LastFailedFetch = DateTimeOffset.UtcNow;
        await DbContext.SaveChangesAsync();

        // Act
        var command = new ResetProviderHealthCommand(providerId);
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updatedProvider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        updatedProvider.Should().NotBeNull();
        updatedProvider!.ConsecutiveFailures.Should().Be(0);
        updatedProvider.LastFailedFetch.Should().BeNull();
    }

    [Fact]
    public async Task ResetProviderHealth_WithHealthyProvider_ShouldSucceed()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act - Reset health on already healthy provider
        var command = new ResetProviderHealthCommand(providerId);
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.ConsecutiveFailures.Should().Be(0);
    }

    [Fact]
    public async Task ResetProviderHealth_WithNonExistentProvider_ShouldFail()
    {
        // Arrange
        var command = new ResetProviderHealthCommand(99999);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task ResetProviderHealth_MultipleProviders_ShouldResetAll()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var provider1Id = await CreateTestProviderAsync(currencyId);
        var provider2Id = await CreateTestProviderAsync(currencyId);

        // Simulate failures
        var providers = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Where(p => new[] { provider1Id, provider2Id }.Contains(p.Id))
            .ToListAsync();

        foreach (var provider in providers)
        {
            provider.ConsecutiveFailures = 3;
            provider.LastFailedFetch = DateTimeOffset.UtcNow;
        }
        await DbContext.SaveChangesAsync();

        // Act
        var result1 = await Mediator.Send(new ResetProviderHealthCommand(provider1Id));
        var result2 = await Mediator.Send(new ResetProviderHealthCommand(provider2Id));

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();

        var updatedProviders = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Where(p => new[] { provider1Id, provider2Id }.Contains(p.Id))
            .ToListAsync();

        updatedProviders.Should().OnlyContain(p => p.ConsecutiveFailures == 0);
        updatedProviders.Should().OnlyContain(p => p.LastFailedFetch == null);
    }
}
