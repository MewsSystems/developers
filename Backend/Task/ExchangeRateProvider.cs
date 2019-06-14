using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string CentralNationalBankDaylyRatesTextUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        private static readonly HttpClient HttpClient = new HttpClient();

        private static Currency TargetCurrency = new Currency("CZK");

        private static readonly CultureInfo RatesFormatCultureInfo = CultureInfo.GetCultureInfoByIetfLanguageTag("en-US");

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return GetExchangeRatesAsync(currencies).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Async version of <see cref="GetExchangeRates(IEnumerable{Currency})"/> 
        /// which looks preferable as it spends most of the time performing I/O bound operation
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var ratesText = await GetRawTextAsync();

            if (string.IsNullOrWhiteSpace(ratesText)) throw new DownloadExchangeRatesTextException();


            var allRates = ParseExchangeRates(ratesText, TargetCurrency);

            return FilterByCurrencyList(allRates, currencies);
        }

        private async Task<string> GetRawTextAsync()
        {
            return await HttpClient.GetStringAsync(
                CentralNationalBankDaylyRatesTextUrl).ConfigureAwait(false);
        }

        /// <summary>
        /// Parses a daily exchange rates text from Central National Bank site.
        /// The format is as follows:
        /// 13 Jun 2019 #113
        /// Country|Currency|Amount|Code|Rate
        /// Australia|dollar|1|AUD|15.660
        /// ...
        /// </summary>
        /// <param name="rateText"></param>
        /// <returns>List of rates</returns>
        private IEnumerable<ExchangeRate> ParseExchangeRates(string ratesText, Currency targetCurrency)
        {
            var lines = ratesText.Split(new[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            // Skip two header lines for now
            var rateLines = lines.Skip(2);

            var rates = new List<ExchangeRate>();
            foreach(var line in rateLines)
            {
                var row = line.Split('|');
                if (row.Length != 5)
                {
                    LogError($"Unexpected number of fields in a rate row: {line}");
                    continue;
                }
                if (!int.TryParse(row[2], out var amount))
                {
                    LogError($"Unexpected amount field format: {row[2]} in the line: {line}");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row[3]) || row[3].Length != 3 || row[3] != row[3].ToUpper())
                {
                    LogError($"Unexpected currency code format: {row[3]} in the line: {line}");
                    continue;
                }
                if (!decimal.TryParse(row[4], NumberStyles.Any, RatesFormatCultureInfo, out var rate))
                {
                    LogError($"Unexpected currency rate format: {row[4]} in the line: {line}");
                    continue;
                }

                rates.Add(new ExchangeRate(new Currency(row[3]), targetCurrency, Decimal.Divide(rate, amount)));
            }

            return rates;
        }

        private static void LogError(string message)
        {
            // Can apply max error policy like - throw exception after 10 errors
            Trace.TraceWarning(message);
        }

        private IEnumerable<ExchangeRate> FilterByCurrencyList(IEnumerable<ExchangeRate> allRates, IEnumerable<Currency> currencies)
        {
            return from r in allRates
                   join c in currencies on r.SourceCurrency.Code equals c.Code
                   select r;
        }
    }
}
