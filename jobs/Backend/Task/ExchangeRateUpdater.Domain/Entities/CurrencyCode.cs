namespace ExchangeRateUpdater.Domain.Entities
{
    public class CurrencyCode : ConceptualString
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public override string Value { get; }

        public static implicit operator CurrencyCode(string code) => new(code);

        public CurrencyCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(code));

            if (code.Length != 3)
                throw new ArgumentException("Invalid Three-letter ISO currency code format.", code);

            Value = code;
        }
    }
}
