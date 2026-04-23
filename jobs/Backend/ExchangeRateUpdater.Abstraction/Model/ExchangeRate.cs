namespace ExchangeRateUpdater.Abstraction.Model
{
    /// <summary>
    /// Represents an exchange rate between two currencies, providing the source currency, target currency, and the
    /// exchange rate value.
    /// </summary>
    public class ExchangeRate
    {

        /// <summary>
        /// Initializes a new instance of the ExchangeRate class using the specified source and target currencies and
        /// the exchange rate value.
        /// </summary>
        /// <param name="sourceCurrency">The currency from which the exchange rate is calculated.</param>
        /// <param name="targetCurrency">The currency to which the exchange rate is applied.</param>
        /// <param name="value">The exchange rate value. Must be greater than zero.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value parameter is less than or equal to zero.</exception>
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, DateTime validFor)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Exchange rate value must be greater than zero.");

            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
            ValidFor = validFor;
        }

        /// <summary>
        /// Gets the currency used as the source for conversion.
        /// </summary>
        public Currency SourceCurrency { get; }

        /// <summary>
        /// Gets the currency to which the transaction amount will be converted.
        /// </summary>
        public Currency TargetCurrency { get; }

        /// <summary>
        /// Gets the current exchange rate value represented by this instance. 
        /// How much of the target currency is equivalent to one unit of the source currency.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Gets the date and time until which the current entity is valid.
        /// </summary>
        public DateTime ValidFor { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value} (valid for {ValidFor})";
        }
    }
}
