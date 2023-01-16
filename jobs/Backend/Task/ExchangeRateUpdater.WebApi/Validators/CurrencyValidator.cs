using FluentValidation;

namespace ExchangeRateUpdater.WebApi.Validators;

public class CurrencyValidator : AbstractValidator<Currency>
{
    public CurrencyValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("Empty currency codes are not allowed");
    }
}

