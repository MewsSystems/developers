namespace ExchangeRateUpdater
{
    /// <summary>
    /// Calculatro for exchange rates from underlying data source based on currency codes.
    /// </summary>
    public sealed class ExchangeRateCalculator : IExchangeRateCalculator
    {
        private readonly IExchangeRateDataSource dataSource;

        /// <summary>
        /// Initializes a new instance of <see cref="ExchangeRateCalculator"/>.
        /// </summary>
        /// <param name="dataSource">Data source containing exchange rates values.</param>
        public ExchangeRateCalculator(IExchangeRateDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        /// <inheritdoc />
        public bool TryGet(string currencyCode, out decimal exchangeRate)
        {
            if (dataSource.TryGet(currencyCode, out var amount, out var rate))
            {
                exchangeRate = Calculate(amount, rate);

                return true;
            }

            exchangeRate = decimal.Zero;

            return false;
        }

        /// <summary>
        /// Calculates exchange rates from provided data.
        /// </summary>
        private static decimal Calculate(int amount, decimal rate)
        {
            return amount / rate;
        }
    }
}
