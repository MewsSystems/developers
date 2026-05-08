namespace ExchangeRateProvider.Domain.Constants;

public static class CurrencyServiceKeys
{
    public const string CZK = "CZK";

    public const string Default = CZK;

    private static readonly HashSet<string> SupportedCurrencies = new(StringComparer.OrdinalIgnoreCase)
    {
        CZK
    };

    public static bool IsSupported(string currencyCode)
    {
        return !string.IsNullOrWhiteSpace(currencyCode) && SupportedCurrencies.Contains(currencyCode);
    }

    public static IReadOnlyCollection<string> GetSupportedCurrencies()
    {
        return SupportedCurrencies;
    }
}
