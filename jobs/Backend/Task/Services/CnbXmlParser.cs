using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace ExchangeRateUpdater.Services
{
    /// <summary>
    /// Parses XML data from the CNB exchange rate feed, extracting rates
    /// for requested currencies only.
    /// </summary>
    public class CnbXmlParser : ICnbXmlParser
    {
        private readonly ILogger<CnbXmlParser> _logger;

        public CnbXmlParser(ILogger<CnbXmlParser> logger)
        {
            _logger = logger;
        }

        public IEnumerable<ExchangeRate> Parse(string xml, IEnumerable<Currency> requestedCurrencies)
        {
            var requestedCodes = requestedCurrencies.Select(c => c.Code);
            var czk = new Currency("CZK");

            var result = new List<ExchangeRate>();

            var document = XDocument.Parse(xml);
            var rows = document.Descendants("radek");

            foreach (var row in rows)
            {
                var code = row.Attribute("kod")?.Value;
                if (code == null || !requestedCodes.Contains(code))
                    continue;

                try
                {
                    var amountStr = row.Attribute("mnozstvi")?.Value ?? "1";
                    var rateStr = row.Attribute("kurz")?.Value ?? "0";

                    var amount = int.Parse(amountStr, CultureInfo.InvariantCulture);
                    var rate = decimal.Parse(rateStr, new CultureInfo("cs-CZ"));

                    result.Add(new ExchangeRate(
                        sourceCurrency: czk,
                        targetCurrency: new Currency(code),
                        value: rate / amount
                    ));
                }
                catch
                {
                    _logger.LogWarning("Unable to parse rate for currency: {Code}", code);
                }
            }

            _logger.LogInformation("Parsed {Count} exchange rates from XML.", result.Count);
            return result;
        }
    }
}
