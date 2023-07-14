using FluentValidation;

namespace ExchangeRateUpdater.Implementation.Queries
{
    public class GetExchangeRatesQueryValidator : AbstractValidator<GetExchangeRatesQuery>
    {
        public GetExchangeRatesQueryValidator()
        {
            RuleFor(x => x.Currencies).NotNull().NotEmpty();
        }
    }
}
