using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRateProvider : ICurrentExchangeRateProvider
    {
        public CNBExchangeRateProvider()
        {
            _baseCurrency = new Currency("CZK");
        }

        private const string CurrentExchangesFileUri = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        private readonly Currency _baseCurrency;

        /// <summary>
        /// Gets the current exchange rate records by CNB.
        /// </summary>
        /// <returns>The current CNB exchange rate records by CNB API.</returns>
        private IDictionary<string, CNBExchangeRateRecord> GetCurrentExchangeRateRecords()
        {
            Stream stream;
            var exchangeRateRecords = new Dictionary<string, CNBExchangeRateRecord>();

            try
            {
                stream = (new WebClient()).OpenRead(CurrentExchangesFileUri);
            }
            catch
            {
                return exchangeRateRecords;
            }

            using (var reader = new StreamReader(stream))
            {
                // Read first 2 header lines
                reader.ReadLine();
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var currencyLine = reader.ReadLine();
                    CNBExchangeRateRecord exchangeRateRecord = ParseRecordFromLine(currencyLine);

                    exchangeRateRecords.Add(exchangeRateRecord.IsoCode, exchangeRateRecord);
                }
            }

            return exchangeRateRecords;
        }

        private CNBExchangeRateRecord ParseRecordFromLine(string line)
        {
            var currencyInfo = line.Split('|');
            var isoCode = currencyInfo[3];
            var exchangeRate = decimal.Parse(currencyInfo[4].Replace(',', '.'));
            var amount = int.Parse(currencyInfo[2]);

            return new CNBExchangeRateRecord(isoCode, exchangeRate, amount);
        }

        /// <summary>
        /// Gets the current exchange rate by CNB API.
        /// </summary>
        /// <returns>The current exchange rate by CNB API.</returns>
        /// <param name="currencies">Currencies.</param>
        public IEnumerable<ExchangeRate> GetCurrentExchangeRate(IEnumerable<Currency> currencies)
        {
            var baseCurrency = currencies.FirstOrDefault(c => c.Equals(_baseCurrency));

            if (baseCurrency == null)
            {
                yield break;
            }

            IDictionary<string, CNBExchangeRateRecord> records = GetCurrentExchangeRateRecords();
            currencies = currencies.Where(c => records.ContainsKey(c.Code));

            foreach (Currency currency in currencies)
            {
                yield return new ExchangeRate(currency, baseCurrency, records[currency.Code].ExchangeRate);
            }
        }
    }
}
