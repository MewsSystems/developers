using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdated.CnbApi.Model
{
    public static class MappingsExtensions
    {
        /// <summary>
        /// Converting from inner CnBApi type to application type.
        /// </summary>
        /// <remarks>The type of <paramref name="rate"/> has the rate value defined to an Amount of the currency.
        /// For internal ExchangeRate is always defined for a single unit of the target currency.
        /// </remarks>
        public static ExchangeRate ToExchangeRate(this ExRateDailyRest rate)
        {
            return new ExchangeRate(
                Constants.Currencies.CZK,
                new Currency(rate.CurrencyCode),
                (decimal)rate.Rate / (decimal)rate.Amount,
                rate.ValidFor.Value);
        }
    }
}
