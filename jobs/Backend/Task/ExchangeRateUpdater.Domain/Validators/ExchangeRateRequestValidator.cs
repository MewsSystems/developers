namespace ExchangeRateUpdater.Domain.Validators;

public class ExchangeRateRequestValidator : AbstractValidator<ExchangeRateRequest>
{
    public ExchangeRateRequestValidator()
    {
        RuleFor(x => x.Currencies)
        .NotEmpty()
        .ForEach(x => x.SetValidator(new CurrencyCodeValidator()));
    }
}
