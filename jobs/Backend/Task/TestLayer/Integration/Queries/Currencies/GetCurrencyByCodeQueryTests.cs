using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Queries.Currencies.GetCurrencyByCode;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Currencies;

public class GetCurrencyByCodeQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrency(string code)
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        return result.Value;
    }

    [Fact]
    public async Task GetCurrencyByCode_WithExistingCurrency_ShouldReturnCurrency()
    {
        // Arrange
        await CreateTestCurrency("USD");

        var query = new GetCurrencyByCodeQuery("USD");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Code.Should().Be("USD");
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetCurrencyByCode_WithNonExistentCode_ShouldReturnNull()
    {
        // Arrange
        var query = new GetCurrencyByCodeQuery("ZZZ");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCurrencyByCode_WithLowercaseCode_ShouldFail()
    {
        // Arrange
        await CreateTestCurrency("EUR");

        var query = new GetCurrencyByCodeQuery("eur");

        // Act & Assert - ISO 4217 codes must be uppercase
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("Code");
    }

    [Fact]
    public async Task GetCurrencyByCode_WithInvalidCode_ShouldFail()
    {
        // Arrange
        var query = new GetCurrencyByCodeQuery("");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("Code");
    }

    [Fact]
    public async Task GetCurrencyByCode_WithMultipleCurrencies_ShouldReturnCorrectOne()
    {
        // Arrange
        await CreateTestCurrency("GBP");
        await CreateTestCurrency("JPY");
        await CreateTestCurrency("CHF");

        var query = new GetCurrencyByCodeQuery("JPY");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Code.Should().Be("JPY");
    }
}
