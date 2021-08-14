namespace ExchangeRateUpdater
{
    /// <param name="Code">
    /// Three-letter ISO 4217 code of the currency.
    /// </param>
    public sealed record Currency(string Code) { }
}
