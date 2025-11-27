using ExchangeRateUpdater.Application.ExchangeRates.Query.GetExchangeRatesDaily;
using FluentValidation.TestHelper;

namespace UnitTests.Validation;

public class GetExchangesRatesByDateQueryValidatorTests
{
    private readonly GetExchangesRatesByDateQueryValidator _validator = new();

    [Fact]
    public void Validator_Fails_When_CurrencyCodes_NullOrEmpty()
    {
        var q1 = new GetExchangesRatesByDateQuery { CurrencyCodes = null! };
        var r1 = _validator.TestValidate(q1);
        r1.ShouldHaveValidationErrorFor(x => x.CurrencyCodes);

        var q2 = new GetExchangesRatesByDateQuery { CurrencyCodes = new List<string>() };
        var r2 = _validator.TestValidate(q2);
        r2.ShouldHaveValidationErrorFor(x => x.CurrencyCodes);
    }

    [Fact]
    public void Validator_Fails_When_Code_Length_Not_3()
    {
        var q = new GetExchangesRatesByDateQuery { CurrencyCodes = new List<string> { "US", "CZK" } };
        var r = _validator.TestValidate(q);
        r.ShouldHaveValidationErrorFor("CurrencyCodes[0]");
    }

    [Fact]
    public void Validator_Succeeds_For_Valid_Codes()
    {
        var q = new GetExchangesRatesByDateQuery { CurrencyCodes = new List<string> { "USD", "EUR" } };
        var r = _validator.TestValidate(q);
        r.ShouldNotHaveValidationErrorFor(x => x.CurrencyCodes);
    }
}
