using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Services
{
    /// <summary>
    /// Provides functionality to retrieve exchange rates.
    /// </summary>
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRatesRepository _exchangeRatesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateProvider"/> class.
        /// </summary>
        /// <param name="exchangeRatesRepository">The repository for retrieving exchange rates.</param>
        public ExchangeRateProvider(IExchangeRatesRepository exchangeRatesRepository)
        {
            _exchangeRatesRepository = exchangeRatesRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateOnly? date, IEnumerable<Currency> currencies)
        {
            var exchangeRates = await _exchangeRatesRepository.GetExchangeRatesAsync(date);

            if (currencies != null && currencies.Any())
            {
                exchangeRates = exchangeRates
                    .Where(rates => currencies.Any(currency => currency == rates.SourceCurrency));
            }

            return exchangeRates;
        }
    }
}
