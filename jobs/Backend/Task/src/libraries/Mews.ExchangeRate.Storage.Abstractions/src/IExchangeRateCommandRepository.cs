using System.Collections.Generic;
using System.Threading.Tasks;
using Mews.ExchangeRate.Domain.Models;
using static Mews.ExchangeRate.Storage.Abstractions.Models.StorageStatus;

namespace Mews.ExchangeRate.Storage.Abstractions
{
    public interface IExchangeRateCommandRepository
    {
        /// <summary>
        /// Gets a value indicating whether the storage service is initialized and ready to use.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        bool IsInitializedAndReady { get; }

        /// <summary>
        /// Sets the exchange rates asynchronously.
        /// </summary>
        /// <param name="exchangeRates">The exchange rates.</param>
        /// <param name="source">The source.</param>
        /// <param name="updateStatus">The update status.</param>
        /// <returns></returns>
        Task<bool> SetExchangeRatesAsync(IEnumerable<Domain.Models.ExchangeRate> exchangeRates, string source, UpdateStatus? updateStatus);
    }
}