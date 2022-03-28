using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly Currency czechCrown = new Currency("CZK");

        private readonly IExchangeRateCalculator calculator;

        /// <summary>
        /// Initializes a new instance of <see cref="ExchangeRateProvider"/>.
        /// </summary>
        /// <param name="calculator">Exchange rate calculator.</param>
        public ExchangeRateProvider(IExchangeRateCalculator calculator)
        {
            this.calculator = calculator;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public ICollection<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var result = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                if (currency.Code == czechCrown.Code)
                {
                    result.Add(new ExchangeRate(czechCrown, currency, 1.0M));
                    continue;
                }

                if (calculator.TryGet(currency.Code, out var rate))
                {
                    result.Add(new ExchangeRate(czechCrown, currency, rate));
                    continue;
                }
            }

            return result;
        }
    }
}
