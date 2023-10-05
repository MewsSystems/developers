using System.Collections.Generic;
using System.Threading.Tasks;
using Mews.ExchangeRate.Domain.Models;
using System.Data;

namespace Mews.ExchangeRate.Storage.Abstractions
{
    public interface IExchangeRateStorageService
    {
        /// <summary>
        /// Gets a value indicating whether the storage service is initialized and ready to use.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets the exchange rates for the given currencies asynchronously.
        /// </summary>
        /// <param name="originCurrencies">The origin currencies.</param>
        /// <returns></returns>
        Task<IEnumerable<Domain.Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> originCurrencies);

        /// <summary>
        /// Gets the exchange rate for the given currency asynchronously.
        /// </summary>
        /// <param name="originCurrency">The origin currency.</param>
        /// <returns></returns>
        Task<Domain.Models.ExchangeRate> GetExchangeRateAsync(Currency originCurrency);

        /// <summary>
        /// Sets the exchange rates asynchronously.
        /// </summary>
        /// <param name="exchangeRates">The exchange rates.</param>
        /// <param name="source">The source of the data.</param>
        /// <param name="updateStatus">The update status.</param>
        /// <returns></returns>
        Task<bool> SetExchangeRatesAsync(IEnumerable<Domain.Models.ExchangeRate> exchangeRates, string source, UpdateStatus? updateStatus);
    }
}