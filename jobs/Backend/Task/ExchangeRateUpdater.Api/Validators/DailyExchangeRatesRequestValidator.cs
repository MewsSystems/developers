using ExchangeRateUpdater.Api.Models;
using FluentValidation;

namespace ExchangeRateUpdater.Api.Validators
{
    public class DailyExchangeRatesRequestValidator : AbstractValidator<DailyExchangeRatesRequest>
    {
        public DailyExchangeRatesRequestValidator()
        {
            RuleFor(r => r.CurrencyCodes)
                .NotNull()
                .NotEmpty();

            RuleForEach(r => r.CurrencyCodes)
                .NotNull()
                .NotEmpty()
                .Length(3);
        }
    }
}
