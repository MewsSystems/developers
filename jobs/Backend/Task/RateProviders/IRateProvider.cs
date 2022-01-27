using ExchangeRateUpdater.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.RateProviders
{
    /// <summary>
    /// Provide currency rates for currencies retrieved from URL source
    /// </summary>
    public interface IRateProvider
    {
        /// <summary>
        /// Currency to which all other currencis are related to
        /// </summary>
        public Currency BaseCurrency { get; }

        /// <summary>
        /// Ammount of base currency to which are these rates specified
        /// </summary>
        public ushort BaseAmmount { get; }

        /// <summary>
        /// Retrieve data from configured URL and parse them to currency rates
        /// relative to base currency of this provider
        /// </summary>
        /// <returns>Parsed available currency rates</returns>
        /// <exception cref="Exception"></exception>
        Task<IEnumerable<CurrencyRateDto>> GetRatesAsync();

        /// <summary>
        /// Retrieve data from configured URL and parse them to currency rates
        /// relative to base currency of this provider
        /// </summary>
        /// <param name="relevantDate"></param>
        /// <returns>Parsed available currency rates</returns>
        /// <exception cref="Exception"></exception>
        Task<IEnumerable<CurrencyRateDto>> GetRatesAsync(DateTime relevantDate);
    }
}