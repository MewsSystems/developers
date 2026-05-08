using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace ExchangeRateUpdater
{
    // Class assosiated with kurzy element in API response
    [XmlRoot("kurzy")]
    public class XMLCourses
    {
        [XmlAttribute("banka")]
        public string Banka { get; set; }

        [XmlAttribute("datum")]
        public string Datum { get; set; }

        [XmlAttribute("poradi")]
        public int Poradi { get; set; }

        [XmlElement("tabulka")]
        public XMLTable Tabulka { get; set; }
    }

    // Class assosiated with tabulka element in API response
    public class XMLTable
    {
        [XmlAttribute("typ")]
        public string Typ { get; set; }

        [XmlElement("radek")]
        public List<XMLLine> Radek { get; set; }
    }

    // Class assosiated with radek element in API response
    public class XMLLine
    {
        [XmlAttribute("kod")]
        public string Kod { get; set; }

        [XmlAttribute("mena")]
        public string Mena { get; set; }

        [XmlAttribute("mnozstvi")]
        public int Mnozstvi { get; set; }

        [XmlAttribute("kurz")]
        public string Kurz { get; set; }

        [XmlAttribute("zeme")]
        public string Zeme { get; set; }
    }


    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        

        private async Task<XMLCourses> XMLResponseData()
        {
            using HttpClient client = new HttpClient();

            try
            {
                // GET request for CNB API
                HttpResponseMessage response = await client.GetAsync("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");

                // Wait for succesful response from API
                response.EnsureSuccessStatusCode();

                // Read the XML response
                using Stream stream = await response.Content.ReadAsStreamAsync();

                // Serialize
                var serializer = new XmlSerializer(typeof(XMLCourses));

                // Instead of reading from file locally, read directly from the response stream
                var responseData = (XMLCourses)serializer.Deserialize(stream);

                // return the deserialised response
                return responseData;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"API request error: {e.Message}");
                // Need return as will exit with error 
                return null;
            }
        }




        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var data = await XMLResponseData();
            var resultData = new List<ExchangeRate>();

            foreach(var item in data.Tabulka.Radek)
            {
                // Extract the currency code from the API response
                var ExternalCurrency = new Currency(item.Kod);
                bool isContained = false;

                // Filter to only show specified currencies
                foreach (var cur in currencies)
                {
                    // Check if the currency code from API is in defined currency List
                    if (cur.Code == ExternalCurrency.Code)
                    {
                        isContained = true;
                        break;
                    }
                }

                // If not part of defined list continue
                if (!isContained)
                    continue;


                var replaceValue = item.Kurz;

                // Replace "," with "." - to be converted to decimal
                replaceValue = replaceValue.Replace(",", ".");

                // Convert to decimal with ToDecimal() - input of replaceValue
                decimal currency = System.Convert.ToDecimal(replaceValue) / item.Mnozstvi;

                // Add the exhange to the list in new format
                resultData.Add(new ExchangeRate(new Currency("CZK"), ExternalCurrency, currency));
            }

            return resultData;
        }
    }
}
