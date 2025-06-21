using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    /// <summary>
    /// Default implementation of the exchange rate provider.
    /// </summary>
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICnbExchangeRateRepository _cnbExchangeRateRepository;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(
            ICnbExchangeRateRepository exchangeRateRepository,
            ILogger<ExchangeRateProvider> logger)
        {
            _cnbExchangeRateRepository = exchangeRateRepository ?? throw new ArgumentNullException(nameof(exchangeRateRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
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
