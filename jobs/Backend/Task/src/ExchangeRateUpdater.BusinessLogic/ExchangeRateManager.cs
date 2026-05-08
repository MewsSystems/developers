using ExchangeRateUpdater.BusinessLogic.Models;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.BusinessLogic
{
    /// <summary>
    /// Business logic service that manages and retrieves exchange rates.
    /// Accepts a provider via dependency injection (wired at composition root in Console/API).
    /// </summary>
    public class ExchangeRateManager : IExchangeRateManager
    {
        private readonly Func<IEnumerable<Currency>, IEnumerable<ExchangeRate>> _getExchangeRates;

        /// <summary>
        /// Constructor accepting a function that retrieves exchange rates from the data layer.
        /// This avoids circular dependency between BusinessLogic and Data projects.
        /// </summary>
        public ExchangeRateManager(Func<IEnumerable<Currency>, IEnumerable<ExchangeRate>> getExchangeRates)
        {
            _getExchangeRates = getExchangeRates ?? throw new ArgumentNullException(nameof(getExchangeRates));
        }

        /// <summary>
        /// Retrieves exchange rates for the specified currencies from the data layer provider.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return _getExchangeRates(currencies);
        }
    }
}
