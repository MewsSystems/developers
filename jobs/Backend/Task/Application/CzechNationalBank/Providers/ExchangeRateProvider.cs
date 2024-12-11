using Application.Common.Models;
using Application.Common.Services;
using Application.Common.Validations;
using Application.CzechNationalBank.ApiClient;
using Application.CzechNationalBank.Mappings;

using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Application.CzechNationalBank.Providers
{
    public class ExchangeRateProvider(
        ICNCApiClient _cnbClient, 
        ILogger<ExchangeRateProvider> _logger, 
        ICurrenciesValidationService _currenciesValidationService,
        ICNBExchangeRateMappingService _mappingService) : IExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            _currenciesValidationService.ValidateAndLogWarning(currencies);

            // depending on API reliability, response times, load etc we might want to consider caching this in Redis, or in memory if app service
            var cnbRates = await _cnbClient.GetExRateDailies();

            if (cnbRates == null || cnbRates.Rates.Count() == 0)
            {
                _logger.LogError("No currency rates retrieved from CNB Api");
                return Enumerable.Empty<ExchangeRate>();
            }

            var filteredResults = cnbRates.Rates
                .Where(r => currencies.Any(c => c.Code == r.CurrencyCode));

            return _mappingService.ConvertToExchangeRates(filteredResults);
        }
    }
}
