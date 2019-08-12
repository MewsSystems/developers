using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Utils;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string ExchangeRateUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
                return Enumerable.Empty<ExchangeRate>();

            return GetExchangeRatesImpl(currencies);
        }
        
        private IEnumerable<ExchangeRate> GetExchangeRatesImpl(IEnumerable<Currency> currencies)
        {
            var currencyRows = GetCurrencyRows(currencies);   
            if (currencyRows == null || !currencyRows.Any())
                return Enumerable.Empty<ExchangeRate>();
            
            return currencyRows
                   .SelectMany(uc => currencyRows, (fr, se) =>
                   {   
                       try
                       {
                           return new ExchangeRate(
                               new Currency(fr.Code), 
                               new Currency(se.Code), 
                               decimal.Parse(fr.Rate) * se.Amount / (decimal.Parse(se.Rate) * fr.Amount));
                       }
                       catch (DivideByZeroException)
                       {
                           Console.WriteLine($"Couldn't parse [{fr.Code}({fr.Rate}) and [{se.Code}]({se.Rate})");
                           return null;
                       }
                   })
                   .Where(x => x != null);
        }

        private List<CnbResponseTableRow> GetCurrencyRows(IEnumerable<Currency> currencies)
        {
            var webResponse = WebRequestHelper.GetXmlRequest<CnbResponse>(new Uri(ExchangeRateUrl));  
            
            return webResponse?.Table?.Rows?.Where(x => currencies.Any(c => c.Code == x?.Code)).ToList();
        }        
    }
}
