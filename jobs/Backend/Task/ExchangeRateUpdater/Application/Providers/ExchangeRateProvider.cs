using ExchangeRateUpdater.Application.DTOs;
using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Infrastructure.Sources;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Providers
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly IExchangeRateSource _source;
        public ExchangeRateProvider(ILogger<ExchangeRateProvider> logger, IExchangeRateSource source)
        {
            _logger = logger;
            _source = source;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            IEnumerable<ExchangeRateDTO> rates = await _source.GetDailyExchangeRates();
            return ProcessExchangeRates(currencies, rates);
        }

        /// <summary>
        /// Retrieves exchange rates for the specified currencies on a given date.
        /// </summary>
        /// <param name="currencies">Currencies to retrieve exchange rates for.</param>
        /// <param name="date">Date in yyyy-MM-dd format. Assumed to be validated.</param>
        /// <returns>
        /// A collection of validated and normalized ExchangeRate objects for the given date.
        /// </returns>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesFromDay(IEnumerable<Currency> currencies, string date)
        {
            IEnumerable<ExchangeRateDTO> rates = await _source.GetDailyExchangeRates(date);
            return ProcessExchangeRates(currencies, rates);
        }

        /// <summary>
        /// Compares exchange rates between two different dates and calculates
        /// value and percentage differences.
        /// </summary>
        /// <param name="currencies">Currencies to compare.</param>
        /// <param name="dateA">First comparison date (yyyy-MM-dd).</param>
        /// <param name="dateB">Second comparison date (yyyy-MM-dd).</param>
        /// <returns>
        /// A collection of ExchangeRateDifference representing the difference
        /// in rates between the two dates.
        /// </returns>
        public async Task<IEnumerable<ExchangeRateDifference>> CompareExchangeRatesBetweenDates(IEnumerable<Currency> currencies, string dateA, string dateB)
        {
            IEnumerable<ExchangeRateDTO> ratesA = await _source.GetDailyExchangeRates(dateA);
            IEnumerable<ExchangeRateDTO> ratesB = await _source.GetDailyExchangeRates(dateB);
            return CompareExchangeRates(currencies, ratesA, ratesB);
        }

        /// <summary>
        /// Validates and normalizes raw ExchangeRateDTO objects into domain 
        /// ExchangeRate objects that are included in currencies.
        /// </summary>
        /// <param name="currencies">Currencies to include.</param>
        /// <param name="rates">Raw exchange rate DTOs retrieved from the source.</param>
        /// <returns>
        /// A collection of valid and normalized ExchangeRate objects.
        /// </returns>
        private IEnumerable<ExchangeRate> ProcessExchangeRates(IEnumerable<Currency> currencies, IEnumerable<ExchangeRateDTO> rates)
        {
            List<ExchangeRate> validRates = new List<ExchangeRate>();
            if (rates == null || !rates.Any())
            {
                _logger.LogWarning("No exchange rates retrieved from source.");
                return validRates;
            }

            HashSet<string> currencyCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

            //Need foreach to be able to log skipped rates
            foreach (ExchangeRateDTO exchangeRateDTO in rates)
            {
                if (exchangeRateDTO == null)
                {
                    continue;
                }
                if (!currencyCodes.Contains(exchangeRateDTO.CurrencyCode))
                {
                    continue;
                }
                if (exchangeRateDTO.CurrencyCode == SupportedCurrencies.DefaultBaseCurrency.Code)
                {
                    _logger.LogInformation($"Skipped exchange rate for base currency {exchangeRateDTO.CurrencyCode}");
                    continue;
                }
                if (!IsExchangeRateDTOValid(exchangeRateDTO))
                {
                    _logger.LogWarning($"Skipped invalid exchange rate for Currency={exchangeRateDTO.CurrencyCode} with Amount={exchangeRateDTO.Amount}, Rate={exchangeRateDTO.Rate}");
                    continue;
                }
                validRates.Add(ExchangeRateMapDTOToModel(exchangeRateDTO));
            }

            return validRates;
        }

        /// <summary>
        /// Compares exchange rates between two dates and calculates value and percentage differences.
        /// Dates are expected to be different and validated.
        /// </summary>
        /// <param name="currencies">Currencies to compare.</param>
        /// <param name="ratesA">Exchange rates for the first date.</param>
        /// <param name="ratesB">Exchange rates for the second date.</param>
        /// <returns>
        /// A list of ExchangeRateDifference objects.
        /// </returns>
        private List<ExchangeRateDifference> CompareExchangeRates(IEnumerable<Currency> currencies, IEnumerable<ExchangeRateDTO> ratesA, IEnumerable<ExchangeRateDTO> ratesB)
        {
            List<ExchangeRateDifference> diffs = new List<ExchangeRateDifference>();

            if (ratesA == null || ratesB == null)
            {
                _logger.LogWarning("One or both exchange rate sources are null.");
                return diffs;
            }

            Dictionary<string, ExchangeRateDTO> ratesADict = ratesA
                .Where(r => r != null)
                .GroupBy(r => r.CurrencyCode)
                .ToDictionary(g => g.Key, g => g.First());

            Dictionary<string, ExchangeRateDTO> ratesBDict = ratesB
                .Where(r => r != null)
                .GroupBy(r => r.CurrencyCode)
                .ToDictionary(g => g.Key, g => g.First());

            foreach (Currency currency in currencies)
            {
                if (currency.Code == SupportedCurrencies.DefaultBaseCurrency.Code)
                {
                    _logger.LogInformation($"Skipped base currency {currency.Code}");
                    continue;
                }

                if (!ratesADict.TryGetValue(currency.Code, out var rateA))
                {
                    _logger.LogWarning($"Rate for {currency.Code} not found on first date.");
                    continue;
                }

                if (!ratesBDict.TryGetValue(currency.Code, out var rateB))
                {
                    _logger.LogWarning($"Rate for {currency.Code} not found in second date.");
                    continue;
                }

                if (!IsExchangeRateDTOValid(rateA))
                {
                    _logger.LogWarning($"Skipped invalid rate for {currency.Code} on first date.");
                    continue;
                }

                if (!IsExchangeRateDTOValid(rateB))
                {
                    _logger.LogWarning($"Skipped invalid rate for {currency.Code} on second date.");
                    continue;
                }

                decimal normalizedRateA = rateA.Amount > 1 ? rateA.Rate / rateA.Amount : rateA.Rate;
                decimal normalizedRateB = rateB.Amount > 1 ? rateB.Rate / rateB.Amount: rateB.Rate;
                decimal diff = normalizedRateB - normalizedRateA;
                decimal percentageDiff = normalizedRateA == 0 ? 0 : Math.Round((diff / normalizedRateA) * 100, 2);

                diffs.Add(new ExchangeRateDifference(SupportedCurrencies.DefaultBaseCurrency, currency, normalizedRateA, normalizedRateB, diff, percentageDiff));
            }

            return diffs;
        }

        /// <summary>
        /// Validates that an ExchangeRateDTO contains usable data.
        /// </summary>
        private static bool IsExchangeRateDTOValid(ExchangeRateDTO exchangeRateDTO)
        {
            return !string.IsNullOrWhiteSpace(exchangeRateDTO.CurrencyCode)
                && exchangeRateDTO.Rate > 0
                && exchangeRateDTO.Amount > 0;
        }

        /// <summary>
        /// Maps a valid ExchangeRateDTO to a normalized domain ExchangeRate.
        /// </summary>
        private static ExchangeRate ExchangeRateMapDTOToModel(ExchangeRateDTO exchangeRateDTO)
        {
            //Nomalize rate incase amount given is greater than 1
            decimal normalizedRate = exchangeRateDTO.Amount > 1 ? exchangeRateDTO.Rate / exchangeRateDTO.Amount : exchangeRateDTO.Rate;
            return new ExchangeRate(SupportedCurrencies.DefaultBaseCurrency, new Currency(exchangeRateDTO.CurrencyCode), normalizedRate);
        }
    }
}
