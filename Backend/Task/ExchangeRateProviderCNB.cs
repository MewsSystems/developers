using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderCNB : ExchangeRateProviderCSV
    {
        public static string ApiUrl => "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";  // or via config
        public override char Separator => '|';

        protected override ExchangeRate processLineData(Dictionary<string, string> vals)
        {
            var rate = getDecimal("kurz", vals);
            if (!rate.HasValue)
                return null; // rate not specified
            var ccy = getString("kód", vals);            
            if (string.IsNullOrEmpty(ccy))
                return null; // currency code not specified
            return new ExchangeRate(new Currency("CZK"), new Currency(ccy), rate.Value); // source currency not specified for CNB
        }       

        public override IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                var data = downloadRawData(ApiUrl);
                if (string.IsNullOrWhiteSpace(data))
                    throw new Exception("No Data");  // blank response
                var items = data.Split('\n');
                if (items.Length < 3)
                    return Enumerable.Empty<ExchangeRate>(); // no rates
                initCSVHeader(items[1]); // header is on the second line
                return processCSVData(items.Skip(2), currencies);                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while processing conversion rates - {ex.Message}", ex);
            }
        }

       
    }
}
