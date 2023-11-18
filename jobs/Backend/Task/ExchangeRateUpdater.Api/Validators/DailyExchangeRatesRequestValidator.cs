using ExchangeRateUpdater.Api.Models;
using FluentValidation;

namespace ExchangeRateUpdater.Api.Validators
{
    public class DailyExchangeRatesRequestValidator : AbstractValidator<DailyExchangeRatesRequest>
    {
        public DailyExchangeRatesRequestValidator()
        {
            RuleFor(r => r.Currencies)
                .NotNull()
                .NotEmpty();

            RuleForEach(r => r.Currencies)
                .NotNull()
                .NotEmpty()
                .Length(3);
        }
    }
}
