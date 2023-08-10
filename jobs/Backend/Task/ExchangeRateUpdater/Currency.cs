namespace ExchangeRateUpdater
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public record Currency(string Code)
    {
        public static implicit operator string(Currency currency) => currency.Code;
        public static implicit operator Currency(string currencyCode) => new(currencyCode);

        public override string ToString()
        {
            return Code;
        }
    }
}
