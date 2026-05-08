namespace ExchangeRateUpdater.Core.Models
{
    /// <summary>
    /// Represents a currency.
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <param name="code">The three-letter ISO 4217 code of the currency.</param>
        /// <exception cref="ArgumentException"></exception>
        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));
            }
            Code = code.ToUpperInvariant();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Overrides the ToString method to return the currency code.
        /// </summary>
        /// <returns>The currency code.</returns>
        public override string ToString()
        {
            return Code;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current currency.
        /// </summary>
        /// <param name="obj">The object to compare with the current currency.</param>
        /// <returns>true if the specified object is equal to the current currency; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Currency currency && Code == currency.Code;
        }

        /// <summary>
        /// Generates a hash code for the current currency.
        /// </summary>
        /// <returns>A hash code for the current currency.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Code);
        }

        /// <summary>
        /// Overloaded equality operator to compare two Currency objects.
        /// </summary>
        /// <param name="left">The first Currency object to compare.</param>
        /// <param name="right">The second Currency object to compare.</param>
        /// <returns>true if both Currency objects are equal; otherwise, false.</returns>
        public static bool operator ==(Currency? left, Currency? right)
        {
            return EqualityComparer<Currency>.Default.Equals(left, right);
        }

        /// <summary>
        /// Overloaded inequality operator to compare two Currency objects.
        /// </summary>
        /// <param name="left">The first Currency object to compare.</param>
        /// <param name="right">The second Currency object to compare.</param>
        /// <returns>true if both Currency objects are not equal; otherwise, false.</returns>
        public static bool operator !=(Currency? left, Currency? right)
        {
            return !(left == right);
        }
    }
}
