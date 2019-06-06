using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        public ExchangeRateProvider()
        {
            _baseCurrency = new Currency(BASE_CURR_CODE);
            _format = new CultureInfo("cs-CZ");

        }
        private Currency _baseCurrency;

        private const string URL = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
        private const string BASE_CURR_CODE = "CZK";
        private CultureInfo _format;


        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (!currencies.Select(c => c.Code).Contains(BASE_CURR_CODE))
            {
                throw new System.Exception("Base currency of provider not present in provided list");
            }
            List<ExchangeRate> res = new List<ExchangeRate>();
            using (var client = new HttpClient())
            {
                var sr = new StreamReader(await client.GetStreamAsync(URL));
                //skip header
                await sr.ReadLineAsync();
                await sr.ReadLineAsync();
                string record;
                while ((record = await sr.ReadLineAsync()) != null)
                {
                    var split = record.Split("|");
                    decimal rate = 0;
                    decimal amount = 0;

                    bool success = decimal.TryParse(split[2], NumberStyles.Currency, _format, out amount) && decimal.TryParse(split[4], NumberStyles.Currency, _format, out rate);
                    if (!success)
                    {
                        throw new System.Exception("Could not parse exchange rates");
                    }
                    if (currencies.Select(c => c.Code).Contains(split[3])) res.Add(new ExchangeRate( new Currency(split[3]), _baseCurrency, rate / amount));
                }
            }

            return res;
        }
    }
}
