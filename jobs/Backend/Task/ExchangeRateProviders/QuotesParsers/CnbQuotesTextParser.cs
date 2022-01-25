using ExchangeRateUpdater.CoreClasses;
using ExchangeRateUpdater.ExchangeRateProviders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders.QuotesParsers
{
    public class CnbQuotesTextParser : IQuotesParser
    {
        public IDictionary<Currency, ExchangeRate> ParseQuotes(Currency targetCurrency, string quotes)
        {
            if (targetCurrency == null) throw new ArgumentNullException(nameof(targetCurrency));
            if (quotes == null) throw new ArgumentNullException(nameof(quotes));
            if (string.IsNullOrWhiteSpace(quotes)) throw new ArgumentException(nameof(quotes));

            /*
                01 Oct 2021 #190
                Country|Currency|Amount|Code|Rate
                Australia|dollar|1|AUD|15.834
                Brazil|real|1|BRL|4.030
                ...
             * 
             */

            int dateLen = 11; //Should match beginning of line 1 text file structure
            string headers = "Country|Currency|Amount|Code|Rate"; //Should exactly match line 2 text file structure

            var quotesLines = quotes.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            //Checking txt header format
            if (DateTime.TryParse(quotesLines[0].Substring(0, 11), out var quotesDate) && quotesLines[1] == headers)
            {
                return quotesLines
                    .Skip(2)
                    .Select(line =>
                    {
                        var columns = line.Split('|');
                        var amount = decimal.Parse(columns[2]);
                        var currency = new Currency(columns[3]);
                        var rate = decimal.Parse(columns[4]);

                        return new ExchangeRate(currency, amount, targetCurrency, 1, rate);
                    })
                    .ToDictionary(pair => pair.SourceCurrency, pair => pair);
            }
            else
            {
                throw new Exception("Could not parse quote header. Check for structure changes");
            }
        }
    }
}
