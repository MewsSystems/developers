using System;
using System.Threading.Tasks;

namespace Mews.ExchangeRate.Updater.Services.Abstractions
{
    public interface IExchangeRateUpdateService
    {
        /// <summary>
        /// Refreshes the exchange rates asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<bool> RefreshAllExchangeRatesAsync();

        /// <summary>
        /// Refreshes the exchange rates asynchronously with the data of a given date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        Task<bool> RefreshAllExchangeRatesAsync(DateTime date);


        /// <summary>
        /// Refreshes the exchange rates asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<bool> RefreshCurrencyExchangeRatesAsync();

        /// <summary>
        /// Refreshes the exchange rates asynchronously with the data of a given date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        Task<bool> RefreshCurrencyExchangeRatesAsync(DateTime date);


        /// <summary>
        /// Refreshes the exchange rates asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<bool> RefreshForeignCurrencyExchangeRatesAsync();

        /// <summary>
        /// Refreshes the exchange rates asynchronously with the data of a given date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        Task<bool> RefreshForeignCurrencyExchangeRatesAsync(DateTime date);
    }
}