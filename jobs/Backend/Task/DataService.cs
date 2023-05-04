using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class DataService

    {
        private const string centralBankUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        private const string OtherCurrenciesUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";

        private DataRepository _dataRepository;
        
        public DataService(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        
        public virtual async Task<List<ExchangeRateModel>> GetExchangeRateData(IEnumerable<Currency> currencies)
        {

            var exchangeRateData = new List<ExchangeRateModel>();

            // retrieve list of currency codes

            var currencyCodes = GetCurrencyCodes(currencies);

            // retrieve Data from https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing

            var centralBankData = await _dataRepository.GetData(currencyCodes, centralBankUrl);

            exchangeRateData.AddRange(centralBankData);

            // retrieve data from https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies

            var otherCurrenciesData = await _dataRepository.GetData(currencyCodes, OtherCurrenciesUrl);

            exchangeRateData.AddRange(otherCurrenciesData);

            return exchangeRateData;
        }


        private static List<string> GetCurrencyCodes(IEnumerable<Currency> currencies)
        {
            var currencyCodes = currencies.Select(x => x.Code).ToList();

            return currencyCodes;
        }


    }
}
