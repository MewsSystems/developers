using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
       private const string ApiUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";

       private const char ColumnsSeparator = '|';
       private const char RowsSeparator = '\n';
       
       private readonly Currency DefaultQuoteCurrency = new Currency("CZK");
       
       private readonly HttpClient httpClient = new HttpClient();
    
       /// <summary>
       /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
       /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
       /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
       /// some of the currencies, ignore them.
       /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var apiResult = LoadCNBRates().Result;

            if (string.IsNullOrWhiteSpace(apiResult))
            {
                //log no data available, but web communication was successfull
                return Enumerable.Empty<ExchangeRate>();
            }
            
            IEnumerable<ExchangeRate> allExchangeRates = ParseResult(apiResult);

            Dictionary<string, ExchangeRate> currencyMap = allExchangeRates.ToDictionary(x => x.SourceCurrency.Code);

            List<ExchangeRate> filteredResults = new List<ExchangeRate>();
            
            foreach (Currency currency in currencies)
            {
                if (currencyMap.ContainsKey(currency.Code))
                {
                    filteredResults.Add(currencyMap[currency.Code]);
                }
            }

            return filteredResults;
        }

        private CNBExchangeData ParseLine(string[] values)
        {
            //Australia|dollar|1|AUD|16.136
            CNBExchangeData cnbExchangeData = new CNBExchangeData
            {
                Country = values[0],
                CurrencyName = values[1],
                Amount = int.Parse(values[2]),
                Code = values[3],
                Value = decimal.Parse(values[4])
            };

            return cnbExchangeData;
        }

        private IEnumerable<ExchangeRate> ParseResult(string apiResult)
        {
            //linux style, thats why I don't use Environment.NewLine
            string[] ratesLines = apiResult.Split(RowsSeparator);

            if (ratesLines.Length < 3)
            {
                //log error: data is not suitable format for parsing -> doesn't contain data in format needed
                
                throw new ApplicationException("Received data is not expected format");
            }
            
            //first line is header with date
            //18.Jan 2019 #13
            string dateTime = ratesLines[0];
            
            //second line is column names
            //Country|Currency|Amount|Code|Rate
            string[] header = ratesLines[1].Split(ColumnsSeparator);

            if (header.Length != 5)
            {
                //log error: data is not suitable format for parsing -> doesn't contain data in format needed
                throw new ApplicationException("Received data is not expected format. Cannot parse header.");
            }
            
            List<ExchangeRate> rates = new List<ExchangeRate>();
            
            foreach (string line in ratesLines.Skip(2))
            {
                var values = line.Split(ColumnsSeparator);

                if (values.Length != 5)
                {
                    //log error: data is not suitable format for parsing -> doesn't contain data in format needed
                    continue;
                }
                
                CNBExchangeData exchangeData;
                try
                {
                    exchangeData = ParseLine(values);
                }
                catch (Exception e)
                {
                    // log parsing error
                    continue;
                }
               
                 //CNB has only SOURCE to CZK rate
                var rate = new ExchangeRate(new Currency(exchangeData.Code), DefaultQuoteCurrency , exchangeData.ValueForOne);
                 
                rates.Add(rate);
            }

            return rates;
        }
        
        private async Task<string> LoadCNBRates()
        {
            string rateResults = string.Empty;
            try
            {
                rateResults = await httpClient.GetStringAsync(ApiUrl);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Data couldn't be retrieved from '{ApiUrl}'", e);
            }

            return rateResults;
        }
    }
    
    public class CNBExchangeData
    {
        public string Country { get; set; }
        public string CurrencyName { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }

        internal decimal ValueForOne => Value / Amount;
    }
}
