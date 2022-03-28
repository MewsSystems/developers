namespace ExchangeRateUpdater
{
    /// <summary>
    /// Describes structure capable of calculating exchange rate from underlying data source based on currency codes.
    /// </summary>
    public interface IExchangeRateCalculator
    {
        /// <summary>
        /// Tries to retrieve exchange rate between Czech Crown and other currency identified by <paramref name="currencyCode"/>.
        /// </summary>
        /// <param name="currencyCode">Three-letter ISO 4217 code of the currency.</param>
        /// <param name="exchangeRate">Exchange rate value when data source contains it.</param>
        /// <returns><c>true</c> in case that currency exists and underlying data source contains exchange rate, <c>false</c> otherwise.</returns>
        bool TryGet(string currencyCode, out decimal exchangeRate);
    }
}