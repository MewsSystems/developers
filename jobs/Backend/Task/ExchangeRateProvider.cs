using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {


        private DataService _dataService;
        public ExchangeRateProvider(DataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {

         // Gets combined data from Central Bank and Other currencies data from czech national bank

           var data =  await _dataService.GetExchangeRateData(currencies);

          // converts from ExchangeRateModel to ExchangeRate

           var exchangeRates = ConvertToExchangeRate(data);

           return exchangeRates;

        }



        private static IEnumerable<ExchangeRate> ConvertToExchangeRate(List<ExchangeRateModel> exchangeRateData)
        {
           List<ExchangeRate> currencies = new List<ExchangeRate>();

           foreach (var rate in exchangeRateData)
            {
                decimal value = GetRate(rate);
                var exchangeRate = new ExchangeRate(new Currency(rate.CurrencyCode), new Currency("CZK"),value);
                
                currencies.Add(exchangeRate);
            }

           return currencies;
        }

        // Get rate of CZK with 1 of that specific currency
        private static decimal GetRate(ExchangeRateModel rate)
        {
            decimal.TryParse(rate.Amount, out decimal AmountNumber);

            decimal.TryParse(rate.Rate, out decimal RateNumber);

            return RateNumber / AmountNumber;
        }
    }
}
