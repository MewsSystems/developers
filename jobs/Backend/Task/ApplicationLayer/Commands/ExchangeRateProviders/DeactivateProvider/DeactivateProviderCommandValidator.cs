using FluentValidation;

namespace ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;

public class DeactivateProviderCommandValidator : AbstractValidator<DeactivateProviderCommand>
{
    public DeactivateProviderCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be positive.");
    }
}
