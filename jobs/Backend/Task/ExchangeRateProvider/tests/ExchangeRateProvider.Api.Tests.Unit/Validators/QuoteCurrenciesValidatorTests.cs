using ExchangeRateProvider.Api.Validators;
using FluentValidation.TestHelper;
using Shouldly;

namespace ExchangeRateProvider.Api.Tests.Unit.Validators;

public class QuoteCurrenciesValidatorTests
{
    private readonly QuoteCurrenciesValidator _validator;

    public QuoteCurrenciesValidatorTests()
    {
        _validator = new QuoteCurrenciesValidator();
    }

    [Fact]
    public async Task Validate_WithEmptyList_ReturnsValid()
    {
        // Arrange
        var quoteCurrencies = new List<string>();

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_WithValidThreeLetterCodes_ReturnsValid()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EUR", "GBP", "USD", "JPY" };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_WithLowercaseValidCodes_ReturnsValid()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "eur", "gbp", "usd" };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_WithMixedCaseValidCodes_ReturnsValid()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EuR", "GbP", "UsD" };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_WithEmptyStrings_ReturnsValid()
    {
        // Arrange - Empty/whitespace entries should be skipped, not cause validation errors
        var quoteCurrencies = new List<string> { "", "  ", "\t" };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_WithMixOfValidAndEmptyStrings_ReturnsValid()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EUR", "", "GBP", "  ", "USD" };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_WithTooShortCode_ReturnsInvalid()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EU" }; // Only 2 letters

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldContain("EU");
        result.Errors[0].ErrorMessage.ShouldContain("not a valid ISO 4217 code");
    }

    [Fact]
    public async Task Validate_WithTooLongCode_ReturnsInvalid()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EURO" }; // 4 letters

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldContain("EURO");
        result.Errors[0].ErrorMessage.ShouldContain("not a valid ISO 4217 code");
    }

    [Fact]
    public async Task Validate_WithNumericCharacters_ReturnsInvalid()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EU1", "2ND", "AB3" };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(3);
        result.Errors.ShouldAllBe(e => e.ErrorMessage.Contains("not a valid ISO 4217 code"));
    }

    [Fact]
    public async Task Validate_WithSpecialCharacters_ReturnsInvalid()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EU$", "GB-", "U_D" };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(3);
        result.Errors.ShouldAllBe(e => e.ErrorMessage.Contains("not a valid ISO 4217 code"));
    }

    [Fact]
    public async Task Validate_WithMultipleInvalidCodes_ReturnsAllErrors()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EU", "EURO", "12", "A$D" };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(4);
        result.Errors.ShouldContain(e => e.ErrorMessage.Contains("EU"));
        result.Errors.ShouldContain(e => e.ErrorMessage.Contains("EURO"));
        result.Errors.ShouldContain(e => e.ErrorMessage.Contains("12"));
        result.Errors.ShouldContain(e => e.ErrorMessage.Contains("A$D"));
    }

    [Fact]
    public async Task Validate_WithMixOfValidAndInvalidCodes_ReturnsOnlyInvalidErrors()
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EUR", "INVALID", "GBP", "XX", "USD" };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldContain(e => e.ErrorMessage.Contains("INVALID"));
        result.Errors.ShouldContain(e => e.ErrorMessage.Contains("XX"));

        // Valid codes should not appear in errors
        result.Errors.ShouldAllBe(e => !e.ErrorMessage.Contains("EUR"));
        result.Errors.ShouldAllBe(e => !e.ErrorMessage.Contains("GBP"));
        result.Errors.ShouldAllBe(e => !e.ErrorMessage.Contains("USD"));
    }

    [Theory]
    [InlineData("A")]
    [InlineData("AB")]
    [InlineData("ABCD")]
    [InlineData("ABCDE")]
    public async Task Validate_WithInvalidLength_ReturnsInvalid(string invalidCode)
    {
        // Arrange
        var quoteCurrencies = new List<string> { invalidCode };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldContain(invalidCode);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("A23")]
    [InlineData("AB3")]
    [InlineData("1BC")]
    public async Task Validate_WithNumbers_ReturnsInvalid(string invalidCode)
    {
        // Arrange
        var quoteCurrencies = new List<string> { invalidCode };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors[0].ErrorMessage.ShouldContain(invalidCode);
    }

    [Theory]
    [InlineData("A$D")]
    [InlineData("E@R")]
    [InlineData("GB#")]
    [InlineData("U-D")]
    [InlineData("E_R")]
    public async Task Validate_WithSpecialChars_ReturnsInvalid(string invalidCode)
    {
        // Arrange
        var quoteCurrencies = new List<string> { invalidCode };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors[0].ErrorMessage.ShouldContain(invalidCode);
    }

    [Theory]
    [InlineData("EUR")]
    [InlineData("GBP")]
    [InlineData("USD")]
    [InlineData("JPY")]
    [InlineData("CHF")]
    [InlineData("CAD")]
    [InlineData("AUD")]
    [InlineData("NZD")]
    public async Task Validate_WithValidIsoCodes_ReturnsValid(string validCode)
    {
        // Arrange
        var quoteCurrencies = new List<string> { validCode };

        // Act
        var result = await _validator.TestValidateAsync(quoteCurrencies);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }
}