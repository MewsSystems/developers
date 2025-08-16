namespace ExchangeRateUpdater.Models
{
    public sealed record Currency(string Code)
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public override string ToString()
        {
            return Code;
        }
    }
}