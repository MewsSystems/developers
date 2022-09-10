namespace Domain.Model
{
    public class Currency
    {
        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code), "Currency code should not be null, empty or whitespace.");
            if (code.Length != 3)
                throw new ArgumentException("Currency code should only contain 3 letters.", nameof(code));

            Code = code;
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