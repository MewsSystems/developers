using ExchangeRateUpdater.ExchangeRate.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRate.Repository
{
    /// <summary>
    /// In-memory repository for storing and retrieving exchange rate datasets.
    /// </summary>
    public class InMemoryExchangeRateRepository : IExchangeRateRepository
    {
        private readonly IDictionary<string, ExchangeRateDataset> _exchangeRateData;
        private readonly object _lockObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryExchangeRateRepository"/> class.
        /// </summary>
        public InMemoryExchangeRateRepository()
        {
            _exchangeRateData = new Dictionary<string, ExchangeRateDataset>();
            _lockObject = new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryExchangeRateRepository"/> class with the specified dataset and lock object.
        /// </summary>
        /// <param name="dataset">The initial dataset.</param>
        /// <param name="lockObject">The lock object.</param>
        public InMemoryExchangeRateRepository(IDictionary<string, ExchangeRateDataset> dataset, object lockObject)
        {
            _exchangeRateData = dataset;
            _lockObject = lockObject;
        }

        /// <inheritdoc/>
        public Task<ExchangeRateDataset> GetExchangeRates(string key)
        {
            lock (_lockObject)
            {
                return Task.FromResult(_exchangeRateData.TryGetValue(key, out var value) ? value : null);
            }
        }

        /// <inheritdoc/>
        public Task<bool> SaveExchangeRates(string key, ExchangeRateDataset dataset)
        {
           lock (_lockObject)
            {
                if (dataset.ExchangeRates.Any())
                {
                    _exchangeRateData[key] = dataset;
                }
                return Task.FromResult(true);
            }
        }
    }
}
