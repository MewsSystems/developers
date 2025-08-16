using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Options;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ExchangeOptions _exchangeOptions;
        private readonly ICnbHttpClient _cnbHttpClient;
        private readonly ILogger<ExchangeRateProvider> _logger;

        private readonly Currency _targetCurrency;

        public ExchangeRateProvider(
            ExchangeOptions exchangeOptions,
            ICnbHttpClient cnbHttpClient,
            ILogger<ExchangeRateProvider> logger)
        {
            _exchangeOptions = exchangeOptions;
            _cnbHttpClient = cnbHttpClient;
            _logger = logger;

            _targetCurrency = new Currency(_exchangeOptions.TargetCurrencyCode);
        }

        // TODO: May be async for database call
        public HashSet<string> GetActualCurrencyCodes()
        {
            // Trying to avoid hardcoding of currency values, it could be database or configuration or arguments for console tool.
            // If the currency model will have some extra properties - map to HashSet<Currency>.
            return _exchangeOptions.RequiredCurrencyCodes;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<List<ExchangeRate>> GetExchangeRates(HashSet<string> requiredCurrencyCodes,
            CancellationToken cancellationToken)
        {
            if (requiredCurrencyCodes is null || requiredCurrencyCodes.Count == 0)
            {
                _logger.LogInformation("No currencies was provided to retrieve exchange rates");
                return new List<ExchangeRate>();
            }

            // It would be great to have caching to read from it and invalidate keys for outdated information,
            // but the rates are staying valid for 24 hours and it's not so important if it's one time operation in 24 hours

            var allRates = await _cnbHttpClient.GetDailyExchangeRates(cancellationToken);
            var requiredRates = MapExchangeRates(requiredCurrencyCodes, allRates).ToList();
            _logger.LogInformation("Successfully retrieved {ratesCount} exchange rates", requiredRates.Count);

            return requiredRates;
        }

        private IEnumerable<ExchangeRate> MapExchangeRates(
            HashSet<string> currencyCodes,
            IReadOnlyCollection<CnbExchangeRate> rates)
        {
            foreach (var rate in rates)
            {
                if (!currencyCodes.TryGetValue(rate.CurrencyCode, out var requiredCode))
                {
                    continue;
                }

                yield return new ExchangeRate(new Currency(requiredCode), _targetCurrency, rate.Rate / rate.Amount);
            }
        }
    }
}