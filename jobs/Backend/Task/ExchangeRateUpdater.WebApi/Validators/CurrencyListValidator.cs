using FluentValidation;

namespace ExchangeRateUpdater.WebApi.Validators;

public class CurrencyListValidator : AbstractValidator<IEnumerable<Currency>>
{
    public CurrencyListValidator()
    {
        RuleForEach(x => x).SetValidator(new CurrencyValidator());
    }
}