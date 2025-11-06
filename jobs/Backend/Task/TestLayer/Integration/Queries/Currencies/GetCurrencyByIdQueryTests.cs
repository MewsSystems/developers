using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Queries.Currencies.GetCurrencyById;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Currencies;

public class GetCurrencyByIdQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrency(string code)
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        return result.Value;
    }

    [Fact]
    public async Task GetCurrencyById_WithExistingCurrency_ShouldReturnCurrency()
    {
        // Arrange
        var currencyId = await CreateTestCurrency("USD");

        var query = new GetCurrencyByIdQuery(currencyId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(currencyId);
        result.Code.Should().Be("USD");
    }

    [Fact]
    public async Task GetCurrencyById_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var query = new GetCurrencyByIdQuery(999);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCurrencyById_WithInvalidId_ShouldFail()
    {
        // Arrange
        var query = new GetCurrencyByIdQuery(0);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("CurrencyId");
    }

    [Fact]
    public async Task GetCurrencyById_WithMultipleCurrencies_ShouldReturnCorrectOne()
    {
        // Arrange
        var currency1Id = await CreateTestCurrency("EUR");
        var currency2Id = await CreateTestCurrency("GBP");
        var currency3Id = await CreateTestCurrency("JPY");

        var query = new GetCurrencyByIdQuery(currency2Id);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(currency2Id);
        result.Code.Should().Be("GBP");
    }

    [Fact]
    public async Task GetCurrencyById_AfterCreation_ShouldReturnSameCurrency()
    {
        // Arrange
        var currencyId = await CreateTestCurrency("CHF");

        // Act - Query immediately after creation
        var query = new GetCurrencyByIdQuery(currencyId);
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(currencyId);
        result.Code.Should().Be("CHF");
    }
}
