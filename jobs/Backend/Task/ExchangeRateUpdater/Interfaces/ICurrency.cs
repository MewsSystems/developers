namespace ExchangeRateUpdater.Interfaces
{
    public interface ICurrency
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }
    }
}
