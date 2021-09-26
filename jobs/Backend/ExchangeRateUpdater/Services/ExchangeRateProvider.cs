using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeRateUpdater.ViewModels;
using ExchangeRateUpdater.CnbProxy.Services;
using ExchangeRateUpdater.Utilities.Logging;
using ExchangeRateUpdater.Utilities.Extensions;

namespace ExchangeRateUpdater.Services
{
    class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICnbExchangeRatesService _cnbExchangeRatesService;
        private readonly IMapper _mapper;
        private readonly IAppLogger _logger;

        public ExchangeRateProvider(ICnbExchangeRatesService cnbExchangeRatesService, IMapper mapper, IAppLogger logger)
        {
            _cnbExchangeRatesService = cnbExchangeRatesService;
            _mapper = mapper;
            _logger = logger;
        }
         
        public async Task RetrieveExchangeRatesAsync()
        {
            try
            {
                var rates = await GetExchangeRatesAsync(Currencies.All);

                if (rates == null || !rates.Any())
                {
                    _logger.Warn("Could not retrieve exchange rates");

                    return;
                }

                _logger.Info($"Successfully retrieved {rates.Count()} exchange rates:");

                rates.ForEach(rate => _logger.Info(rate.ToString()));
            }
            catch (Exception e)
            {
                _logger.Error($"Could not retrieve exchange rates: '{e.Message}'");
            }
        }
         
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var cnbExchangeRates = await _cnbExchangeRatesService.GetAsync();

            // TODO: Consider decoupling the AutoMapper for better testability
            
            var exchangeRates = _mapper.Map<List<ExchangeRate>>(cnbExchangeRates);

            // TODO: It might be a business requirement that doesn't match to the presentation layer

            return exchangeRates.Where(x => currencies.Any(currency => currency.Equals(x.SourceCurrency)));
        }


    }
}
