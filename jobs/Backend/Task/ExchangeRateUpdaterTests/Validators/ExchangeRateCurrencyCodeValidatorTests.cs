using ExchangeRateUpdater.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace ExchangeRateUpdaterTests.Validators;

public class ExchangeRateCurrencyCodeValidatorTests
{
    private readonly ExchangeRateCurrencyCodeValidator _sut;

    public ExchangeRateCurrencyCodeValidatorTests()
    {
        _sut = new ExchangeRateCurrencyCodeValidator();
    }

    [Fact]
    public void Given_EmptyArray_When_Validating_Then_HasError()
    {
        var result = _sut.TestValidate([]);
        result.ShouldHaveValidationErrorFor("Currency Codes");
    }

    [Theory]
    [InlineData("US")]
    [InlineData("USDE")]
    public void Given_InvalidLengthCurrencyCode_When_Validating_Then_HasError(string code)
    {
        var result = _sut.TestValidate(new[] { code });
        result.ShouldHaveValidationErrorFor("Currency Code[0]");
    }

    [Fact]
    public void Given_ValidCurrencyCodes_When_Validating_Then_HasNoErrors()
    {
        var result = _sut.TestValidate(new[] { "USD", "EUR", "CZK" });
        result.ShouldNotHaveAnyValidationErrors();
    }
}
