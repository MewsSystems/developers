using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.RescheduleProvider;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.ExchangeRateProviders;

/// <summary>
/// Integration tests for RescheduleProviderCommand.
/// Tests end-to-end provider rescheduling with database persistence.
/// </summary>
public class RescheduleProviderCommandTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrencyAsync(string code = "USD")
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    private async Task<(int ProviderId, string ProviderCode)> CreateTestProviderAsync(
        int currencyId,
        string? code = null)
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
        return (result.Value, code);
    }

    [Fact]
    public async Task RescheduleProvider_WithValidTimeAndTimezone_ShouldUpdateConfiguration()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var (providerId, providerCode) = await CreateTestProviderAsync(currencyId);

        var command = new RescheduleProviderCommand(
            ProviderCode: providerCode,
            UpdateTime: "14:30",
            TimeZone: "CET");

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Include(p => p.Configurations)
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.Configurations.Should().Contain(c => c.SettingKey == "UpdateTime" && c.SettingValue == "14:30");
        provider.Configurations.Should().Contain(c => c.SettingKey == "TimeZone" && c.SettingValue == "CET");
    }

    [Fact]
    public async Task RescheduleProvider_WithNonExistentProvider_ShouldFail()
    {
        // Arrange
        var command = new RescheduleProviderCommand(
            ProviderCode: "NOEXIST",
            UpdateTime: "14:30",
            TimeZone: "CET");

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Theory]
    [InlineData("00:00", "UTC")]
    [InlineData("06:15", "CET")]
    [InlineData("12:30", "CEST")]
    [InlineData("18:45", "EET")]
    [InlineData("23:59", "Europe/Prague")]
    public async Task RescheduleProvider_WithVariousTimeZones_ShouldUpdateCorrectly(
        string updateTime,
        string timeZone)
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var (providerId, providerCode) = await CreateTestProviderAsync(currencyId);

        var command = new RescheduleProviderCommand(
            ProviderCode: providerCode,
            UpdateTime: updateTime,
            TimeZone: timeZone);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Include(p => p.Configurations)
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.Configurations.Should().Contain(c => c.SettingKey == "UpdateTime" && c.SettingValue == updateTime);
        provider.Configurations.Should().Contain(c => c.SettingKey == "TimeZone" && c.SettingValue == timeZone);
    }

    [Fact]
    public async Task RescheduleProvider_UpdateExistingConfiguration_ShouldOverwritePrevious()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var (providerId, providerCode) = await CreateTestProviderAsync(currencyId);

        // First reschedule
        var firstCommand = new RescheduleProviderCommand(
            ProviderCode: providerCode,
            UpdateTime: "10:00",
            TimeZone: "UTC");

        await Mediator.Send(firstCommand);

        // Second reschedule with different values
        var secondCommand = new RescheduleProviderCommand(
            ProviderCode: providerCode,
            UpdateTime: "18:00",
            TimeZone: "CEST");

        // Act
        var result = await Mediator.Send(secondCommand);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Include(p => p.Configurations)
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider.Should().NotBeNull();
        provider!.Configurations.Should().Contain(c => c.SettingKey == "UpdateTime" && c.SettingValue == "18:00");
        provider.Configurations.Should().Contain(c => c.SettingKey == "TimeZone" && c.SettingValue == "CEST");
    }

    [Fact]
    public async Task RescheduleProvider_MultipleProviders_ShouldUpdateIndependently()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var (provider1Id, provider1Code) = await CreateTestProviderAsync(currencyId);
        var (provider2Id, provider2Code) = await CreateTestProviderAsync(currencyId);
        var (provider3Id, provider3Code) = await CreateTestProviderAsync(currencyId);

        var command1 = new RescheduleProviderCommand(provider1Code, "06:00", "UTC");
        var command2 = new RescheduleProviderCommand(provider2Code, "12:00", "CET");
        var command3 = new RescheduleProviderCommand(provider3Code, "18:00", "CEST");

        // Act
        var result1 = await Mediator.Send(command1);
        var result2 = await Mediator.Send(command2);
        var result3 = await Mediator.Send(command3);

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        result3.IsSuccess.Should().BeTrue();

        var providers = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Include(p => p.Configurations)
            .Where(p => new[] { provider1Id, provider2Id, provider3Id }.Contains(p.Id))
            .ToListAsync();

        providers.Should().HaveCount(3);

        var p1 = providers.First(p => p.Id == provider1Id);
        p1.Configurations.Should().Contain(c => c.SettingKey == "UpdateTime" && c.SettingValue == "06:00");
        p1.Configurations.Should().Contain(c => c.SettingKey == "TimeZone" && c.SettingValue == "UTC");

        var p2 = providers.First(p => p.Id == provider2Id);
        p2.Configurations.Should().Contain(c => c.SettingKey == "UpdateTime" && c.SettingValue == "12:00");
        p2.Configurations.Should().Contain(c => c.SettingKey == "TimeZone" && c.SettingValue == "CET");

        var p3 = providers.First(p => p.Id == provider3Id);
        p3.Configurations.Should().Contain(c => c.SettingKey == "UpdateTime" && c.SettingValue == "18:00");
        p3.Configurations.Should().Contain(c => c.SettingKey == "TimeZone" && c.SettingValue == "CEST");
    }

    [Fact]
    public async Task RescheduleProvider_WithEmptyUpdateTime_ShouldFailValidation()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var (_, providerCode) = await CreateTestProviderAsync(currencyId);

        var command = new RescheduleProviderCommand(
            ProviderCode: providerCode,
            UpdateTime: "",
            TimeZone: "CET");

        // Act & Assert - Should fail FluentValidation before reaching handler
        await FluentActions.Invoking(async () => await Mediator.Send(command))
            .Should().ThrowAsync<ApplicationLayer.Common.Exceptions.ValidationException>();
    }

    [Fact]
    public async Task RescheduleProvider_WithInvalidTimeFormat_ShouldFailValidation()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var (_, providerCode) = await CreateTestProviderAsync(currencyId);

        var command = new RescheduleProviderCommand(
            ProviderCode: providerCode,
            UpdateTime: "25:99", // Invalid time
            TimeZone: "CET");

        // Act & Assert
        await FluentActions.Invoking(async () => await Mediator.Send(command))
            .Should().ThrowAsync<ApplicationLayer.Common.Exceptions.ValidationException>();
    }

    [Fact]
    public async Task RescheduleProvider_WithEdgeTimesAndZones_ShouldSucceed()
    {
        // Arrange
        var currencyId = await CreateTestCurrencyAsync();
        var (providerId, providerCode) = await CreateTestProviderAsync(currencyId);

        // Test midnight
        var midnightCommand = new RescheduleProviderCommand(
            ProviderCode: providerCode,
            UpdateTime: "00:00",
            TimeZone: "UTC");

        // Act
        var result1 = await Mediator.Send(midnightCommand);

        // Assert
        result1.IsSuccess.Should().BeTrue();

        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .Include(p => p.Configurations)
            .FirstOrDefaultAsync(p => p.Id == providerId);

        provider!.Configurations.Should().Contain(c => c.SettingKey == "UpdateTime" && c.SettingValue == "00:00");

        // Test end of day
        var endOfDayCommand = new RescheduleProviderCommand(
            ProviderCode: providerCode,
            UpdateTime: "23:59",
            TimeZone: "GMT");

        var result2 = await Mediator.Send(endOfDayCommand);

        result2.IsSuccess.Should().BeTrue();

        await DbContext.Entry(provider).ReloadAsync();
        provider.Configurations.Should().Contain(c => c.SettingKey == "UpdateTime" && c.SettingValue == "23:59");
        provider.Configurations.Should().Contain(c => c.SettingKey == "TimeZone" && c.SettingValue == "GMT");
    }
}
