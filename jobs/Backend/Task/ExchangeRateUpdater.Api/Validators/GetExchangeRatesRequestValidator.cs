using FluentValidation;
using ExchangeRateUpdater.Api.Models;

namespace ExchangeRateUpdater.Api.Validators;

public class GetExchangeRatesRequestValidator : AbstractValidator<GetExchangeRatesRequest>
{
    private static readonly string[] ValidCurrencyCodes =
    [
        "USD", "EUR", "CZK", "JPY", "KES", "RUB", "THB", "TRY", "XYZ"
    ];

    public GetExchangeRatesRequestValidator()
    {
        RuleFor(x => x.CurrencyCodes)
            .Must(codes => codes.Any())
            .WithMessage("At least one currency code must be provided")
            .Must(codes => codes.All(code => ValidCurrencyCodes.Contains(code)))
            .WithMessage("One or more invalid currency codes provided. Valid codes are: " + string.Join(", ", ValidCurrencyCodes));

        RuleFor(x => x.Date)
            .Must(date => date == null || date <= DateTime.UtcNow)
            .WithMessage("Date cannot be in the future");
    }
} 