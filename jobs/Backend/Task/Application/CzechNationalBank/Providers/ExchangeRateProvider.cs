using Application.Common.Models;
using Application.Common.Validations;
using Application.CzechNationalBank.ApiClient;
using Application.CzechNationalBank.Mappings;

using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Application.CzechNationalBank.Providers
{
    public class ExchangeRateProvider(ICNBClient cnbClient, ILogger<ExchangeRateProvider> logger, CurrencyValidator currencyValidator,
        ICNBExchangeRateMappingService mappingService) : IExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // validate - as console app has an invalid currency input but still expects results we will just log for now
            var validationResults = currencies.Select(c => currencyValidator.Validate(c));
            if (validationResults.Any(vr => !vr.IsValid))
            {
                var errorSummary = string.Join(", ", validationResults.SelectMany(vr => vr.Errors).Select(e => e.ErrorMessage));
                logger.LogWarning($"Validation Error on Currency inputs: {errorSummary}");
            }

            // depending on API reliability, load etc we might want to consider caching this in Redis or a DB, or in memory if app service
            var cnbRates = await cnbClient.GetExRateDailies();

            if (cnbRates == null || cnbRates.Rates.Count() == 0)
            {
                logger.LogError("No currency rates retrieved from CNB Api");
                return Enumerable.Empty<ExchangeRate>();
            }

            var filteredResults = cnbRates.Rates
                .Where(r => currencies.Any(c => c.Code == r.CurrencyCode));

            return mappingService.ConvertToExchangeRates(filteredResults);
        }
    }
}
