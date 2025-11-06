using FluentValidation;

namespace ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;

public class BulkUpsertExchangeRatesCommandValidator : AbstractValidator<BulkUpsertExchangeRatesCommand>
{
    public BulkUpsertExchangeRatesCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be positive.");

        RuleFor(x => x.ValidDate)
            .NotEmpty()
            .WithMessage("Valid date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
            .WithMessage("Valid date cannot be more than 1 day in the future.");

        RuleFor(x => x.Rates)
            .NotNull()
            .WithMessage("Rates collection is required.")
            .NotEmpty()
            .WithMessage("At least one exchange rate must be provided.");

        RuleForEach(x => x.Rates)
            .ChildRules(rate =>
            {
                rate.RuleFor(r => r.SourceCurrencyCode)
                    .NotEmpty()
                    .Length(3)
                    .Matches("^[A-Z]{3}$")
                    .WithMessage("Source currency code must be a 3-letter ISO code.");

                rate.RuleFor(r => r.TargetCurrencyCode)
                    .NotEmpty()
                    .Length(3)
                    .Matches("^[A-Z]{3}$")
                    .WithMessage("Target currency code must be a 3-letter ISO code.");

                rate.RuleFor(r => r.Rate)
                    .GreaterThan(0)
                    .WithMessage("Rate must be positive.");

                rate.RuleFor(r => r.Multiplier)
                    .GreaterThan(0)
                    .WithMessage("Multiplier must be positive.");
            });
    }
}
