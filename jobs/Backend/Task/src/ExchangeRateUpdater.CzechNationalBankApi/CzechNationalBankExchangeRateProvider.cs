using ExchangeRateUpdater.Core.Providers;

namespace ExchangeRateUpdater
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private static IEnumerable<Currency> currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };

        public CzechNationalBankExchangeRateProvider()
        {
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            return Task.FromResult(Enumerable.Empty<ExchangeRate>());
        }
    }
}
