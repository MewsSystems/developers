using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using services;
using ExchangeRateUpdater.responses;

namespace ExchangeRateUpdater.services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private const string CzechCurrenyCode = "CZK";
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            IEnumerable<ExchangeRateResponse> combinedRates = await GetRates();

            var exchangeRates = combinedRates
                .Where(rate => currencies.Any(currency => currency.Code == rate.CurrencyCode))
                .Select(rate => new ExchangeRate(
                    new Currency(rate.CurrencyCode),
                    new Currency(CzechCurrenyCode),
                    rate.Amount.ToString(),
                    rate.Rate
                ))
                .ToList();

            return exchangeRates;
        }

        private async Task<IEnumerable<ExchangeRateResponse>> GetRates()
        {
            var today = DateTime.UtcNow;
            var dailyRatesResponse = await CnbToolRequest.DailyRatesRequest(today).Execute();
            var dailyRates = dailyRatesResponse.ParseJsonBody<ExchangeRateListResponse>();

            var otherRatesResponse = await CnbToolRequest.OthersRatesRequest(today).Execute();
            var otherRates = otherRatesResponse.ParseJsonBody<ExchangeRateListResponse>();

            if (otherRates.Rates.Count == 0)
            {
                var oneMonthAgo = today.AddMonths(-1);
                otherRatesResponse = await CnbToolRequest.OthersRatesRequest(oneMonthAgo).Execute();
                otherRates = otherRatesResponse.ParseJsonBody<ExchangeRateListResponse>();
            }

            var combinedRates = dailyRates.Rates.Concat(otherRates.Rates);
            return combinedRates;
        }
    }
}
