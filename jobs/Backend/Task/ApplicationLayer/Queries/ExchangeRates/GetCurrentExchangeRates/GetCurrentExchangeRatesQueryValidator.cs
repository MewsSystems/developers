using FluentValidation;

namespace ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;

/// <summary>
/// Validator for GetCurrentExchangeRatesQuery.
/// Currently no validation rules, but demonstrates the pattern.
/// </summary>
public class GetCurrentExchangeRatesQueryValidator : AbstractValidator<GetCurrentExchangeRatesQuery>
{
    public GetCurrentExchangeRatesQueryValidator()
    {
        // No specific validation rules needed for this query
        // But the validator can be extended in the future if filtering/pagination is added
    }
}
