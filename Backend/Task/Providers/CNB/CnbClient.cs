using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Class for getting the exchange rates in XML format from CNB web.
    /// 
    /// It might seem useless to extract this client to its own class,
    /// but had it stayed inside CnbExchangeRateProvider that class could not be correctly unit tested,
    /// because it would have a hardcoded dependency on an external service (CNB web).
    /// 
    /// This way CnbExchangeRateProvider can get unit tests and CnbClient can get integration tests.
    /// </summary>
    public class CnbClient : ICnbClient
    {
        private readonly HttpClient _httpClient;

        public CnbClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = new TimeSpan(0, 10, 0);
        }

        /// <summary>
        /// Gets the exchange rates using the provided CNB url
        /// </summary>
        /// <param name="cnbUrl"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CnbXmlRow>> ReadExchangeRatesFromUrlAsync(string cnbUrl)
        {
            var result = new List<CnbXmlRow>();

            try
            {
                //getting the stream seems a little faster than URL + XmlReader, but CNB is still slow
                var stream = await _httpClient.GetStreamAsync(cnbUrl);

                using (var xml = XmlReader.Create(stream, new XmlReaderSettings { IgnoreWhitespace = true }))
                {
                    xml.ReadToDescendant("tabulka");

                    while (xml.Read())
                    {
                        if (!ValidateRow(xml)) continue;

                        result.Add(new CnbXmlRow
                        {
                            CurrencyCode = xml.GetAttribute("kod"),
                            CurrencyName = xml.GetAttribute("mena"),
                            Amount = int.Parse(xml.GetAttribute("mnozstvi")),
                            ExchangeRate = decimal.Parse(xml.GetAttribute("kurz")),
                            Country = xml.GetAttribute("zeme")
                        });
                    }
                }
            }
            catch(InvalidOperationException e)
            {
                //most likely an incorrect URL => log exception
            }
            catch(Exception e)
            {
                //log exception
            }

            return result;
        }

        private bool ValidateRow(XmlReader reader)
        {
            if(!int.TryParse(reader.GetAttribute("mnozstvi"), out int amount) ||
                !decimal.TryParse(reader.GetAttribute("kurz"), out decimal rate))
            {
                return false;
            }

            return true;
        }
    }
}