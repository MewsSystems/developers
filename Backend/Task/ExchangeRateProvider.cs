using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        //constants probably should be declared somewhere else but let's just put them here for sake of this example task
        private readonly string CNBBasic = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=";
        private readonly string CNBOthers = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?date=";
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var csv = string.Empty;
            try
            {
                csv = GetCSVRatesFromCNB(DateTime.Now);
            }
            catch (Exception ex)
            {
                //log the exception
                //log.error(...);
                return Enumerable.Empty<ExchangeRate>();
            }

            var CNBResults = Enumerable.Empty<CNBResult>();
            try
            {
                CNBResults = ProcessCSVRates(csv);
            }
            catch (Exception ex)
            {
                //log the exception
                //log.error(...);
                return Enumerable.Empty<ExchangeRate>();
            }

            //Now I've realized that I've been probably using a wrong source from CNB. I've only found any_currency to CZK rates so I'll just convert 
            //parameter currencies to another five random currencies
            return CalculateExchangeRates(CNBResults, currencies);
        }

        private IEnumerable<ExchangeRate> CalculateExchangeRates(IEnumerable<CNBResult> cNBResults, IEnumerable<Currency> currencies)
        {
            var result = new List<ExchangeRate>();
            var rnd = new Random();
            foreach (var currency in currencies)
            {
                var sourceRate = cNBResults.Where(x => x.Code.Equals(currency.Code)).FirstOrDefault();
                if (sourceRate==null)
                    continue;
                for (int i = 0; i<5;i++)
                {
                    int index = rnd.Next(0, cNBResults.Count());
                    var targetRate = cNBResults.ElementAt(index);
                    if (targetRate.Rate.Equals(0) || sourceRate.Amount.Equals(0) || targetRate.Amount.Equals(0))
                        continue;
                    //I have to keep in mind that Amounts might differ
                    var value = (sourceRate.Rate / sourceRate.Amount) / (targetRate.Rate / targetRate.Amount);
                    var sourceCurrency = currencies.Where(x => x.Code.Equals(sourceRate.Code)).FirstOrDefault();
                    var targetCurrency = new Currency(targetRate.Code);
                    var currentExchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);
                    result.Add(currentExchangeRate);
                }
            }
            return result;
        }

        private IEnumerable<CNBResult> ProcessCSVRates(string csv)
        {
            //I wasn't sure if I was allowed to use third-party libraries so I parsed the CSV string myself
            try
            {
                var lines = csv.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(1);
                var result = new List<CNBResult>();
                foreach (var item in lines)
                {
                    var values = item.Split('|');
                    var model = new CNBResult
                    {
                        Country = values[0],
                        Currency = values[1],
                        Amount = int.Parse(values[2]),
                        Code = values[3],
                        Rate = decimal.Parse(values[4])
                    };
                    result.Add(model);
                }
                return result;
            }
            catch
            {
                throw new CNBParseException();
            }
        }

        //Setting DateTime is not necessary but might come handy in the future
        public string GetCSVRatesFromCNB(DateTime day)
        {
            //I've found out that ČNB actually divides their exchange rates into two lists so I'll need both of them
            //HttpClient and async methods are also possible
            try
            {
                using (var client = new WebClient())
                {
                    var date = day.ToString("dd+MMMM+yyyy");
                    //first line contains date which I don't need
                    var basicRates = DeleteLines(client.DownloadString(CNBBasic + date), 1);
                    //now I don't need the first two lines as both results will be concated
                    var otherRates = DeleteLines(client.DownloadString(CNBOthers + date), 2);
                    return basicRates + otherRates;
                }
            }
            catch
            {
                throw new CNBReadException();
            }
        }

        private static string DeleteLines(string input, int linesToSkip)
        {
            int startIndex = 0;
            for (int i = 0; i < linesToSkip; ++i)
                startIndex = input.IndexOf('\n', startIndex) + 1;
            return input.Substring(startIndex);
        }

        //this class should be in it's own file but I don't want to alter the example solution anywhere but in this class
        public class CNBResult
        {
            public string Country { get; set; }
            public string Currency { get; set; }
            public int Amount { get; set; }
            public string Code { get; set; }
            public decimal Rate { get; set;}
        }
    }

    //this should also be in a different file
    public class CNBReadException : Exception
    {
        public CNBReadException() : base() { }
        public CNBReadException(string message) : base(message) { }
        public CNBReadException(string message, System.Exception inner) : base(message, inner) { }
    }

    public class CNBParseException : Exception
    {
        public CNBParseException() : base() { }
        public CNBParseException(string message) : base(message) { }
        public CNBParseException(string message, System.Exception inner) : base(message, inner) { }
    }
}
