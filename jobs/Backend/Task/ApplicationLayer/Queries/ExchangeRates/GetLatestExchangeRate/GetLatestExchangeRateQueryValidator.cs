using FluentValidation;

namespace ApplicationLayer.Queries.ExchangeRates.GetLatestExchangeRate;

public class GetLatestExchangeRateQueryValidator : AbstractValidator<GetLatestExchangeRateQuery>
{
    public GetLatestExchangeRateQueryValidator()
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

        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .When(x => x.ProviderId.HasValue)
            .WithMessage("Provider ID must be positive when specified.");
    }
}
