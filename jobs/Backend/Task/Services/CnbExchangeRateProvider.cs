using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public sealed class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private const string TargetCurrencyCode = "CZK";

        private readonly ILogger<CnbExchangeRateProvider> _logger;
        private readonly ICnbApiClient _cnbApiClient;

        public CnbExchangeRateProvider(
            HttpClient httpClient,
            ILogger<CnbExchangeRateProvider> logger,
            ICnbApiClient cnbApiClient)
        {
            _logger = logger;
            _cnbApiClient = cnbApiClient;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            ArgumentNullException.ThrowIfNull(currencies);

            var currencyCodesToSearch = currencies
                .Select(c => c.Code)
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            currencyCodesToSearch.Remove(TargetCurrencyCode);
            if (currencyCodesToSearch.Count == 0)
                return Enumerable.Empty<ExchangeRate>();

            var foundRates = new Dictionary<string, ExchangeRate>(StringComparer.OrdinalIgnoreCase);

            var dailyRates = await _cnbApiClient.GetDailyRatesAsync();
            ProcessRates(dailyRates, currencyCodesToSearch, foundRates);

            if (foundRates.Count < currencyCodesToSearch.Count)
            {
                var notFoundCodes = currencyCodesToSearch.Except(foundRates.Keys);
                _logger.LogWarning("Could not find rates for the following currencies from the daily source: {NotFoundCodes}", string.Join(", ", notFoundCodes));
            }

            return foundRates.Values;
        }

        private static void ProcessRates(IEnumerable<CnbApiRateDto> rates, ISet<string> codesToFind, Dictionary<string, ExchangeRate> foundRates)
        {
            if (rates is null)
                return;

            var targetCurrency = new Currency(TargetCurrencyCode);
            foreach (var rateDto in rates)
            {
                if (string.IsNullOrWhiteSpace(rateDto.CurrencyCode)) continue;

                if (codesToFind.Contains(rateDto.CurrencyCode) && !foundRates.ContainsKey(rateDto.CurrencyCode))
                {
                    var valuePerUnit = rateDto.Amount > 0 ? rateDto.Rate / rateDto.Amount : 0;
                    foundRates[rateDto.CurrencyCode] = new ExchangeRate(new Currency(rateDto.CurrencyCode), targetCurrency, valuePerUnit);
                }
            }
        }
    }
}