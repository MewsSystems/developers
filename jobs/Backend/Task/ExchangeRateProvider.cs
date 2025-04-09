using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRateCsvRecord
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }

    public class ExchangeRateProvider
    {
        private const string DailyCnbUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var result = new List<ExchangeRate>();

            using var client = new HttpClient();
            var response = client.GetAsync(DailyCnbUrl).Result;
            response.EnsureSuccessStatusCode();

            using var stream = response.Content.ReadAsStream();
            using var reader = new StreamReader(stream);

            // Read and discard the first line ("09 Apr 2025 #70")
            reader.ReadLine();

            // Create CsvHelper configuration with the required delimiter
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "|"
            };

            // Create a CSV reader
            using var csv = new CsvReader(reader, config);

            // Read records from the CSV file
            var records = csv.GetRecords<ExchangeRateCsvRecord>().ToList();

            // Create a set of currency codes to check against
            var currenciesSet = new HashSet<string>(currencies.Select(c => c.Code));

            foreach (var record in records)
            {
                // Only add exchange rates for currencies that are explicitly defined in the source
                if (currenciesSet.Contains(record.Code))
                {
                    // Calculate the corrected rate based on Amount
                    var correctedRate = record.Rate / record.Amount;

                    result.Add(new ExchangeRate(
                        new Currency("CZK"), // Source currency is always CZK in this case
                        new Currency(record.Code),
                        correctedRate
                    ));
                }
            }

            return result;
        }
    }
}
