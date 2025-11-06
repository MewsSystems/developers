using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.Currencies.DeleteCurrency;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.Currencies;

public class DeleteCurrencyCommandTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrency(string code)
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        return result.Value;
    }

    [Fact]
    public async Task DeleteCurrency_WithValidCurrency_ShouldDeleteCurrency()
    {
        // Arrange
        var currencyId = await CreateTestCurrency("NZD");

        var command = new DeleteCurrencyCommand(currencyId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify currency was deleted from database
        var currency = await DbContext.Set<DataLayer.Entities.Currency>()
            .FirstOrDefaultAsync(c => c.Id == currencyId);

        currency.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCurrency_WithNonExistentCurrency_ShouldFail()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(999);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task DeleteCurrency_WithInvalidId_ShouldFail()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(0);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().ContainKey("CurrencyId");
    }

    [Fact]
    public async Task DeleteCurrency_Multiple_ShouldDeleteAll()
    {
        // Arrange
        var currency1Id = await CreateTestCurrency("SEK");
        var currency2Id = await CreateTestCurrency("NOK");
        var currency3Id = await CreateTestCurrency("DKK");

        // Act - Delete all three
        var result1 = await Mediator.Send(new DeleteCurrencyCommand(currency1Id));
        var result2 = await Mediator.Send(new DeleteCurrencyCommand(currency2Id));
        var result3 = await Mediator.Send(new DeleteCurrencyCommand(currency3Id));

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        result3.IsSuccess.Should().BeTrue();

        // Verify all currencies were deleted
        var remaining = await DbContext.Set<DataLayer.Entities.Currency>()
            .Where(c => new[] { currency1Id, currency2Id, currency3Id }.Contains(c.Id))
            .ToListAsync();

        remaining.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteCurrency_TwiceWithSameId_ShouldFailSecondTime()
    {
        // Arrange
        var currencyId = await CreateTestCurrency("MXN");

        // Act - Delete once
        var firstResult = await Mediator.Send(new DeleteCurrencyCommand(currencyId));
        firstResult.IsSuccess.Should().BeTrue();

        // Act - Try to delete again
        var secondResult = await Mediator.Send(new DeleteCurrencyCommand(currencyId));

        // Assert
        secondResult.IsSuccess.Should().BeFalse();
        secondResult.Error.Should().Contain("not found");
    }
}
