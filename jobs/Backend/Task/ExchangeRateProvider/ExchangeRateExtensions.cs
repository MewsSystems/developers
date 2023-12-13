using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Internal Extensions used at application layer.
    /// </summary>
    internal static class ExchangeRateExtensions
    {
        /// <summary>
        /// Gets true, when the both Source and Target currencies are available in the passed currencies
        /// </summary>
        public static bool IsSpecifiedFor(this ExchangeRate rate, IEnumerable<Currency> currencies)
        {
            return currencies.Contains(rate.SourceCurrency) && currencies.Contains(rate.TargetCurrency);
        }
    }
}
