using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string serviceURL = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string defaultCurrency = "CZK";
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// 
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                List<ExchangeRate> rates = new List<ExchangeRate>();
                string exchangeRateString = (new WebClient()).DownloadString(serviceURL);
                if (string.IsNullOrEmpty(exchangeRateString))
                {
                    return new List<ExchangeRate>();
                }
                List<string> lines = exchangeRateString.TrimEnd().Split('\n').ToList();
                //Remove the header of the lines.
                lines.RemoveAt(0);
                lines.RemoveAt(0);
                foreach (var item in lines)
                {
                    rates.Add(ConvertLineToExchangeRate(item));
                }
                return filterRates(rates, currencies);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
        }
        /// <summary>
        /// The following function will convert each string line to ExchangeRate.
        /// </summary>
        /// <param name="line">line of rate information</param>
        /// <returns></returns>
        public ExchangeRate ConvertLineToExchangeRate(string line)
        {
            try
            {
                List<string> rate = line.Split('|').ToList();
                if (rate.Count != 5)
                {
                    throw new Exception(line);
                }
                decimal amount = decimal.Parse(rate[2]);
                decimal rateValue = decimal.Parse(rate[4]);
                decimal currencyRate = rateValue / amount;
                return new ExchangeRate(new Currency(rate[3]), new Currency(defaultCurrency), currencyRate);
            }
            catch (Exception)
            {

                throw new Exception(line);
            }
           
        }
        /// <summary>
        /// The following function will filter the rates, according to the required currencies 
        /// </summary>
        /// <param name="rates">the last online rating</param>
        /// <param name="currencies">the required currencies</param>
        /// <returns></returns>
        public List<ExchangeRate> filterRates(List<ExchangeRate> rates , IEnumerable<Currency> currencies)
        {
            return rates.Where(rate => currencies.Any(currency => currency.Code.ToLower() == rate.SourceCurrency.Code.ToLower())).ToList();
        }
    }
}
