using ExchangeRates.Api.DTOs;
using FluentValidation;

namespace ExchangeRates.Api.Validators
{
    public class GetExchangeRatesRequestValidator : AbstractValidator<GetExchangeRatesRequest>
    {
        public GetExchangeRatesRequestValidator()
        {
            RuleForEach(x => x.Currencies)
                .NotEmpty().WithMessage("Currency code cannot be empty.")
                .Matches("^[A-Za-z]{3}$")
                .WithMessage("Currency code must be exactly 3 alphabetic characters (A–Z).");
        }
    }
}