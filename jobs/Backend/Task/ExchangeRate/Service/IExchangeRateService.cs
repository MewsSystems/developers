using ExchangeRateUpdater.ExchangeRate.Controller.Model;
using ExchangeRateUpdater.ExchangeRate.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRate.Service
{
    /// <summary>
    /// Defines the contract for the exchange rate service.
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// Retrieves daily exchange rates for the specified currencies on the given date.
        /// </summary>
        /// <param name="request">The request containing the base currency, date, language, and target currencies.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the fetched exchange rates.</returns>
        Task<FetchDailyExchangeRateResponseInternal> GetDailyExchangeRatesAsync(
            FetchDailyExchangeRateRequestInternal request,
            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the daily exchange rates in the repository by crawling the source for the specified currency and date.
        /// </summary>
        /// <param name="currency">The base currency.</param>
        /// <param name="date">The date for which to update the exchange rates.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateDailyExchangeRates(
            Currency currency,
            DateOnly date,
            CancellationToken cancellationToken);

        /// <summary>
        /// Sets up a worker that periodically updates the exchange rates in the repository.
        /// </summary>
        void SetupExchangeRateUpdaterWorker();
    }
}
