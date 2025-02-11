using ExchangeRateUpdater.Application.Queries;
using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater.Application.Validators;

/// <summary>
/// Validates the date and currency inputs for exchange rate queries.
/// </summary>
public class GetExchangeRatesQueryValidator : AbstractValidator<GetExchangeRatesQuery>
{
    private const int MaxCurrencyCount = 20;
    private static readonly Regex CurrencyCodeRegex = new(@"^[A-Z]{3}$", RegexOptions.Compiled);

    /// <summary>
    /// Initializes a new instance of the <see cref="GetExchangeRatesQueryValidator"/> class.
    /// </summary>
    public GetExchangeRatesQueryValidator()
    {
        RuleFor(x => x.Date)
            .Cascade(CascadeMode.Stop)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("The date cannot be in the future.")
            .GreaterThanOrEqualTo(new DateTime(1991, 1, 1, 0, 0, 0, DateTimeKind.Utc))
            .WithMessage("The date must be after January 1, 1991 (when CNB started providing exchange rates).")
            .When(x => x.Date is not null);

        RuleFor(x => x.Currencies)
            .Must(currencies => currencies is { Count: <= MaxCurrencyCount })
            .WithMessage($"You can request a maximum of {MaxCurrencyCount} currencies at a time.")
            .Must(currencies => currencies!.All(currency => CurrencyCodeRegex.IsMatch(currency)))
            .WithMessage("All currencies must be 3-letter uppercase ISO 4217 codes (e.g., USD, EUR).")
            .When(x => x.Currencies is not null);
    }
}
