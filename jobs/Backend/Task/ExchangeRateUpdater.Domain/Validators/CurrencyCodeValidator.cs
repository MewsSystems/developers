namespace ExchangeRateUpdater.Domain.Validators;

public class CurrencyCodeValidator : AbstractValidator<Currency>
{
    public CurrencyCodeValidator()
    {
        RuleFor(x => x.Code)
        .NotEmpty()
        .Length(3)
        .Matches("^[A-Z]{3}$").WithMessage("Currency ISO code must consist of 3 uppercase letters");
    }
}
