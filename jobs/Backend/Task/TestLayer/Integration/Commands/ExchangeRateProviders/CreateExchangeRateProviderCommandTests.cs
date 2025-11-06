using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.ExchangeRateProviders;

public class CreateExchangeRateProviderCommandTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrencyAsync(string code = "USD")
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    [Fact]
    public async Task CreateExchangeRateProvider_WithValidData_ShouldCreateProvider()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("USD");
        var uniqueCode = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);

        var command = new CreateExchangeRateProviderCommand(
            Name: "Test Provider",
            Code: uniqueCode,
            Url: "https://api.example.com/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        // Verify provider was created in database
        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Code == uniqueCode);

        provider.Should().NotBeNull();
        provider!.Name.Should().Be("Test Provider");
        provider.Code.Should().Be(uniqueCode);
        provider.Url.Should().Be("https://api.example.com/rates");
        provider.BaseCurrencyId.Should().Be(currencyId);
        provider.RequiresAuthentication.Should().BeFalse();
        provider.ApiKeyVaultReference.Should().BeNull();
        provider.IsActive.Should().BeTrue();
        provider.ConsecutiveFailures.Should().Be(0);
    }

    [Fact]
    public async Task CreateExchangeRateProvider_WithAuthentication_ShouldCreateProviderWithApiKey()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("EUR");
        var uniqueCode = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);

        var command = new CreateExchangeRateProviderCommand(
            Name: "Authenticated Provider",
            Code: uniqueCode,
            Url: "https://api.secured.com/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: true,
            ApiKeyVaultReference: "vault-key-123"
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Code == uniqueCode);

        provider.Should().NotBeNull();
        provider!.RequiresAuthentication.Should().BeTrue();
        provider.ApiKeyVaultReference.Should().Be("vault-key-123");
    }

    [Fact]
    public async Task CreateExchangeRateProvider_WithDuplicateCode_ShouldFail()
    {
        // Arrange - Create first provider
        var currencyId = await CreateTestCurrencyAsync("GBP");
        var uniqueCode = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);

        var firstCommand = new CreateExchangeRateProviderCommand(
            Name: "First Provider",
            Code: uniqueCode,
            Url: "https://api.first.com/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );
        await Mediator.Send(firstCommand);

        // Act - Try to create duplicate
        var secondCommand = new CreateExchangeRateProviderCommand(
            Name: "Second Provider",
            Code: uniqueCode,
            Url: "https://api.second.com/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );
        var result = await Mediator.Send(secondCommand);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already exists");
    }

    [Fact]
    public async Task CreateExchangeRateProvider_WithInvalidCurrencyId_ShouldFail()
    {
        // Arrange
        var uniqueCode = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);

        var command = new CreateExchangeRateProviderCommand(
            Name: "Invalid Provider",
            Code: uniqueCode,
            Url: "https://api.example.com/rates",
            BaseCurrencyId: 99999, // Non-existent currency ID
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task CreateExchangeRateProvider_WithInvalidData_ShouldFail()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync("JPY");

        var command = new CreateExchangeRateProviderCommand(
            Name: "",  // Empty name
            Code: "",  // Empty code
            Url: "",   // Empty URL
            BaseCurrencyId: currencyId,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateExchangeRateProvider_WithMultipleDifferentProviders_ShouldCreateAll()
    {
        // Arrange
        var usdId = await CreateTestCurrencyAsync("USD");
        var eurId = await CreateTestCurrencyAsync("EUR");

        var providers = new[]
        {
            new CreateExchangeRateProviderCommand(
                Name: "Provider 1",
                Code: Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8),
                Url: "https://api.provider1.com/rates",
                BaseCurrencyId: usdId,
                RequiresAuthentication: false,
                ApiKeyVaultReference: null
            ),
            new CreateExchangeRateProviderCommand(
                Name: "Provider 2",
                Code: Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8),
                Url: "https://api.provider2.com/rates",
                BaseCurrencyId: eurId,
                RequiresAuthentication: true,
                ApiKeyVaultReference: "vault-key-456"
            ),
            new CreateExchangeRateProviderCommand(
                Name: "Provider 3",
                Code: Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8),
                Url: "https://api.provider3.com/rates",
                BaseCurrencyId: usdId,
                RequiresAuthentication: false,
                ApiKeyVaultReference: null
            )
        };

        // Act
        var results = new List<int>();
        foreach (var command in providers)
        {
            var result = await Mediator.Send(command);
            result.IsSuccess.Should().BeTrue();
            results.Add(result.Value);
        }

        // Assert
        results.Should().HaveCount(3);
        results.Should().OnlyHaveUniqueItems();

        // Verify all providers exist in database
        var savedProviders = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Where(p => providers.Select(cmd => cmd.Code).Contains(p.Code))
            .ToListAsync();

        savedProviders.Should().HaveCount(3);
    }
}
