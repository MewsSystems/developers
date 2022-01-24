using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string ExchangeUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        private const string FirstLinePattern = "^((0[1-9]|[12][0-9]|3[01])[-/.](0[1-9]|1[012])[-/.](19|20)\\d\\d)\\s\\s*#(\\d+)\\s*$"; //21.01.2022 #15
        private const string HeaderPattern = "země|měna|množství|kód|kurz";
        private const int Amount = 2;
        private const int CountyCode = 3;
        private const int Rate = 4;
        private const int ColumnCount = 5;

        private Dictionary<Currency, ExchangeRate> ValidatedExchangeRates;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var sourceRates = GetSourceRates();
            ParseRates(sourceRates);
            return SelectRates(currencies);
        }

        private string GetSourceRates()
        {
            var client = new HttpClient();

            try
            {
                using (var response = client.GetAsync(ExchangeUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var obtainedText = content.ReadAsStringAsync().Result;
                            return StringEncodingHelper.ToUtf8(obtainedText);

                        }
                    }

                    throw new HttpRequestException($"Cannot get rates due to status: {response.StatusCode}"); //we can improve our error handling with handling each response status, but for now it's enough
                }
            }
            catch (Exception e)
            {
                throw new Exception($"There's an issue with getting rates from CNB portal : {e}");
            }
        }

        public void ParseRates(string inputText)
        {
            string line;
            int count = 0;
            ValidatedExchangeRates = new Dictionary<Currency, ExchangeRate>();

            using (StringReader reader = new StringReader(inputText))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    count++;

                    switch (count)
                    {
                        case int n when (n == 1):
                            ParseDateAndNumber(line);
                            continue;

                        case int n when (n == 2):
                            CheckColumnCount(line);
                            ParseHeader(line);
                            continue;

                        case int n when (n >= 3):

                            CheckColumnCount(line);
                            ParseCodeAndRates(line);
                            continue;

                        default:
                            throw new ArgumentException(message: "Row is invalid: ", paramName: line);
                    }
                }
            }
        }

        private IEnumerable<ExchangeRate> SelectRates(IEnumerable<Currency> currencies)
        {
            var filteredRates = new List<ExchangeRate>();

            IEnumerator<Currency> iEnumeratorCurrency = currencies.GetEnumerator();
            var validated = ValidatedExchangeRates.Values.ToList();

            //Yeap, this is not the best solution, rather use a more complex linq
            while (iEnumeratorCurrency.MoveNext())
            {
                foreach (var val in validated)
                {
                    string iso = val.ToString().Substring(0, 3);
                    if (iso.Contains(iEnumeratorCurrency.Current.ToString()))
                    {
                        filteredRates.Add(val);
                    }
                }
            }

            return filteredRates;
        }

        public void ParseDateAndNumber(string line)
        {
            string[] dateTimeLine;

            dateTimeLine = line.Split(' ');

            if (dateTimeLine.Length != 2)
            {
                throw new Exception($"Wrong file format: {line}");
            }

            try
            {
                Match matched = Regex.Match(line, @$"{FirstLinePattern}");

                if (!matched.Success)
                    throw new Exception($"There's an issue while parsing date and number: {line}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem with parsing date and number: {ex.Message}, line: { line }");
            }
        }

        public void ParseHeader(string line)
        {
            bool isEqual = string.Equals(line, HeaderPattern);
            if (!isEqual)
                throw new Exception($"Header pattern is different. Expected: {HeaderPattern}, result: {line}");
        }

        public void ParseCodeAndRates(string line)
        {

            var columns = line.Split('|');
            var targetCurrency = new Currency("CZK");

            try
            {
                var currency = new Currency(columns[CountyCode]);
                var amount = decimal.Parse(columns[Amount]);
                var rate = decimal.Parse(columns[Rate]);
                var value = rate / amount;
                var exchangeRate = new ExchangeRate(currency, targetCurrency, value);

                ValidatedExchangeRates.Add(currency, exchangeRate);
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot parse rate from [{line}]:[{ex.Message}]");
            }
        }

        public void CheckColumnCount(string line)
        {
            var columns = line.Split('|');
            if (columns.Length != ColumnCount)
                throw new Exception($"Column length is different. Expected: {ColumnCount}, result: {columns.Length}");
        }

        public static class StringEncodingHelper
        {
            public static string ToUtf8(string text)
            {
                return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
            }
        }
    }
}