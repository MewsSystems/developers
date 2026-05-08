using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Queries;
using FluentValidation;
using NodaTime.Text;

namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Validators;

public class GetExchangeRateQueryValidator : AbstractValidator<GetExchangeRateQuery>
{
    public GetExchangeRateQueryValidator()
    {
        RuleFor(x => x.SourceCurrency).
            NotEmpty().
            WithMessage("Source currency is required").
            Must(BeValidIso4217CurrencyCode).
            WithMessage("Source currency must be a valid ISO 4217 code (3 uppercase letters)");

        RuleFor(x => x.TargetCurrency).
            NotEmpty().
            WithMessage("Target currency is required").
            Must(BeValidIso4217CurrencyCode).
            WithMessage("Target currency must be a valid ISO 4217 code (3 uppercase letters)");

        RuleFor(x => x.Date).
            Must(BeIso8601DateOrNull).
            WithMessage("Date must be ISO-8601 (yyyy-MM-dd)");
    }

    /// <summary>
    /// Validates if a string is a valid ISO-8601 date or null/empty
    /// </summary>
    public static bool BeIso8601DateOrNull(string? date)
    {
        if (string.IsNullOrWhiteSpace(date))
            return true;

        var parseResult = LocalDatePattern.Iso.Parse(date);
        return parseResult.Success;
    }

    /// <summary>
    /// Validates if a string is a valid ISO-4217 currency code (3 uppercase letters)
    /// </summary>
    public static bool BeValidIso4217CurrencyCode(string code)
    {
        // Simple validation: ISO 4217 codes are 3 uppercase letters
        return !string.IsNullOrWhiteSpace(code) &&
               code.Length == 3 &&
               code.All(char.IsLetter) &&
               code.All(char.IsUpper);
    }
}