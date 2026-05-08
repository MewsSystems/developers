using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;

namespace ExchangeRateUpdater.Core.Common;

public static class CurrencyPairParser
{
    public static CurrencyPair ParseCurrencyPair(string currencyPair)
    {
        if (string.IsNullOrWhiteSpace(currencyPair))
            throw new ArgumentException("Currency pair cannot be null or empty", nameof(currencyPair));

        var parts = currencyPair.Split('/');
        if (parts.Length != 2)
            throw new ArgumentException("Currency pair must be in format SOURCE/TARGET, e.g. USD/CZK", nameof(currencyPair));

        var sourceCurrency = parts[0].
            Trim();
        var targetCurrency = parts[1].
            Trim();

        if (string.IsNullOrWhiteSpace(sourceCurrency) || string.IsNullOrWhiteSpace(targetCurrency))
            throw new ArgumentException("Both source and target currency codes must be specified", nameof(currencyPair));

        if (!IsValidCurrencyCode(sourceCurrency) || !IsValidCurrencyCode(targetCurrency))
            throw new ArgumentException("Currency codes must be valid ISO 4217 codes (3 uppercase letters)", nameof(currencyPair));

        return new CurrencyPair
        {
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency
        };
    }

    private static bool IsValidCurrencyCode(string code)
    {
        return !string.IsNullOrWhiteSpace(code) &&
               code.Length == 3 &&
               code.All(char.IsLetter) &&
               code.All(char.IsUpper);
    }
}