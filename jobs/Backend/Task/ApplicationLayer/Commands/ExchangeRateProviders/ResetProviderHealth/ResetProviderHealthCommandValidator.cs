using FluentValidation;

namespace ApplicationLayer.Commands.ExchangeRateProviders.ResetProviderHealth;

public class ResetProviderHealthCommandValidator : AbstractValidator<ResetProviderHealthCommand>
{
    public ResetProviderHealthCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be positive.");
    }
}
