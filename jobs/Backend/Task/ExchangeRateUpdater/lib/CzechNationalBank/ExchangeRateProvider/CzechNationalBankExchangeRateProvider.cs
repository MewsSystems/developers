using ExchangeRateUpdater.Lib.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        public Currency ProvidedCurrencyConversion { get; private set; }
        public IExchangeRateProviderSettings Settings { get; }

        private readonly IExchangeRatesParallelHttpClient _exchangeRateClient;

        public CzechNationalBankExchangeRateProvider(
            IExchangeRateProviderSettings settings,
            IExchangeRatesParallelHttpClient exchangeRateClient
            )
        {
            ProvidedCurrencyConversion = new Currency("CZK");
            Settings = settings;
            _exchangeRateClient = exchangeRateClient;
        }

        /// <summary>
        /// Get Exchange Rates From Czech National Bank
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns>
        /// list of exchange rates
        /// </returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(
            IEnumerable<Currency> currencies
            )
        {
            var exchangeRates = _exchangeRateClient.GetExchangeRatesAsync(currencies).Result;

            var result = MapToExchangeRates(new Currency("CZK"), currencies, exchangeRates)
               .Where(exchangeRate => exchangeRate.Value != 1) // filter out entries where no conversion is happening
               .ToList();

            return result;

        }




        public IEnumerable<ExchangeRate> MapToExchangeRates(
           Currency sourceCurrency,
           IEnumerable<Currency> currencies,
           IEnumerable<ProviderExchangeRate> exchangeRates
       )
        {
            var exchangeRatesLookup = exchangeRates
                .ToDictionary(k => k.Currency, v => v);

            return currencies
                    .Where(
                        targetCurrency =>
                            targetCurrency != sourceCurrency && exchangeRatesLookup.ContainsKey(targetCurrency)
                     )
                    .Where(
                        targetCurrency =>
                            exchangeRatesLookup[targetCurrency].BaseRate > 0 // cant divide by zero...
                            && exchangeRatesLookup[targetCurrency].MostRecentRate.HasValue
                            && exchangeRatesLookup[targetCurrency].MostRecentRate.Value > 0 // cant divide by zero...
                     )
                    .SelectMany(targetCurrency => CreateExchangeRate(sourceCurrency, targetCurrency, exchangeRatesLookup))
                    .ToList();
        }

        /// <summary>
        /// Create a Common ExchangeRate Object using, the source and targets values provided in the exchange rate lookup
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="targetCurrency"></param>
        /// <param name="exchangeRatesLookup"></param>
        /// <returns></returns>
        public ExchangeRate[] CreateExchangeRate(
            Currency sourceCurrency,
            Currency targetCurrency,
            Dictionary<Currency, ProviderExchangeRate> exchangeRatesLookup
        )
        {
            var targetExchangeRate = exchangeRatesLookup[targetCurrency];

            // get the reciprocal of the base rate
            var aToBValue = targetExchangeRate.BaseRate / targetExchangeRate.MostRecentRate.Value;

            var aToB = new ExchangeRate(
                    sourceCurrency,
                    targetCurrency,
                    Math.Round(aToBValue, Settings.Precision)
                );

            var bToA = new ExchangeRate(
                    targetCurrency,
                    sourceCurrency,
                    targetExchangeRate.MostRecentRate.Value
                );

            return new[] { aToB, bToA };
        }
    }
}
