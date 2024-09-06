using ExchangeRateUpdater.ExchangeRate.Model;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRate.Repository
{
    /// <summary>
    /// Represents a repository for storing and retrieving exchange rate datasets.
    /// </summary>
    public interface IExchangeRateRepository
    {
        /// <summary>
        /// Saves the exchange rates dataset with the specified key.
        /// </summary>
        /// <param name="key">The key identifying the dataset.</param>
        /// <param name="dataset">The exchange rate dataset to be saved.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the dataset was successfully saved; otherwise, false.</returns>
        Task<bool> SaveExchangeRates(string key, ExchangeRateDataset dataset);

        /// <summary>
        /// Retrieves the exchange rate dataset associated with the specified key.
        /// </summary>
        /// <param name="key">The key identifying the dataset.</param>
        /// <returns>A task representing the asynchronous operation, returning the exchange rate dataset if found; otherwise, null.</returns>
        Task<ExchangeRateDataset> GetExchangeRates(string key);
    }
}
