namespace ExchangeRateUpdater.Models;

/// <summary>
///     Gets the three-letter ISO 4217 currency code (e.g., "USD", "EUR", "CZK").
/// </summary>
public class Currency
{
    public Currency(string code)
    {
        Code = code;
    }

    public string Code { get; }

    public override string ToString()
    {
        return Code;
    }
}