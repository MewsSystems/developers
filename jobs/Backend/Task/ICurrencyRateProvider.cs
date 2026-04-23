using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface ICurrencyRateProvider
    {
        /// <summary>
        /// Retrieves all available exchange rates from source.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken);
    }
}