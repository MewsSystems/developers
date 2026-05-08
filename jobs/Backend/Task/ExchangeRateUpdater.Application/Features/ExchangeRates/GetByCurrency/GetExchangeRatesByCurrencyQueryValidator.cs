using FluentValidation;

namespace ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;

internal class GetExchangeRatesByCurrencyQueryValidator : AbstractValidator<GetExchangeRatesByCurrencyQuery>
{
    public GetExchangeRatesByCurrencyQueryValidator()
    {
        RuleFor(x => x.CurrencyCodes)
            .NotEmpty();
        RuleForEach(x => x.CurrencyCodes)
            .NotEmpty()
            .Length(3)
            .WithMessage("Must be valid ISO code e.g EUR");
        RuleFor(x => x.ForDate)
            .LessThan(DateTime.UtcNow.AddDays(1)).WithMessage("Cannot be future date")
            .When(x => x.ForDate.HasValue);
    }
}
