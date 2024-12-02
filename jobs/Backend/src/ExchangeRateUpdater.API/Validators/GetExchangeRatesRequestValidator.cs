using ExchangeRateUpdater.API.Models.RequestModels;
using ExchangeRateUpdater.Types;
using FluentValidation;

namespace ExchangeRateUpdater.API.Validators
{
    public class GetExchangeRatesRequestValidator : AbstractValidator<GetExchangeRatesRequest>
    {
        public GetExchangeRatesRequestValidator()
        {
            RuleForEach(x => x.Currencies).ChildRules(order =>
            {
                order.RuleFor(x => x).Must(MustBeAValidCurrency)
                .WithMessage("Invalid currency supplied");
            });
        }

        private bool MustBeAValidCurrency(string code)
        {
            bool isValid = CurrencyDictionary.ContainsCode(code);
            return isValid;
        }
    }
}
