using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                
                if (currencies == null || !currencies.Any())
                {
                    return new List<ExchangeRate>();
                }

                var allCurrencies = currencies.ToArray();
                var response = await GetXmlSource();
                var exchangeRates = new List<ExchangeRate>();
                foreach (var item in response.Descendants())
                {
                    var currencyCode = GetXmlAttribute("kod", item);
                    if (!allCurrencies.Any(x => x.Code.ToLower().Equals(currencyCode.ToLower())))
                    {
                        continue;
                    }
                    var quantity = GetXmlAttribute("mnozstvi", item);
                    var rate = GetXmlAttribute("kurz", item);
                    var culture = CultureInfo.GetCultureInfo("cs-cs");
                    var canParseRate = decimal.TryParse(rate, NumberStyles.Any, culture, out decimal parsedRate);
                    var canParseQuantity = int.TryParse(quantity, out int parsedQuantity);
                    if (!string.IsNullOrWhiteSpace(currencyCode) & canParseQuantity & canParseRate)
                    {
                        if (allCurrencies.Any())
                        {
                            if (allCurrencies.Any(x => x.Code.ToLower().Equals(currencyCode.ToLower())))
                            {
                                var targetCurrency = allCurrencies.First(x => x.Code.ToLower().Equals("czk"));
                                var sourceCurrency = allCurrencies.First(x => x.Code.ToLower().Equals(currencyCode.ToLower()));
                                if(!(parsedQuantity > 0) || !(parsedRate > 0)) continue;
                                exchangeRates.Add(new ExchangeRate(sourceCurrency,targetCurrency,parsedRate/parsedQuantity));
                            }
                        }
                    }
                }
                return exchangeRates;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            

        }

        private static async Task<XDocument> GetXmlSource()
        {
            try
            {
                const string requestUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(requestUrl);
                var stream = await response.Content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(stream, Encoding.UTF8);
                var result = await streamReader.ReadToEndAsync();
                return XDocument.Parse(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception {ex.Message}");
                Console.WriteLine("Failed to get the xml source.");
                throw;
            }
            
        }

        private static string GetXmlAttribute(string elementName, XElement item)
        {
            try
            {
                if (item.HasAttributes && item.Attribute(elementName) != null)
                {
                    var elementValue = item.Attribute(elementName)?.Value;
                    if (elementValue != null)
                    {
                        if(!string.IsNullOrWhiteSpace(elementValue))
                        {
                            return elementValue;
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception {ex.Message}");
                Console.WriteLine("Failed to get the xml attribute.");
                throw;
            }

        }
    }
}
