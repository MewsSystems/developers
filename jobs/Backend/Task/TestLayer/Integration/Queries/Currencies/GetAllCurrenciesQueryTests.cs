using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Queries.Currencies.GetAllCurrencies;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Currencies;

public class GetAllCurrenciesQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrency(string code)
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        return result.Value;
    }

    [Fact]
    public async Task GetAllCurrencies_WithMultipleCurrencies_ShouldReturnPagedResults()
    {
        // Arrange - Create 5 test currencies
        await CreateTestCurrency("USD");
        await CreateTestCurrency("EUR");
        await CreateTestCurrency("GBP");
        await CreateTestCurrency("JPY");
        await CreateTestCurrency("CHF");

        var query = new GetAllCurrenciesQuery(PageNumber: 1, PageSize: 3);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(3, "at least 3 currencies should exist");
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(3);
    }

    [Fact]
    public async Task GetAllCurrencies_WithSearchTerm_ShouldReturnMatchingCurrencies()
    {
        // Arrange
        await CreateTestCurrency("USD");
        await CreateTestCurrency("EUR");
        await CreateTestCurrency("AUD");
        await CreateTestCurrency("CAD");

        var query = new GetAllCurrenciesQuery(
            PageNumber: 1,
            PageSize: 10,
            SearchTerm: "U"
        );

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(2, "at least USD and AUD should match 'U'");
        result.Items.Should().OnlyContain(c => c.Code.Contains("U", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetAllCurrencies_WithSecondPage_ShouldReturnCorrectItems()
    {
        // Arrange - Create 5 currencies
        await CreateTestCurrency("PLN");
        await CreateTestCurrency("CZK");
        await CreateTestCurrency("HUF");
        await CreateTestCurrency("RON");
        await CreateTestCurrency("BGN");

        var query = new GetAllCurrenciesQuery(PageNumber: 2, PageSize: 2);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(1, "second page should have at least 1 item");
        result.PageNumber.Should().Be(2);
    }

    [Fact]
    public async Task GetAllCurrencies_WithInvalidPageSize_ShouldFail()
    {
        // Arrange
        var query = new GetAllCurrenciesQuery(PageNumber: 1, PageSize: 150); // Max is 100

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("PageSize");
    }

    [Fact]
    public async Task GetAllCurrencies_WithInvalidPageNumber_ShouldFail()
    {
        // Arrange
        var query = new GetAllCurrenciesQuery(PageNumber: 0, PageSize: 10); // Must be >= 1

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("PageNumber");
    }

    [Fact]
    public async Task GetAllCurrencies_WithLargePageSize_ShouldReturnAllCurrencies()
    {
        // Arrange
        await CreateTestCurrency("THB");
        await CreateTestCurrency("SGD");
        await CreateTestCurrency("MYR");

        var query = new GetAllCurrenciesQuery(
            PageNumber: 1,
            PageSize: 100
        );

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(3, "at least 3 currencies should exist");
    }

    [Fact]
    public async Task GetAllCurrencies_SearchWithNoMatches_ShouldReturnEmptyResult()
    {
        // Arrange
        await CreateTestCurrency("BRL");
        await CreateTestCurrency("ARS");

        var query = new GetAllCurrenciesQuery(
            PageNumber: 1,
            PageSize: 10,
            SearchTerm: "ZZZ" // Non-existent currency code
        );

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
    }
}
