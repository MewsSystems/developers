using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CnbProvider
{
    /// <summary>
    /// Provides FX rates for CZK from Czech National Bank site www.cnb.cz
    /// </summary>
    class CnbExchangeRatesProvider : ICustomExchangeRatesProvider
    {
        public static readonly Currency ProviderCurrency = new Currency("CZK");

        private readonly ICnbFxRatesWebLoader _cnbFxRatesWebLoader;

        private readonly List<CnbFxRatesSource> _cnbRatesSources = new List<CnbFxRatesSource>
        {
            new CnbFxMainCurrenciesSource(),
            new CnbFxOtherCurrenciesSource()
        };

        public CnbExchangeRatesProvider(ICnbFxRatesWebLoader cnbFxRatesWebLoader)
        {
            _cnbFxRatesWebLoader = cnbFxRatesWebLoader ?? throw new ArgumentNullException(nameof(cnbFxRatesWebLoader));
        }

        CnbExchangeRatesProvider(ICnbFxRatesWebLoader cnbFxRatesWebLoader, List<CnbFxRatesSource> cnbRatesSources)
        {
            _cnbRatesSources = cnbRatesSources ?? throw new ArgumentNullException(nameof(cnbRatesSources));
            _cnbFxRatesWebLoader = cnbFxRatesWebLoader ?? throw new ArgumentNullException(nameof(cnbFxRatesWebLoader));
        }

        public async Task<(IEnumerable<ExchangeRate> rates, bool ratesWhereUpdated)> GetAllRatesAsync()
        {
            bool ratesWhereUpdated = false;

            var syncSources = _cnbRatesSources.Where(x => x.NewRatesAvailable()).Select(_cnbFxRatesWebLoader.TryGetLatestRates).ToList();
            if (syncSources.Any())
            {
                var result = await Task.WhenAll(syncSources).ConfigureAwait(false);
                ratesWhereUpdated = result.Any(updated => updated);
            }
            
            return (_cnbRatesSources.SelectMany(rateSource => rateSource.Current.LoadedRates), ratesWhereUpdated);
        }
    }
}