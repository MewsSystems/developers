using FluentValidation;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries
{
    /// <summary>
    /// Validator for the <see cref="GetExchangeRatesQuery"/> class.
    /// </summary>
    public class GetExchangeRatesQueryValidator : AbstractValidator<GetExchangeRatesQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetExchangeRatesQueryValidator"/> class.
        /// </summary>
        public GetExchangeRatesQueryValidator()
        {
            RuleFor(x => x.CurrencyCodes)
                .Must(x => x?.Count < 10)
                .WithMessage("The CurrencyCodes must contain fewer than 10 items")
                .ForEach(code =>
                {
                    code.Must(x => x?.Length == 3)
                        .WithMessage("Code has to be three-letter ISO 4217 code of the currency.");
                });
        }
    }
}
