using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string CNB_RATE_URL = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        private const string CNB_EXPECTED_RATE_TABLE_HEADER = "země|měna|množství|kód|kurz";
        private const int EXPECTED_COLUMNS = 5;
        private const int POS_MUTIPLIER = 2;
        private const int POS_CURRENCY = 3;
        private const int POS_RATE = 4;

        private static Currency czk_curr = new Currency("CZK");

        private DateTime LastLoadDate;
        private int DayNo;
        private Dictionary<Currency, ExchangeRate> AllExchangeRates;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            GetRatesFromCNB();

            return SelectRates(currencies);
        }

        /* Get rates from CNB */
        private void GetRatesFromCNB()
        {
            string data;

            using (WebClient client = new WebClient())
            {
                try
                {
                    /* Download file with rate sa string */
                    data = client.DownloadString(CNB_RATE_URL);

                    /* Convert data to UTF-8 */
                    byte[] bytes = Encoding.Default.GetBytes(data);
                    data = Encoding.UTF8.GetString(bytes);
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.NameResolutionFailure)
                    {
                        throw new Exception($"Cannot get rates from CNB [Bad domain of URL: {CNB_RATE_URL}]");
                    }
                    else if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse response = (HttpWebResponse)ex.Response;
                        throw new Exception($"Cannot get rates from CNB [File with rates not found: {CNB_RATE_URL}]");
                    }

                    throw new Exception($"Cannot get rates from CNB [{ex.Message}]");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Cannot read rates from CNB {ex.Message}");
                }
            }

            ParseRatesFromCNB(data);
        }

        /* Parse rates from CNB into Dictionary */
        public void ParseRatesFromCNB(string data)
        {
            string line;
            int lineNo = 0;
            string[] fields;

            Decimal multipler;
            Currency curr;
            Decimal rate;
            ExchangeRate exRate;

            AllExchangeRates = new Dictionary<Currency, ExchangeRate>(new CurrencyComparer());
            //AllExchangeRates = new Dictionary<Currency, ExchangeRate>();

            using (StringReader reader = new StringReader(data))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lineNo += 1;

                    // System.Console.WriteLine($"Read line {lineNo} [{line}]");

                    /* Parse file header */
                    if (lineNo == 1)
                    {
                        fields = line.Split(' ');

                        if (fields.Length != 2)
                        {
                            throw new Exception($"Unsupported file format - wrong head [{line}]");
                        }

                        try
                        {
                            /* Parse datestamp of the file */
                            LastLoadDate = DateTime.Parse(fields[0]);

                            /* Parse dayNo of the file */
                            Match match = Regex.Match(fields[1], @"^\s*#(\d+)\s*$");
                            if (match.Success)
                            {
                                DayNo = Int32.Parse(match.Groups[1].Value);
                            }
                            else
                            {
                                System.Console.WriteLine($"Cannot parse day number from [{line}]");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Cannot parse file datestamp or date number [{line}]:[{ex.Message}]");
                        }

                        // System.Console.WriteLine($"Parsed file header [{LastLoadDate.ToShortDateString()}] [{DayNo}]");

                        continue;
                    }

                    /* Check table header */
                    if (lineNo == 2)
                    {
                        if (line.CompareTo(CNB_EXPECTED_RATE_TABLE_HEADER) != 0)
                        {
                            throw new Exception($"Table header is not as expected [{line}] vs [{CNB_EXPECTED_RATE_TABLE_HEADER}]");
                        }

                        // System.Console.WriteLine($"Table header accepted");

                        continue;
                    }

                    /* Parse rates */
                    fields = line.Split('|');
                    if (fields.Length != EXPECTED_COLUMNS)
                    {
                        throw new Exception($"Unsupported number of fields [{fields.Length}] vs [{EXPECTED_COLUMNS}]");
                    }

                    try
                    {
                        multipler = Decimal.Parse(fields[POS_MUTIPLIER]);
                        curr = new Currency(fields[POS_CURRENCY]);
                        rate = Decimal.Parse(fields[POS_RATE]);
                        exRate = new ExchangeRate(curr, czk_curr, rate / multipler);

                        AllExchangeRates.Add(curr, exRate);

                        // System.Console.WriteLine($"Parsed rate {lineNo-2}: {exRate}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Cannot parse rate from [{line}]:[{ex.Message}]");
                    }
                }
            }
        }

        /* Select requested rates */
        private IEnumerable<ExchangeRate> SelectRates(IEnumerable<Currency> currencies)
        {
            var selectedRates = new List<ExchangeRate>();
            // ExchangeRate rate;

            // System.Console.WriteLine($"Stored rates from {LastLoadDate.ToShortDateString()}, day number {DayNo}");
            // System.Console.WriteLine($"Stored rates for [{String.Join(",", AllExchangeRates.Keys.Select(o => o.ToString()).ToArray())}]");

            /* Select via foreach */
            /*
            foreach (Currency curr in currencies)
            {
                // System.Console.WriteLine($"Selecting {curr}");

                if (AllExchangeRates.TryGetValue(curr, out rate))
                {
                    selectedRates.Add(rate);
                }
                else
                {
                    System.Console.WriteLine($"Exchange rate for {czk_curr}/{curr} not available");
                }
            }
            */

            /* Select via Linq */
            selectedRates = AllExchangeRates.Where(item => currencies.Contains(item.Key, new CurrencyComparer())).Select(keypar => keypar.Value).ToList();

            return selectedRates;
        }
    }

    /* It will be better than create own CurrencyComparer to implement Equals() and GetHashCode() in Currency class */
    /*
        public class Currency {

            ...

            public override bool Equals(object obj)
            {
                try
                {
                    if ( string.Compare(((Currency)obj).Code, Code) == 0)
                    {
                        return true;
                    }
                }
                catch {
                    // Do not care
                }

                return false;
            }

            public override int GetHashCode()
            {
                return Code.GetHashCode();
            }
        }
    */

    /* Curreny Comparer to get data from Dictionary */
    class CurrencyComparer : IEqualityComparer<Currency>
    {
        public bool Equals(Currency x, Currency y)
        {
            try
            {
                if (String.Compare(x.Code, y.Code) == 0)
                {
                    return true;
                }
            }
            catch { /* Do not care */ }

            return false;
        }

        public int GetHashCode(Currency x)
        {
            return x.Code.GetHashCode();
        }
    }
}
