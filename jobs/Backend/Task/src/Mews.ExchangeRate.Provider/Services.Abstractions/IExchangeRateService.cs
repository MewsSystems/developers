using Mews.ExchangeRate.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mews.ExchangeRate.Provider.Services.Abstractions
{
    public interface IExchangeRateService
    {
        /// <summary>
        /// Gets the exchange rate asynchronously.
        /// </summary>
        /// <param name="sourceCurrency">The source currency.</param>
        /// <returns></returns>
        Task<Domain.Models.ExchangeRate> GetExchangeRateAsync(Currency sourceCurrency);

        /// <summary>
        /// Gets the exchange rates asynchronously.
        /// </summary>
        /// <param name="sourceCurrencies"></param>
        /// <returns></returns>
        Task<IEnumerable<Domain.Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> sourceCurrencies);


    }
}