using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string url = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt;jsessionid=0EC0A4838228AF2925056C7F18F47D1D?date=01.02.2022";
            using var client = new System.Net.WebClient();  // WebClient downloads web pages and files
            var content = client.DownloadString(url);       // download the web page (via url) to "content"
            var exRates = new List<ExchangeRate>();         // empty List to hold found exchange rates

            // build a DataTable from the source data.
            // valDate is potentially useful (eg. comparing age of two tables)
            var dTable = CsvStrToTable(content, out string valDate);

            foreach (DataRow row in dTable.Rows)
            {
                string srcCode = row[3].ToString(); // pull the source currency's ISO-4217 code
                string trgCode = "CZK";             // could be any code but from this url it makes sense

                if (currencies.Any(x => x.Code == srcCode))
                {
                    Currency srcCurr = new Currency(srcCode);
                    Currency trgCurr = new Currency(trgCode);

                    int srcAmount = int.Parse(row[2].ToString());       // source amount will be whole
                    decimal trgRate = decimal.Parse(row[4].ToString()); // web page gives target currency by rate, not amount

                    // ensures rate is in 1-to-1 basis, eg. some exchanges are 100-to-1
                    decimal exRate = srcAmount == 1 ? trgRate : decimal.Divide(trgRate, srcAmount);

                    exRates.Add(new ExchangeRate(srcCurr, trgCurr, exRate));    // add rates to the list to be returned
                }
            }

            return exRates;
        }

        /// <summary>
        /// Reads string into table, assumes csv format of string. 
        /// Outputs a DataTable along with a string for the date of validity of the info.
        /// </summary>
        private DataTable CsvStrToTable(string content, out string date)
        {
            char[] DELIMS = { ',', '|', ';', '\t' }; // typical CSV delimiters
            string[] lines = content.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            // Search for most common and evenly-distributed separator
            // Assumes some reliability from data source, but covers simple modifications.
            //     eg. if data source switches delimiter from '|' to ','
            var query = DELIMS.Select(x => new
                { Separator = x, Found = lines.GroupBy(line => line.Count(c => c == x)) })
                .OrderByDescending(delim => delim.Found.Count(g => g.Key > 0))
                .ThenBy(delim => delim.Found.Count()).First();
            char delim = query.Separator;

            var table = new DataTable();
            date = lines[0];    // in this case first line is a date, not part of the "table"

            // add columns/headers to the table
            string[] headers = lines[1].Split(delim);
            foreach (string header in headers)
                table.Columns.Add(header);

            // add rows to the table
            for (int i = 2; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] values = line.Split(delim);

                var dtRow = table.NewRow();
                for (int j = 0; j < headers.Length; j++)
                    dtRow[j] = values[j];

                table.Rows.Add(dtRow);
            }

            return table;
        }
    }
}