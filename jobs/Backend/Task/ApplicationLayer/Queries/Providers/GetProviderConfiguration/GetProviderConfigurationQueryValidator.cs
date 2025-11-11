using FluentValidation;

namespace ApplicationLayer.Queries.Providers.GetProviderConfiguration;

public class GetProviderConfigurationQueryValidator : AbstractValidator<GetProviderConfigurationQuery>
{
    public GetProviderConfigurationQueryValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be positive.");
    }
}
