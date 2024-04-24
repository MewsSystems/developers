namespace ExchangeRateUpdater.Application.ExchangeRates.Query.GetExchangeRatesDaily;

using FluentValidation;

public class GetExchangesRatesByDateQueryValidator : AbstractValidator<GetExchangesRatesByDateQuery>
{
    public GetExchangesRatesByDateQueryValidator()
    {
        RuleFor(x => x.CurrencyCodes)
            .NotEmpty().NotNull()
            .ForEach(code =>
            {
                code.NotEmpty().NotNull().Must(x => x.Length == 3)
                    .WithMessage("Currency Code must to be three-letter ISO 4217 code of the currency.");
            });
    }
}