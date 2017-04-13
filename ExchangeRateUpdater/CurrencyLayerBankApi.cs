using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class CurrencyLayerBankApi : IBankApi
    {
        private string _apiKey = ConfigurationManager.AppSettings["ApiKey"];
        private string _url;

        public CurrencyLayerBankApi(IEnumerable<Currency> currencies, Currency source)
        {
            ///////////////////////////////////////////
            // api from https://currencylayer.com/ ////
            ///////////////////////////////////////////
            _url = $"http://apilayer.net/api/live?access_key={_apiKey}&currencies={FormatCurrencies(currencies)}&format=1";
        }

        private string FormatCurrencies(IEnumerable<Currency> currencies)
        {
            return string.Join(",", currencies.Select(c => c.Code));
        }

        private CurrencyResponse DesirializeResponse()
        {
            var jsonResponse = new WebClient().DownloadString(_url);
            return JsonConvert.DeserializeObject<CurrencyResponse>(jsonResponse);
        }

        public IEnumerable<ExchangeRate> GetValues(IEnumerable<Currency> currencies)
        {
            var rates = new List<ExchangeRate>();
            var convertedResponse = DesirializeResponse();

            if (convertedResponse == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var currency in currencies)
            {
                var dic = convertedResponse.Quotes.FirstOrDefault(x => x.Key.Contains(currency.Code));

                if (dic.Key == "" || dic.Value == 0) continue;          // validation for not supported currency


                var src = new Currency("USD");                          // in free licence USD is the default base currency
                var targ = new Currency(dic.Key.Replace("USD", ""));    // api return not fancy value
                var val = dic.Value;

                rates.Add(new ExchangeRate(src, targ, val));
            }

            return rates;
        }
    }
}
