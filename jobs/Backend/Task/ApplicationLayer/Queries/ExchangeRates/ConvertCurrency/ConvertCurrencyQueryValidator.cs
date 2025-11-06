using FluentValidation;

namespace ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;

public class ConvertCurrencyQueryValidator : AbstractValidator<ConvertCurrencyQuery>
{
    public ConvertCurrencyQueryValidator()
    {
        RuleFor(x => x.SourceCurrencyCode)
            .NotEmpty()
            .Length(3)
            .Matches("^[A-Z]{3}$")
            .WithMessage("Source currency code must be a 3-letter ISO code.");

        RuleFor(x => x.TargetCurrencyCode)
            .NotEmpty()
            .Length(3)
            .Matches("^[A-Z]{3}$")
            .WithMessage("Target currency code must be a 3-letter ISO code.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be positive.");

        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .When(x => x.ProviderId.HasValue)
            .WithMessage("Provider ID must be positive when specified.");

        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .When(x => x.Date.HasValue)
            .WithMessage("Date cannot be in the future.");
    }
}
