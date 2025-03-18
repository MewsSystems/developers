using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    public static class CnbExchangeRateMapper
    {
        /// <summary>
        /// Maps the CNB response DTO to a list of ExchangeRate objects.
        /// </summary>
        /// <param name="response">The deserialized CNB response.</param>
        /// <param name="requestedCurrencies">The requested currencies.</param>
        /// <returns>A list of ExchangeRate objects.</returns>
        public static IEnumerable<ExchangeRate> Map(CnbExchangeRateResponse response, IEnumerable<Currency> requestedCurrencies)
        {
            var requestedCodes = new HashSet<string>(requestedCurrencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

            return response.Rates
                .Where(rate => requestedCodes.Contains(rate.CurrencyCode))
                .Select(rate =>
                {
                    decimal normalizedRate = rate.Rate / rate.Amount;
                    return new ExchangeRate(new Currency("CZK"), new Currency(rate.CurrencyCode), normalizedRate); // TODO: hardcode CZK?
                });
        }
    }
}
