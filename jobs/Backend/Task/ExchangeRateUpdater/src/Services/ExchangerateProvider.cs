using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    /// <summary>
    /// Provides a mechanism for retrieving exchange rates for a given set of currencies.
    /// This is the default implementation of <see cref="IExchangeRateProvider"/> and relies on an <see cref="ICnbExchangeRateRepository"/> to fetch the data.
    /// </summary>
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICnbExchangeRateRepository _cnbExchangeRateRepository;
        private readonly ILogger<ExchangeRateProvider> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateProvider"/> class.
        /// </summary>
        /// <param name="exchangeRateRepository">The repository for accessing CNB exchange rate data.</param>
        /// <param name="logger">The logger for logging information and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exchangeRateRepository"/> or <paramref name="logger"/> is null.</exception>
        public ExchangeRateProvider(
            ICnbExchangeRateRepository exchangeRateRepository,
            ILogger<ExchangeRateProvider> logger)
        {
            _cnbExchangeRateRepository = exchangeRateRepository ?? throw new ArgumentNullException(nameof(exchangeRateRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        
        /// <summary>
        /// Asynchronously retrieves exchange rates for a specified list of currencies.
        /// </summary>
        /// <param name="currencies">An enumerable collection of <see cref="Currency"/> objects for which to retrieve exchange rates.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="ExchangeRate"/>
        /// for the requested currencies. Returns an empty collection if the input <paramref name="currencies"/> is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="currencies"/> collection is null.</exception>
        /// <exception cref="Exception">Rethrows exceptions from the underlying repository on failure.</exception>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (currencies == null)
                throw new ArgumentNullException(nameof(currencies));

            var currencyList = currencies.ToList();
            if (currencyList.Count == 0)
            {
                _logger.LogWarning("No currencies provided for exchange rate lookup");
                return Enumerable.Empty<ExchangeRate>();
            }

            try
            {
                _logger.LogInformation("Retrieving exchange rates for {Count} currencies", currencyList.Count);

                var rates = await _cnbExchangeRateRepository.GetSpecificExchangeRatesAsync(currencyList);
                var ratesList = rates.ToList();

                _logger.LogInformation("Successfully retrieved {Count} exchange rates", ratesList.Count);
                return ratesList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve exchange rates");
                throw;
            }
        }
    }
}