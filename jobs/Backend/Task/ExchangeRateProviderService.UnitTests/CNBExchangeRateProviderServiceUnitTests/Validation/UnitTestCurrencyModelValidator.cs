using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Validation;
using FluentValidation.TestHelper;

namespace ExchangeRateProviderService.UnitTests.CNBExchangeRateProviderServiceUnitTests.Validation;

[TestClass]
public class UnitTestCurrencyModelValidator
{
    private CurrencyModelValidator _currencyModelValidator = default!;

    private static readonly string CurrencyCodeWithLength2 = "XY";
    private static readonly string CurrencyCodeWithLength3 = "XYZ";
    private static readonly string CurrencyCodeWithLength4 = "XYZX";

    [TestInitialize]
    public void BeforeEach()
    {
        _currencyModelValidator = new CurrencyModelValidator();
    }

    [TestMethod]
    public void TestValidate_CurrencyCodeIsEmpty_ReturnsValidationError()
    {
        var currency = CreateCurrencyWithCode("");

        var validationResult = _currencyModelValidator.TestValidate(currency);

        validationResult.ShouldHaveValidationErrorFor(currency => currency.Code);
    }

    [TestMethod]
    public void TestValidate_CurrencyCodeIs2_ReturnsValidationError()
    {
        var currency = CreateCurrencyWithCode(CurrencyCodeWithLength2);

        var validationResult = _currencyModelValidator.TestValidate(currency);

        validationResult.ShouldHaveValidationErrorFor(currency => currency.Code);
    }

    [TestMethod]
    public void TestValidate_CurrencyCodeIs4_ReturnsValidationError()
    {
        var currency = CreateCurrencyWithCode(CurrencyCodeWithLength4);

        var validationResult = _currencyModelValidator.TestValidate(currency);

        validationResult.ShouldHaveValidationErrorFor(currency => currency.Code);
    }

    [TestMethod]
    public void TestValidate_CurrencyCodeIs3_ReturnsTrue()
    {
        var currency = CreateCurrencyWithCode(CurrencyCodeWithLength3);

        var validationResult = _currencyModelValidator.TestValidate(currency);

        validationResult.ShouldNotHaveValidationErrorFor(currency => currency.Code);
    }

    private static CurrencyDto CreateCurrencyWithCode(string currencyCode)
    {
        return new CurrencyDto
        {
            Code = currencyCode,
        };
    }
}
