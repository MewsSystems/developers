using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Validation;
using FluentValidation;

namespace ExchangeRateProviderService.UnitTests.CNBExchangeRateProviderServiceUnitTests.Validation;

[TestClass]
public class UnitTestExchangeRateModelValidator
{
    private ExchangeRateModelValidator _exchangeRateModelValidator = default!;
    private InlineValidator<CurrencyDto> _currencyModelValidator = default!;

    private readonly string FakeCurrencyCode = "XYZ";

    [TestInitialize]
    public void BeforeEach()
    {
        _currencyModelValidator = new InlineValidator<CurrencyDto>();
        _exchangeRateModelValidator = new ExchangeRateModelValidator(_currencyModelValidator);
    }

    [TestMethod]
    public void TestValidate_BaseCurrencyAndTargetCurrencyDoNotPassValidation_ReturnsValidationError()
    {
        var baseCurrency = CreateCurrencyWithCode(string.Empty);
        var targetCurrency = CreateCurrencyWithCode(string.Empty);
        var exchangeRate = CreateExchangeRate(baseCurrency, targetCurrency);

        MockValidatorFailureForEmptyCurrencyCode();

        var validationResult = _exchangeRateModelValidator.Validate(exchangeRate);
        var errors = validationResult.Errors;

        Assert.IsFalse(validationResult.IsValid);
        Assert.AreEqual(2, errors.Count);
    }

    [TestMethod]
    public void TestValidate_BaseCurrencyDoNotPassValidation_ReturnsValidationError()
    {
        var baseCurrency = CreateCurrencyWithCode(string.Empty);
        var targetCurrency = CreateCurrencyWithCode(FakeCurrencyCode);
        var exchangeRate = CreateExchangeRate(baseCurrency, targetCurrency);

        MockValidatorFailureForEmptyCurrencyCode();

        var validationResult = _exchangeRateModelValidator.Validate(exchangeRate);
        var errors = validationResult.Errors;

        Assert.IsFalse(validationResult.IsValid);
        Assert.AreEqual(1, errors.Count);
    }

    [TestMethod]
    public void TestValidate_TargetCurrencyDoNotPassValidation_ReturnsValidationError()
    {
        var baseCurrency = CreateCurrencyWithCode(FakeCurrencyCode);
        var targetCurrency = CreateCurrencyWithCode(string.Empty);
        var exchangeRate = CreateExchangeRate(baseCurrency, targetCurrency);

        MockValidatorFailureForEmptyCurrencyCode();

        var validationResult = _exchangeRateModelValidator.Validate(exchangeRate);
        var errors = validationResult.Errors;

        Assert.IsFalse(validationResult.IsValid);
        Assert.AreEqual(1, errors.Count);
    }

    [TestMethod]
    public void TestValidate_BaseCurrencyAndTargetCurrencyPassValidation_ReturnsTrue()
    {
        var baseCurrency = CreateCurrencyWithCode(FakeCurrencyCode);
        var targetCurrency = CreateCurrencyWithCode(FakeCurrencyCode);
        var exchangeRate = CreateExchangeRate(baseCurrency, targetCurrency);

        var validationResult = _exchangeRateModelValidator.Validate(exchangeRate);
        var errors = validationResult.Errors;

        Assert.IsTrue(validationResult.IsValid);
        Assert.AreEqual(0, errors.Count);
    }

    private void MockValidatorFailureForEmptyCurrencyCode()
    {
        _currencyModelValidator
            .RuleFor(currency => currency.Code).NotEmpty();
    }

    private static CurrencyDto CreateCurrencyWithCode(string currencyCode)
    {
        return new CurrencyDto
        {
            Code = currencyCode,
        };
    }

    private static ExchangeRateDto CreateExchangeRate(
        CurrencyDto baseCurrency, CurrencyDto targetCurrency)
    {
        return new ExchangeRateDto
        {
            BaseCurrency = baseCurrency,
            TargetCurrency = targetCurrency
        };
    }
}
