using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Queries;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace ExchangeRateUpdater.Tests.Unit.Validators;

public class GetExchangeRateQueryValidatorTests
{
    private readonly GetExchangeRateQueryValidator _validator;

    public GetExchangeRateQueryValidatorTests()
    {
        _validator = new GetExchangeRateQueryValidator();
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
    public void BeIso8601DateOrNull_ValidatesCorrectly(string? dateString, bool expectedResult)
    {
        // Act
        var result = GetExchangeRateQueryValidator.BeIso8601DateOrNull(dateString);
        
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
    public void BeValidIso4217CurrencyCode_ValidatesCorrectly(string? codeString, bool expectedResult)
    {
        // Act
        var result = GetExchangeRateQueryValidator.BeValidIso4217CurrencyCode(codeString);

        // Assert
        result.Should().Be(expectedResult);
    }
    
    [Fact]
    public void Validate_ValidQuery_PassesValidation()
    {
        // Arrange
        var query = new GetExchangeRateQuery("USD", "CZK", "2023-05-16");

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_NullDate_StillPassesValidation()
    {
        // Arrange
        var query = new GetExchangeRateQuery("USD", "CZK", null);

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_InvalidDate_FailsDateValidation()
    {
        // Arrange
        var query = new GetExchangeRateQuery("USD", "CZK", "16.05.2023"); // Wrong format

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }
    
    [Fact]
    public void Validate_InvalidSourceCurrency_FailsSourceCurrencyValidation()
    {
        // Arrange
        var query = new GetExchangeRateQuery("usd", "CZK", "2023-05-16"); // Not uppercase

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.SourceCurrency);
    }
    
    [Fact]
    public void Validate_InvalidTargetCurrency_FailsTargetCurrencyValidation()
    {
        // Arrange
        var query = new GetExchangeRateQuery("USD", "czk", "2023-05-16"); // Not uppercase

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.TargetCurrency);
    }
    
    [Fact]
    public void Validate_EmptySourceCurrency_FailsRequiredValidation()
    {
        // Arrange
        var query = new GetExchangeRateQuery("", "CZK", "2023-05-16");

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.SourceCurrency);
    }
    
    [Fact]
    public void Validate_EmptyTargetCurrency_FailsRequiredValidation()
    {
        // Arrange
        var query = new GetExchangeRateQuery("USD", "", "2023-05-16");

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.TargetCurrency);
    }
} 