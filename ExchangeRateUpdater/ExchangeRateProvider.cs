using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            DateTime _today = DateTime.Now;
           
            // Considering that target currency is CZK
            Currency targetCurrency = new Currency("CZK");


            List<ExchangeRate> _rates = new List<ExchangeRate>();
            List<Currency> _currencyList = new List<Currency>();
            List<String> _currencyCodes = new List<string>();
            _currencyList = currencies.ToList<Currency>();
            
            foreach (Currency _currency in _currencyList)           // Fill source currencies in list
            {
                _currencyCodes.Add(_currency.Code);

            }
            // This allows us to retreive daily exchange rates everytime instead of manually changing the link.
            System.Net.WebRequest _request = System.Net.WebRequest.Create("https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date=" + _today.ToString("dd.mm.yyyy"));

            System.Net.WebResponse _response;
            _response = _request.GetResponse();         // Contains data from web
            if (_response.ContentLength > 0)
            {
                Stream _dataStream = _response.GetResponseStream();
                StreamReader _reader;
                _reader = new StreamReader(_dataStream);
                string _source = _reader.ReadToEnd();
                _reader.DiscardBufferedData();
                string[] _dataCaptured = _source.Split('\n');
                foreach (var _data in _dataCaptured)
                {
                    if (_data.Contains('|'))
                    {                    
                        String _currency = _data.Split('|')[3];             // Read Code from data retreived from webpage
                        int index = _currencyCodes.IndexOf(_currency);      // Check codes against codes from source
                        if (index >= 0)
                        {
                            Decimal _value = Convert.ToDecimal(_data.Split('|')[4]);
                            Currency _cur = new Currency(_currency);
                            ExchangeRate _rate = new ExchangeRate(_cur, targetCurrency, _value);
                            _rates.Add(_rate);
                        }
                    }

                }

            }

            return _rates;
            
           // return Enumerable.Empty<ExchangeRate>();
        }
    }
}
