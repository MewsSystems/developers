using System.Collections.Generic;
using ExchangeRateProvider.Model;

namespace ExchangeRateProvider
{
    /// <summary>
    /// ExchangeRatesProvider contract
    /// </summary>
    internal interface IExchangeRateProvider
    {
        /// <summary>
        /// GetExchangeRates contract
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        IEnumerable<ExchangeRateDto> GetExchangeRates(IEnumerable<CurrencyDto> currencies);
    }
}