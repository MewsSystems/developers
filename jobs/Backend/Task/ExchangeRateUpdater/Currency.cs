namespace ExchangeRateUpdater
{
    public record Currency(string Code)
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; init; } = Code;
        public override string ToString()
        {
            return Code;
        }
    };
}
