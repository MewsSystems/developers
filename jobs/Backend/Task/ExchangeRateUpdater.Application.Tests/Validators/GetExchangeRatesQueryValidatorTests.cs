using ExchangeRateUpdater.Application.Queries;
using ExchangeRateUpdater.Application.Validators;

namespace ExchangeRateUpdater.Application.Tests.Validators;

public class GetExchangeRatesQueryValidatorTests
{
    private readonly GetExchangeRatesQueryValidator _validator;

    public GetExchangeRatesQueryValidatorTests()
    {
        _validator = new GetExchangeRatesQueryValidator();
    }

    [Fact]
    public void Validate_ValidDateAndCurrencies_ShouldPass()
    {
        var query = new GetExchangeRatesQuery
        {
            Date = DateTime.Today.AddDays(-1), // Valid past date
            Currencies = new List<string> { "USD", "EUR" }
        };

        var result = _validator.Validate(query);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_FutureDate_ShouldFail()
    {
        var query = new GetExchangeRatesQuery
        {
            Date = DateTime.Today.AddDays(1), // Future date
            Currencies = new List<string> { "USD" }
        };

        var result = _validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("The date cannot be in the future."));
    }

    [Fact]
    public void Validate_DateBefore1991_ShouldFail()
    {
        var query = new GetExchangeRatesQuery
        {
            Date = new DateTime(1990, 12, 31), // Before 1991
            Currencies = new List<string> { "USD" }
        };

        var result = _validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("The date must be after January 1, 1991"));
    }

    [Fact]
    public void Validate_TooManyCurrencies_ShouldFail()
    {
        var query = new GetExchangeRatesQuery
        {
            Date = DateTime.Today,
            Currencies = ["USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP"]
        };

        var result = _validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("You can request a maximum of 20 currencies"));
    }

    [Theory]
    [InlineData("usd")] // Lowercase
    [InlineData("US")]
    [InlineData("USDE")] // Too long
    [InlineData("1US")] // Invalid format
    public void Validate_InvalidCurrencyFormat_ShouldFail(string invalidCurrency)
    {
        var query = new GetExchangeRatesQuery
        {
            Date = DateTime.Today,
            Currencies = new List<string> { invalidCurrency }
        };

        var result = _validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("All currencies must be 3-letter uppercase ISO 4217 codes"));
    }
}
