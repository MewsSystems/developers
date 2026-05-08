using ExchangeRateUpdater.Domain.Exceptions;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentValidation;

namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates
{
    public class GetExchangeRatesQueryValidator : AbstractValidator<GetExchangeRatesQuery>
    {
        public GetExchangeRatesQueryValidator()
        {
            RuleFor(x => x.Currencies)
                .NotEmpty();

            RuleForEach(x => x.Currencies)
                .SetValidator(new CurrencyCodeValidator());

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.Today)
                .When(x => x.Date.HasValue)
                .WithMessage("Invalid date. Future dates not allowed");
        }
    }

    public class CurrencyCodeValidator : AbstractValidator<string>
    {
        public CurrencyCodeValidator()
        {
            RuleFor(x => x)
                .Must(BeAValidCurrency)
                .WithMessage("Invalid currency ISO code");
        }

        private bool BeAValidCurrency(string currencyCode)
        {
            try
            {
                _ = new Currency(currencyCode);
                return true;
            }
            catch (DomainException)
            {
                return false;
            }
        }
    }
}
