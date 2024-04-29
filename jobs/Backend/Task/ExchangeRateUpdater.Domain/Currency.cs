namespace ExchangeRateUpdater.Domain
{
    public class Currency
    {
        public Currency(string? code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            {
                throw new ArgumentException("Currency code must be a three-letter ISO 4217 code.", nameof(code));
            }

            Code = code.ToUpperInvariant();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}
