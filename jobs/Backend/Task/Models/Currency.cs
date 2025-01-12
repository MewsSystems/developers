namespace ExchangeRateUpdater.Models;

public class Currency(string code, string name = null)
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; } = code;

    /// <summary>
    /// Name of the currency.
    /// </summary>
    public string Name { get; } = name;

    public override string ToString()
    {
        return string.IsNullOrEmpty(Name) ? Code : $"{Code} ({Name})";
    }
}