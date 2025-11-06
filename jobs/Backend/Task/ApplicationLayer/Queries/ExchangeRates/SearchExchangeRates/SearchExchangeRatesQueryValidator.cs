using FluentValidation;

namespace ApplicationLayer.Queries.ExchangeRates.SearchExchangeRates;

public class SearchExchangeRatesQueryValidator : AbstractValidator<SearchExchangeRatesQuery>
{
    public SearchExchangeRatesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100.");

        RuleFor(x => x.SourceCurrencyCode)
            .Length(3)
            .Matches("^[A-Z]{3}$")
            .When(x => !string.IsNullOrEmpty(x.SourceCurrencyCode))
            .WithMessage("Source currency code must be a 3-letter ISO code.");

        RuleFor(x => x.TargetCurrencyCode)
            .Length(3)
            .Matches("^[A-Z]{3}$")
            .When(x => !string.IsNullOrEmpty(x.TargetCurrencyCode))
            .WithMessage("Target currency code must be a 3-letter ISO code.");

        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .When(x => x.ProviderId.HasValue)
            .WithMessage("Provider ID must be positive when specified.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate!.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date.");

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue ||
                      (x.EndDate.Value.ToDateTime(TimeOnly.MinValue) - x.StartDate.Value.ToDateTime(TimeOnly.MinValue)).TotalDays <= 365)
            .WithMessage("Date range cannot exceed 365 days.");

        RuleFor(x => x.MinRate)
            .GreaterThan(0)
            .When(x => x.MinRate.HasValue)
            .WithMessage("Minimum rate must be positive.");

        RuleFor(x => x.MaxRate)
            .GreaterThan(0)
            .When(x => x.MaxRate.HasValue)
            .WithMessage("Maximum rate must be positive.")
            .GreaterThanOrEqualTo(x => x.MinRate!.Value)
            .When(x => x.MinRate.HasValue && x.MaxRate.HasValue)
            .WithMessage("Maximum rate must be greater than or equal to minimum rate.");
    }
}
