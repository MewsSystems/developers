using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeleteProvider;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.ExchangeRateProviders;

public class DeleteProviderCommandTests : IntegrationTestBase
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
    public async Task DeleteProvider_WithoutExchangeRates_ShouldDeleteProvider()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var command = new DeleteProviderCommand(providerId, Force: false);
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProvider_WithNonExistentProvider_ShouldFail()
    {
        // Arrange
        var command = new DeleteProviderCommand(99999, Force: false);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task DeleteProvider_MultipleProviders_ShouldDeleteAll()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var provider1Id = await CreateTestProviderAsync(currencyId);
        var provider2Id = await CreateTestProviderAsync(currencyId);
        var provider3Id = await CreateTestProviderAsync(currencyId);

        // Act
        var result1 = await Mediator.Send(new DeleteProviderCommand(provider1Id, Force: false));
        var result2 = await Mediator.Send(new DeleteProviderCommand(provider2Id, Force: false));
        var result3 = await Mediator.Send(new DeleteProviderCommand(provider3Id, Force: false));

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        result3.IsSuccess.Should().BeTrue();

        var providers = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Where(p => new[] { provider1Id, provider2Id, provider3Id }.Contains(p.Id))
            .ToListAsync();

        providers.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteProvider_AfterCreation_ShouldBeDeletedFromDatabase()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var uniqueCode = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);
        var providerId = await CreateTestProviderAsync(currencyId, uniqueCode);

        // Verify provider exists
        var providerBeforeDelete = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Code == uniqueCode);
        providerBeforeDelete.Should().NotBeNull();

        // Act
        var command = new DeleteProviderCommand(providerId, Force: false);
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify provider no longer exists
        var providerAfterDelete = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Code == uniqueCode);
        providerAfterDelete.Should().BeNull();
    }
}
