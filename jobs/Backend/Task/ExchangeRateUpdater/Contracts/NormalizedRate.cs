namespace ExchangeRateUpdater.Contracts;

/// <summary>
/// Exchange rate abstracted from bank specific result contract.
/// </summary>
public record struct NormalizedRate
{
    /// <summary>
    /// Currency relevant to the exchange rate.
    /// </summary>
    public Currency Currency { get; }

    /// <summary>
    /// The value of the exchange rate.
    /// </summary>
    public decimal Value { get; }

    public NormalizedRate(string currencyCode, decimal value)
    {
        Currency = new Currency(currencyCode);
        Value = value;
    }
}