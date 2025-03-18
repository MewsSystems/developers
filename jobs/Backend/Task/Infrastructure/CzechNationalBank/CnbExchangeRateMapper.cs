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
        /// <returns>A list of ExchangeRate objects.</returns>
        public static IEnumerable<ExchangeRate> Map(CnbExchangeRateResponse response)
        {
            return response.Rates
                .Select(rate =>
                {
                    decimal normalizedRate = rate.Rate / rate.Amount;
                    DateTime validFor = DateTime.Parse(rate.ValidFor).Date;
                    return new ExchangeRate(new Currency(rate.CurrencyCode), new Currency("CZK"), normalizedRate);
                });
        }
    }
}
