using ExchangeRateUpdater.CbnApiClient.Dto;
using ExchangeRateUpdater.Abstraction.Model;

namespace ExchangeRateUpdater.CbnApiClient.Mapper
{

    /// <summary>
    /// Extension methods to map CNB API DTOs to domain models.
    /// </summary>
    public static class CnbExchangeRateMapperExtensions
    {
        private const string TARGET_CURRENCY_CODE = "CZK";
        /// <summary>
        /// Maps a single <see cref="ExchangeRateEntry"/> to a domain <see cref="ExchangeRate"/>.
        /// By convention the CNB API provides rates as an amount of CZK for a given <see cref="ExchangeRateEntry.Amount"/>
        /// units of the foreign currency. This method maps the entry to an ExchangeRate where the source currency
        /// is the foreign currency and the target currency is CZK. The resulting value is the rate for one unit of
        /// the source currency (Rate / Amount).
        /// </summary>
        /// <param name="entry">The DTO entry to map.</param>
        /// <returns>The mapped <see cref="ExchangeRate"/>.</returns>
        public static ExchangeRate ToDomain(this ExchangeRateEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);

            // Avoid division by zero and ensure the amount is positive as expected by the CNB API.
            if (entry.Amount <= 0)
                throw new ArgumentException("Entry amount must be greater than zero.", nameof(entry));

            // CNB returns the amount of CZK for 'Amount' units of the foreign currency.
            // Normalize to a per-1-unit rate.
            var valuePerUnit = entry.Rate / entry.Amount;

            var source = new Currency(entry.CurrencyCode);
            var target = new Currency(TARGET_CURRENCY_CODE);

            return new ExchangeRate(source, target, valuePerUnit, entry.ValidFor);
        }

        /// <summary>
        /// Maps a sequence of <see cref="ExchangeRateEntry"/> to <see cref="IEnumerable{ExchangeRate}"/>.
        /// Null entries are ignored.
        /// </summary>
        public static IEnumerable<ExchangeRate> ToDomain(this IEnumerable<ExchangeRateEntry> entries)
        {
            ArgumentNullException.ThrowIfNull(entries);

            return entries
                .Where(e => e is not null)
                .Select(e => e.ToDomain());
        }
    }
}
