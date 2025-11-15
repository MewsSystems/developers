using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application.Services
{
    /// <summary>
    /// Application service to orchestrate exchange rate retrieval.
    /// Uses one or more IExchangeRateProvider instances.
    /// </summary>
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IEnumerable<IExchangeRateProvider> _providers;
        private readonly ILogger<ExchangeRateService> _logger;

        /// <summary>
        /// Initializes the service with one or more exchange rate providers.
        /// </summary>
        /// <param name="providers">Injected exchange rate providers.</param>
        public ExchangeRateService(IEnumerable<IExchangeRateProvider> providers, ILogger<ExchangeRateService> logger)
        {
            _providers = providers ?? throw new ArgumentNullException(nameof(providers));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves exchange rates for the requested currencies asynchronously.
        /// Only returns rates explicitly provided by the underlying providers.
        /// </summary>
        public Task<IEnumerable<ExchangeRate>> GetRatesAsync(IEnumerable<Currency> requestedCurrencies)
        {
            if (requestedCurrencies == null)
                throw new ArgumentNullException(nameof(requestedCurrencies));

            var results = new List<ExchangeRate>();

            foreach (var provider in _providers)
            {
                try
                {
                    var rates = provider.GetExchangeRates(requestedCurrencies);
                    if (rates != null)
                        results.AddRange(rates);
                }
                catch (Exception ex) { 
                    _logger.LogError(ex, "Exchange rate provider '{Provider}' failed.", provider.GetType().Name);
                    throw new InvalidOperationException(
                        $"Failed to retrieve exchange rates from provider '{provider.GetType().Name}'.", ex);
                }
            }
            var distinctRates = results
                .GroupBy(r => new { SourceCode= r.Source.Code, TargetCode = r.Target.Code })
                .Select(g => g.First());

            return Task.FromResult(distinctRates.AsEnumerable());
        }
    }
}
