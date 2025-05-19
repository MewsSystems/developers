using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace ExchangeRateUpdater.Tests.Unit.Validators;

public class BatchRateRequestValidatorTests
{
    private readonly BatchRateRequestValidator _validator;

    public BatchRateRequestValidatorTests()
    {
        _validator = new BatchRateRequestValidator();
    }

    [Theory]
    [InlineData("2023-05-16", true)]
    [InlineData("2023-05-16T00:00:00", false)] // Not a valid ISO-8601 date (includes time)
    [InlineData("16-05-2023", false)] // Not in ISO format (dd-MM-yyyy)
    [InlineData("16.05.2023", false)] // Not in ISO format (dd.MM.yyyy)
    [InlineData("2023/05/16", false)] // Not in ISO format (yyyy/MM/dd)
    [InlineData("2023-13-16", false)] // Invalid month
    [InlineData("2023-05-32", false)] // Invalid day
    [InlineData("", true)] // Empty string should be valid (nullable)
    [InlineData(null, true)] // Null should be valid
    public void BeIso8601Date_ValidatesCorrectly(string? dateString, bool expectedResult)
    {
        // Act
        var result = BatchRateRequestValidator.BeIso8601Date(dateString);
            
        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("USD/CZK", true)]
    [InlineData("EUR/USD", true)]
    [InlineData("usd/czk", false)] // Not uppercase
    [InlineData("USD-CZK", false)] // Wrong separator
    [InlineData("USDC/ZK", false)] // Wrong format (first code)
    [InlineData("USD/CZKK", false)] // Wrong format (second code)
    [InlineData("USD", false)] // Missing second code
    [InlineData("USD/", false)] // Missing second code
    [InlineData("/CZK", false)] // Missing first code
    [InlineData("", false)] // Empty string
    [InlineData("USD/CZK/EUR", false)] // Too many codes
    [InlineData("US1/CZK", false)] // Contains digit
    [InlineData("USD/CZ#", false)] // Contains special character
    public void BeIso4217Pair_ValidatesCorrectly(string pairString, bool expectedResult)
    {
        // Act
        var result = BatchRateRequestValidator.BeIso4217Pair(pairString);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("USD", true)]
    [InlineData("EUR", true)]
    [InlineData("CZK", true)]
    [InlineData("usd", false)] // Not uppercase
    [InlineData("US", false)] // Too short
    [InlineData("USDD", false)] // Too long
    [InlineData("US1", false)] // Contains digit
    [InlineData("US#", false)] // Contains special character
    [InlineData("", false)] // Empty
    [InlineData(null, false)] // Null
    public void IsIso4217CurrencyCode_ValidatesCorrectly(string? codeString, bool expectedResult)
    {
        // Act
        var result = BatchRateRequestValidator.IsIso4217CurrencyCode(codeString);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void Validate_ValidRequest_PassesValidation()
    {
        // Arrange
        var request = new BatchRateRequest
        {
            Date = "2023-05-16",
            CurrencyPairs = new[] { "USD/CZK", "EUR/CZK" }
        };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_InvalidDate_FailsDateValidation()
    {
        // Arrange
        var request = new BatchRateRequest
        {
            Date = "16.05.2023", // Wrong format
            CurrencyPairs = new[] { "USD/CZK", "EUR/CZK" }
        };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void Validate_InvalidCurrencyPair_FailsCurrencyPairValidation()
    {
        // Arrange
        var request = new BatchRateRequest
        {
            Date = "2023-05-16",
            CurrencyPairs = new[] { "USD/CZK", "eur/czk" } // One pair is not uppercase
        };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.CurrencyPairs);
    }

    [Fact]
    public void Validate_EmptyCurrencyPairs_FailsNotEmptyValidation()
    {
        // Arrange
        var request = new BatchRateRequest
        {
            Date = "2023-05-16",
            CurrencyPairs = Array.Empty<string>()
        };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.CurrencyPairs);
    }
} 