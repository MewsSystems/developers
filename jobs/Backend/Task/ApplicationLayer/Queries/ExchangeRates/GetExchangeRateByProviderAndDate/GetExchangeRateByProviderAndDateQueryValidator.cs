using FluentValidation;

namespace ApplicationLayer.Queries.ExchangeRates.GetExchangeRateByProviderAndDate;

public class GetExchangeRateByProviderAndDateQueryValidator : AbstractValidator<GetExchangeRateByProviderAndDateQuery>
{
    public GetExchangeRateByProviderAndDateQueryValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be positive.");

        RuleFor(x => x.ValidDate)
            .NotEmpty()
            .WithMessage("Valid date is required.");
    }
}
