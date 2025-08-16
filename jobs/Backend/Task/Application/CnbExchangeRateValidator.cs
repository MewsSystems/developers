using FluentValidation;

namespace ExchangeRateUpdater.Application
{
    public class CnbExchangeRateValidator : AbstractValidator<CnbExchangeRate>
    {
        public CnbExchangeRateValidator()
        {
            RuleFor(rate => rate.Amount).GreaterThan(0);
            RuleFor(rate => rate.Rate).GreaterThan(0m);
        }
    }
}
