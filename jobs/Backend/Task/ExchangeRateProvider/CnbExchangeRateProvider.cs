using ExchangeRateProvider.Constants;
using ExchangeRateProvider.Interfaces;
using ExchangeRateProvider.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateProvider
{
    /// <summary>
    /// Provides methods for retrieving exchange rates for specified currencies from the Czech National Bank (CNB) API.
    /// </summary>
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly string _targetCurrency = Currency.Czk;
        private readonly ICnbHttpClient _cnbHttpClient;

        public CnbExchangeRateProvider(ICnbHttpClient cnbHttpClient)
        {
            _cnbHttpClient = cnbHttpClient;
        }

        /// <summary>
        /// Retrieves exchange rates for specified currencies from the CNB API.
        /// </summary>
        /// <param name="currencies">An array of currency models representing the currencies for which exchange rates are requested.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The result contains an array of <see cref="ExchangeRateModel"/> objects
        /// representing the exchange rates between the specified currencies and the target currency.
        /// </returns>
        public async Task<ExchangeRateModel[]> GetExchangeRatesAsync(CurrencyModel[] currencies, CancellationToken cancellationToken = default)
        {
            var czkExchangeRates = await _cnbHttpClient.GetCzkExchangeRatesAsync(DateTime.UtcNow.Date, cancellationToken: cancellationToken);
            if (czkExchangeRates is null || czkExchangeRates.Rates is null)
                return [];

            return czkExchangeRates.Rates
                .Where(rate => currencies.Any(c => c.Code == rate.CurrencyCode))
                .OrderBy(rate => rate.CurrencyCode)
                .Select(rate => new ExchangeRateModel
                {
                    SourceCurrency = new CurrencyModel(rate.CurrencyCode),
                    TargetCurrency = new CurrencyModel(_targetCurrency),
                    Value = rate.Rate
                })
                .ToArray();
        }
    }
}