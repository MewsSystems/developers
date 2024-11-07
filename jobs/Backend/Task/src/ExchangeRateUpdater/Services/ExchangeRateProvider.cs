using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Mappers;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{

    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IFxRateService _fxRateService;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(IFxRateService fxRateService, ILogger<ExchangeRateProvider> logger)
        {
            _fxRateService = fxRateService;
            _logger = logger;
        }       

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
            IEnumerable<Currency> currencies,
            DateTime? date,
            string language = "EN",
            CancellationToken cancellationToken = default)
        {
            if (!currencies.Any())
            {
                _logger.LogWarning("The requested currencies list was empty. No exchange rates will be extracted.");
                return Enumerable.Empty<ExchangeRate>();
            }
            if (language is not "EN" or "CZ")
            {
                _logger.LogInformation("The requested language was invalid. Reverting to default value EN.");
                language = "EN";
            }

            var exchangeRatesDate = date ?? DateTime.UtcNow;

            var allApiFxRates = await _fxRateService.GetFxRatesAsync(exchangeRatesDate, language, cancellationToken);

            var currencyCodes = currencies.Select(c=>c.Code).ToHashSet();
            var requestedRates = allApiFxRates.Where(fx => currencyCodes.Contains(fx.CurrencyCode));
            var exchangeRates = ExchangeRateMapper.MapApiFxRatesToExchangeRates(requestedRates);

            return exchangeRates;
        }
    }
}
