using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public enum ProviderName
    {
        CNB,
        ECB
    }

    public interface IExchangeRateProvider
    {
        ProviderName ProviderName { get; }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}