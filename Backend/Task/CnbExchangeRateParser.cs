using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ExchangeRateUpdater {
    class CnbExchangeRateParser : IExchangeRateParser {
        private static readonly CultureInfo _csCulture = CultureInfo.GetCultureInfo("cs");
        private static readonly Currency _czk = new Currency("CZK");

        public async Task<IEnumerable<ExchangeRate>> ParseAsync(Stream stream) {
            var rates = new List<ExchangeRate>();

            using (var reader = new StreamReader(stream)) {
                string line = null;

                while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null) {
                    if (this.TryParseLine(line, out var rate)) {
                        rates.Add(rate);
                    }
                };
            }

            return rates;
        }

        private bool TryParseLine(string line, out ExchangeRate result) {
            var chunks = line.Split('|');
            if (chunks.Length != 5 ||
                !int.TryParse(chunks[2], out var amount) ||
                !decimal.TryParse(chunks[4], NumberStyles.Currency, _csCulture, out var rate)
              ) {
                result = null;
                return false;
            }

            result = new ExchangeRate(_czk, new Currency(chunks[3]), rate / amount);
            return true;
        }
    }
}
