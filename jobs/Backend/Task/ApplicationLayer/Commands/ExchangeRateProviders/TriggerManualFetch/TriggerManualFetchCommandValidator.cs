using FluentValidation;

namespace ApplicationLayer.Commands.ExchangeRateProviders.TriggerManualFetch;

public class TriggerManualFetchCommandValidator : AbstractValidator<TriggerManualFetchCommand>
{
    public TriggerManualFetchCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be positive.");
    }
}
