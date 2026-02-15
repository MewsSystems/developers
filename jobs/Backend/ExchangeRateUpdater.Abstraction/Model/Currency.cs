namespace ExchangeRateUpdater.Abstraction.Model
{
    public class Currency
    {
        /// <summary>
        /// Initializes a new instance of the Currency class using the specified currency code.
        /// </summary>
        /// <param name="code">The currency code to represent. This value must be a non-empty string consisting of exactly three letters (e.g., "USD", "EUR"). It must not contain any whitespace or non-letter
        /// characters. The code is trimmed and converted to uppercase.</param>
        /// <exception cref="ArgumentException">Thrown if the specified <paramref name="code"/> is null, empty, or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentException">Thrown if the specified <paramref name="code"/> does not have exactly three characters.</exception>
        /// <exception cref="ArgumentException">Thrown if the specified <paramref name="code"/> contains non-letter characters.</exception>
        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Currency code must not be empty.", nameof(code));

            code = code.Trim();
            if (code.Length != 3)
                throw new ArgumentException("Currency code must be exactly three characters long.", nameof(code));

            if (!code.All(char.IsLetter))
                throw new ArgumentException("Currency code must consist of letters only.", nameof(code));

            Code = code.Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        // Compare using OrdinalIgnoreCase so the comparison is
        // culture-invariant and consistent across environments 
        // (e.g. Turkish dotted/dotless i behaviour).
        public bool Equals(Currency other) => string.Equals(Code, other.Code, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj) => obj is Currency other && Equals(other);

        // Use the same comparer for hash code to respect equality contract.
        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Code);
    }
}
