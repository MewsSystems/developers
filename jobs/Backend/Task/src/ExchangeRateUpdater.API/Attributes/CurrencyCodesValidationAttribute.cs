using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater.API.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class CurrencyCodesValidationAttribute()
    : ValidationAttribute("targetCurrencies must be comma-separated 3-letter uppercase codes (e.g., USD,EUR,GBP), ISO 4217.")
{
    private readonly Regex _currencyCodePattern = new Regex(@"^[A-Z]{3}(,[A-Z]{3})*$", RegexOptions.Compiled);

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            ErrorMessage = "targetCurrencies parameter is required.";
            return false;
        }

        string? currenciesString = value.ToString();
        if (string.IsNullOrWhiteSpace(currenciesString))
        {
            ErrorMessage = "At least one currency code is required.";
            return false;
        }

        if (!_currencyCodePattern.IsMatch(currenciesString))
        {
            ErrorMessage =
                "targetCurrencies must be comma-separated 3-letter uppercase codes (e.g., USD,EUR,GBP), ISO 4217.";
            return false;
        }

        return true;
    }
}