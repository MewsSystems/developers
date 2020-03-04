using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRateProvider: IExchangeRateProvider
    {
        private const string CNB_URL_BASIC = "http://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string CNB_URL_OTHERS = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";
        private const char DELIMETER = '|';

        private const byte CODE_FIELD = 3;
        private const byte AMOUNT_FIELD = 2;
        private const byte RATE_FIELD = 4;

        private readonly Currency _targetCurrency;

        public CNBExchangeRateProvider()
        {
            _targetCurrency = new Currency("CZK");
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var basic = GetExchangeRatesFromURLAsync(currencies, CNB_URL_BASIC);
            var others = GetExchangeRatesFromURLAsync(currencies, CNB_URL_OTHERS);

            await Task.WhenAll(basic, others);

            return basic.Result.Concat(others.Result); // these should be distinct, no need to check for duplicates
        }

        /// <summary>
        /// 1st line: date
        /// 2nd line: table header
        /// </summary>
        /// <param name="reader">Reader obtained from file URL</param>
        /// <returns></returns>
        private async Task SkipHeaderAsync(StreamReader reader)
        {
            await reader.ReadLineAsync();
            await reader.ReadLineAsync();
        }

        /// <summary>
        /// Get exchage rates from source specified by URL.
        /// </summary>
        /// <param name="currencies">Desired currencies.</param>
        /// <param name="URL">URL of source data.</param>
        /// <returns>Collection of exchange rates present in data from URL.</returns>
        private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesFromURLAsync(IEnumerable<Currency> currencies, string URL)
        {
            List<ExchangeRate> result = new List<ExchangeRate>();

            using (var client = new HttpClientWrapper())
            {
                using (var reader = new StreamReader(await client.GetStreamAsync(URL, retriesLimit: 5)))
                {
                    await SkipHeaderAsync(reader);

                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] entries = line.Split(DELIMETER);

                        if (currencies.Select(c => c.Code).Contains(entries[CODE_FIELD])) // check if we need the line before parsing data
                        {

                            if (decimal.TryParse(entries[AMOUNT_FIELD], out decimal amount) && decimal.TryParse(entries[RATE_FIELD], out decimal rate))
                            {
                                result.Add(new ExchangeRate(new Currency(entries[CODE_FIELD]), _targetCurrency, rate / amount)); // some entries represent exchange rate per 100 or per 1000 units
                            }
                            else
                            {
                                throw new InvalidDataException($"Malformed line recieved from \"{URL}\": \"{line}\".");
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
