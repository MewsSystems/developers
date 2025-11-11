using FluentValidation;

namespace ApplicationLayer.Queries.Providers.GetProviderHealth;

public class GetProviderHealthQueryValidator : AbstractValidator<GetProviderHealthQuery>
{
    public GetProviderHealthQueryValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be positive.");
    }
}
