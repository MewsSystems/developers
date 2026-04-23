using System.ComponentModel.DataAnnotations;
using ExchangeRateUpdater.Api.Presentation.Contracts.Requests;

namespace ExchangeRateUpdater.Api.Tests.Presentation.Validators;

public class GetExchangeRatesRequestValidatorTests
{
    private static IList<ValidationResult> ValidateModel(GetExchangeRatesRequest request)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(request, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void Validate_WithValidRequest_PassesValidation()
    {
        // Arrange
        var request = new GetExchangeRatesRequest
        {
            Currencies = "USD,EUR,GBP",
            Date = "2025-06-15"
        };

        // Act
        var results = ValidateModel(request);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_WithNullValues_PassesValidation()
    {
        // Arrange
        var request = new GetExchangeRatesRequest();

        // Act
        var results = ValidateModel(request);

        // Assert
        Assert.Empty(results);
    }

    [Theory]
    [InlineData("US")]
    [InlineData("USDA")]
    [InlineData("U$D")]
    [InlineData("123")]
    [InlineData("USD,E")]
    public void Validate_WithInvalidCurrencyCodes_FailsValidation(string currencies)
    {
        // Arrange
        var request = new GetExchangeRatesRequest { Currencies = currencies };

        // Act
        var results = ValidateModel(request);

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(GetExchangeRatesRequest.Currencies)));
    }

    [Theory]
    [InlineData("not-a-date")]
    [InlineData("2025-13-01")]
    [InlineData("2025-06-32")]
    [InlineData("25-06-15")]
    public void Validate_WithInvalidDate_FailsValidation(string date)
    {
        // Arrange
        var request = new GetExchangeRatesRequest { Date = date };

        // Act
        var results = ValidateModel(request);

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(GetExchangeRatesRequest.Date)));
    }
}
