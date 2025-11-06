using ApplicationLayer.Commands.Currencies.CreateCurrency;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.Currencies;

public class CreateCurrencyCommandTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateCurrency_WithValidCode_ShouldCreateCurrency()
    {
        // Arrange
        var command = new CreateCurrencyCommand("USD");

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        // Verify currency was created in database
        var currency = await DbContext.Set<DataLayer.Entities.Currency>()
            .FirstOrDefaultAsync(c => c.Code == "USD");

        currency.Should().NotBeNull();
        currency!.Code.Should().Be("USD");
    }

    [Fact]
    public async Task CreateCurrency_WithDuplicateCode_ShouldFail()
    {
        // Arrange - Create first currency
        var firstCommand = new CreateCurrencyCommand("EUR");
        await Mediator.Send(firstCommand);

        // Act - Try to create duplicate
        var secondCommand = new CreateCurrencyCommand("EUR");
        var result = await Mediator.Send(secondCommand);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already exists");
    }

    [Fact]
    public async Task CreateCurrency_WithInvalidCode_ShouldFail()
    {
        // Arrange
        var command = new CreateCurrencyCommand("INVALID");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().ContainKey("Code");
    }

    [Fact]
    public async Task CreateCurrency_WithEmptyCode_ShouldFail()
    {
        // Arrange
        var command = new CreateCurrencyCommand("");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().ContainKey("Code");
    }

    [Fact]
    public async Task CreateCurrency_WithLowercaseCode_ShouldFail()
    {
        // Arrange
        var command = new CreateCurrencyCommand("gbp");

        // Act & Assert - ISO 4217 codes must be uppercase
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().ContainKey("Code");
    }

    [Fact]
    public async Task CreateCurrency_WithMultipleCurrencies_ShouldCreateAll()
    {
        // Arrange
        var codes = new[] { "JPY", "CHF", "AUD", "CAD" };

        // Act
        var results = new List<int>();
        foreach (var code in codes)
        {
            var command = new CreateCurrencyCommand(code);
            var result = await Mediator.Send(command);
            result.IsSuccess.Should().BeTrue();
            results.Add(result.Value);
        }

        // Assert
        results.Should().HaveCount(4);
        results.Should().OnlyHaveUniqueItems();

        // Verify all currencies exist in database
        var currencies = await DbContext.Set<DataLayer.Entities.Currency>()
            .Where(c => codes.Contains(c.Code))
            .ToListAsync();

        currencies.Should().HaveCount(4);
    }
}
