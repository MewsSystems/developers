using FluentValidation;

namespace ApplicationLayer.Commands.ExchangeRateProviders.ActivateProvider;

/// <summary>
/// Validator for ActivateProviderCommand.
/// </summary>
public class ActivateProviderCommandValidator : AbstractValidator<ActivateProviderCommand>
{
    public ActivateProviderCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0).WithMessage("Provider ID must be positive.");
    }
}
