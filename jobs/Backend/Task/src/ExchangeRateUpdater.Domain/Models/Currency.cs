namespace ExchangeRateUpdater.Domain.Models
{
    public record Currency
    {
        public string Code { get; }

        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Currency code cannot be null, empty, or whitespace", nameof(code));
            }

            if (code.Length != 3)
            {
                throw new ArgumentException("Currency code must be exactly 3 characters", nameof(code));
            }

            Code = code.ToUpperInvariant();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public override string ToString()
        {
            return Code;
        }
    }
}
