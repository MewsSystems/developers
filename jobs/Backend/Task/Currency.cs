using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// County name of the currency
        /// </summary>
        public string Country { get; private set; }
        /// <summary>
        /// Name of the currency
        /// </summary>
        public string CurrencyName { get; private set; }
        /// <summary>
        /// Exchange rate of the currency in CZK
        /// </summary>
        public decimal Rate { get; private set; }
        /// <summary>
        /// Amount of money to be exchanged
        /// </summary>
        public decimal Amount { get; private set; }
    }
}
