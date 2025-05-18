using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using FluentValidation;
using NodaTime.Text;

namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Validators;

public class BatchRateRequestValidator : AbstractValidator<BatchRateRequest>
{
    public BatchRateRequestValidator()
    {
        RuleFor(x => x.Date).
            Must(BeIso8601Date).
            When(x => x.Date != null).
            WithMessage("Date must be ISO-8601 (yyyy-MM-dd)");

        RuleForEach(x => x.CurrencyPairs).
            Must(BeIso4217Pair).
            WithMessage("Each pair must be ISO-4217, e.g. USD/CZK");

        RuleFor(x => x.CurrencyPairs).
            NotEmpty().
            WithMessage("At least one currency pair must be specified");
    }

    private static bool BeIso8601Date(string? date)
    {
        if (string.IsNullOrWhiteSpace(date))
            return true;

        var parseResult = LocalDatePattern.Iso.Parse(date);
        return parseResult.Success;
    }

    private static bool BeIso4217Pair(string pair)
    {
        if (string.IsNullOrWhiteSpace(pair))
            return false;

        var parts = pair.Split('/');
        if (parts.Length != 2)
            return false;

        return IsIso4217CurrencyCode(parts[0]) && IsIso4217CurrencyCode(parts[1]);
    }

    private static bool IsIso4217CurrencyCode(string code)
    {
        // Simple validation: ISO 4217 codes are 3 uppercase letters
        return !string.IsNullOrWhiteSpace(code) &&
               code.Length == 3 &&
               code.All(char.IsLetter) &&
               code.All(char.IsUpper);
    }
}