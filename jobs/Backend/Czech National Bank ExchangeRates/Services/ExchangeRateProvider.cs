using Czech_National_Bank_ExchangeRates.Models;
using Czech_National_Bank_ExchangeRates.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICNBExchangeRateRepo _cnbExchangeRateRepo;
        public ExchangeRateProvider(ICNBExchangeRateRepo cnbExchangeRateRepo)
        {
            _cnbExchangeRateRepo = cnbExchangeRateRepo;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<ExchangeRates> GetExchangeRatesByDate(string dateString)
        {
            var exchangeRates = await _cnbExchangeRateRepo.GetExhangeRateData(dateString);
            return exchangeRates;
        }
    }
}
