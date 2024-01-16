using ExchangeRateUpdater.Mappers;
using ExchangeRateUpdater.Services;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currentDate = DateTime.UtcNow;
            var exchangeRateList = new List<ExchangeRate>();

            try
            {
                var rawDataCurrency = HttpClientService.GetDataFromCzBankFrequentCurrency(currentDate);
                if (!string.IsNullOrEmpty(rawDataCurrency))
                {
                    exchangeRateList.AddRange(ExchangeRateMapper.MapExchangeRateListFromString(currencies, rawDataCurrency));
                }

                var rawDataOtherCurrency = HttpClientService.GetDataFromCzBankOtherCurrency(currentDate);
                if (!string.IsNullOrEmpty(rawDataOtherCurrency))
                {
                    exchangeRateList.AddRange(ExchangeRateMapper.MapExchangeRateListFromString(currencies, rawDataOtherCurrency));
                }

                return exchangeRateList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ExchangeRateUpdater.GetExchangeRates\n" + ex.Message);
                return exchangeRateList;
            }
        }
    }
}
