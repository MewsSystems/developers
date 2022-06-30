namespace ExchangeRateUpdater
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public record Currency(string Code)
    {
        public override string ToString() => Code;
    }
}
