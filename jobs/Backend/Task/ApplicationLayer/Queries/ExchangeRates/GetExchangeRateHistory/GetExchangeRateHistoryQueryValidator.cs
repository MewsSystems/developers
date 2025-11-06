using FluentValidation;

namespace ApplicationLayer.Queries.ExchangeRates.GetExchangeRateHistory;

public class GetExchangeRateHistoryQueryValidator : AbstractValidator<GetExchangeRateHistoryQuery>
{
    public GetExchangeRateHistoryQueryValidator()
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

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater than or equal to start date.");

        RuleFor(x => x)
            .Must(x => (x.EndDate.ToDateTime(TimeOnly.MinValue) - x.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays <= 365)
            .WithMessage("Date range cannot exceed 365 days.");

        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .When(x => x.ProviderId.HasValue)
            .WithMessage("Provider ID must be positive when specified.");
    }
}
