using System.Collections.Generic;
using System.Threading.Tasks;
using Mews.ExchangeRate.Domain.Models;

namespace Mews.ExchangeRate.Storage.Abstractions
{
    public interface IExchangeRateQueryRepository
    {
        /// <summary>
        /// Gets a value indicating whether the storage service is initialized and ready to use.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        bool IsInitializedAndReady { get; }

        /// <summary>
        /// Gets the exchange rates for the given currencies asynchronously.
        /// </summary>
        /// <param name="sourceCurrencies">The origin currencies.</param>
        /// <returns></returns>
        Task<IEnumerable<Domain.Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> sourceCurrencies);

        /// <summary>
        /// Gets the exchange rate for the given currency asynchronously.
        /// </summary>
        /// <param name="sourceCurrency">The origin currency.</param>
        /// <returns></returns>
        Task<Domain.Models.ExchangeRate> GetExchangeRateAsync(Currency sourceCurrency);
    }
}