using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {

        public static Currency DefaultSourceCurrency { get; set; }
        private string WebResult { get; set; }
        private List<ExchangeRate> ImportedRates { get; set; }

        public ExchangeRateProvider(Currency defaultSourceCurrency, string webResult)
        {
            DefaultSourceCurrency = defaultSourceCurrency;
            WebResult = webResult;
            ImportedRates = new List<ExchangeRate> { };
        }            

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {

            int i = 0;

            //Split by line
            foreach (var line in WebResult.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                //Ignore first 2 lines
                if (i >= 2)
                {
                    string country = "";
                    string currency = "";
                    int units = 1;
                    string code = "";
                    double rate = 0;

                    int j = 0;

                    //Split by pipe character into columns
                    string[] columns = line.Split('|');

                    foreach (string column in columns)
                    {
                        //Assign column to respective properties based on expected position in the line.
                        switch (j)
                        {
                            case 0:
                                country = column;
                                break;
                            case 1:
                                currency = column;
                                break;
                            case 2:
                                units = Int32.Parse(column);
                                break;
                            case 3:
                                code = column;
                                break;
                            case 4:
                                rate = Convert.ToDouble(column);
                                break;
                        }
                        j++;
                    }

                    ImportedRates.Add(new ExchangeRate(DefaultSourceCurrency, new Currency(country, currency, code), units, rate));

                }
                i++;
            }

            //Return, filtered to display only those in main currency list.
            return ImportedRates.Where(importedRate => currencies.Any(currency => currency.Code == importedRate.TargetCurrency.Code));
        }
    }
}
