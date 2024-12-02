namespace ExchangeRateUpdater.CnbRates;

/// <summary>
/// Response of CNB Exchange Rate API. The retrieved rates are against Czech Crown (CZK).
/// </summary>
public class CnbRateResult
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string CurrencyCode { get; init; } = null!;

    /// <summary>
    /// Base for the exchange rate, i.e. the amount of foreign currency defined by <see cref="CurrencyCode"/> for which the <see cref="Rate"/> is applicable.
    /// </summary>
    public int Amount { get; init; }

    /// <summary>
    /// How many CZK is <see cref="Amount"/> of the <see cref="CurrencyCode"/> worth.
    /// </summary>
    public decimal Rate { get; init; }
}