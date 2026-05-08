namespace ExchangeRateUpdater.Core.Models
{
    /// <summary>
    /// Represents an exchange rate between two currencies.
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRate"/> class.
        /// </summary>
        /// <param name="sourceCurrency">The source currency (the currency to be converted from).</param>
        /// <param name="targetCurrency">The target currency (the currency to be converted to).</param>
        /// <param name="value">The exchange rate value, which must be greater than zero.</param>
        /// <exception cref="ArgumentException">Thrown when source or target currency is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is less than or equal to zero.</exception>
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            if (string.IsNullOrWhiteSpace(sourceCurrency.Code) || string.IsNullOrWhiteSpace(targetCurrency.Code))
            {
                throw new ArgumentException("Source and target currencies cannot be null or empty.");
            }
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Exchange rate value must be greater than zero.");
            }
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        /// <summary>
        /// The source currency of the exchange rate.
        /// </summary>
        public Currency SourceCurrency { get; }

        /// <summary>
        /// The target currency of the exchange rate.
        /// </summary>
        public Currency TargetCurrency { get; }

        /// <summary>
        /// The value of the exchange rate, representing how much one unit of the source currency is worth in the target currency.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Overrides the ToString method to return a string representation of the exchange rate.
        /// </summary>
        /// <returns>A string in the format "SourceCurrency/TargetCurrency=Value".</returns>
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current exchange rate.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if the specified object is equal to the current exchange rate; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is ExchangeRate rate &&
                   EqualityComparer<Currency>.Default.Equals(SourceCurrency, rate.SourceCurrency) &&
                   EqualityComparer<Currency>.Default.Equals(TargetCurrency, rate.TargetCurrency) &&
                   Value == rate.Value;
        }

        /// <summary>
        /// Generates a hash code for the current exchange rate.
        /// </summary>
        /// <returns>A hash code for the current exchange rate.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(SourceCurrency, TargetCurrency, Value);
        }

        /// <summary>
        /// Overloaded equality operator to compare two ExchangeRate objects.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>true if both ExchangeRate objects are equal; otherwise, false.</returns>
        public static bool operator ==(ExchangeRate? left, ExchangeRate? right)
        {
            return EqualityComparer<ExchangeRate>.Default.Equals(left, right);
        }

        /// <summary>
        /// Overloaded inequality operator to compare two ExchangeRate objects.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>true if both ExchangeRate objects are not equal; otherwise, false.</returns>
        public static bool operator !=(ExchangeRate? left, ExchangeRate? right)
        {
            return !(left == right);
        }
    }
}
