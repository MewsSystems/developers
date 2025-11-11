using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.TriggerManualFetch;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.ExchangeRateProviders;

public class TriggerManualFetchCommandTests : IntegrationTestBase
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
    public async Task TriggerManualFetch_WithActiveProvider_ShouldEnqueueJob()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Act
        var command = new TriggerManualFetchCommand(providerId);
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().StartWith("fake-job-"); // FakeBackgroundJobService returns this format
    }

    [Fact]
    public async Task TriggerManualFetch_WithNonExistentProvider_ShouldFail()
    {
        // Arrange
        var command = new TriggerManualFetchCommand(99999);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task TriggerManualFetch_WithInactiveProvider_ShouldFail()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var providerId = await CreateTestProviderAsync(currencyId);

        // Deactivate the provider
        var deactivateCommand = new DeactivateProviderCommand(providerId);
        await Mediator.Send(deactivateCommand);

        // Act
        var command = new TriggerManualFetchCommand(providerId);
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not active");
    }

    [Fact]
    public async Task TriggerManualFetch_MultipleProviders_ShouldEnqueueAllJobs()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var provider1Id = await CreateTestProviderAsync(currencyId);
        var provider2Id = await CreateTestProviderAsync(currencyId);
        var provider3Id = await CreateTestProviderAsync(currencyId);

        // Act
        var result1 = await Mediator.Send(new TriggerManualFetchCommand(provider1Id));
        var result2 = await Mediator.Send(new TriggerManualFetchCommand(provider2Id));
        var result3 = await Mediator.Send(new TriggerManualFetchCommand(provider3Id));

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        result3.IsSuccess.Should().BeTrue();

        result1.Value.Should().NotBe(result2.Value);
        result2.Value.Should().NotBe(result3.Value);
        result1.Value.Should().NotBe(result3.Value);
    }
}
