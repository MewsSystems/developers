using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ExchangeRateUpdater

{
    public class ExchangeRateProvider
    {

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// 
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return Enumerable.Empty<ExchangeRate>();
        }
        public async Task<string> GetStringOfExchangeRates()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("exchangeRateUpdater", "test");

            string currencyList = null;

            HttpResponseMessage currencyResponse = await client.GetAsync("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");

            if (currencyResponse.IsSuccessStatusCode)
            {
                currencyList = await currencyResponse.Content.ReadAsAsync<string>();
            }

            return currencyList;
        }
        public /*List<string>*/ string GetAllFoos()
        {
           var client = new HttpClient();

            client.DefaultRequestHeaders.Add("exchangeRateUpdater", "test");

            //List<string> currencyList = null;
            string xmlString = null;
            XmlDocument xmlDoc = new XmlDocument();
            HttpResponseMessage response = client.GetAsync("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml").Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                var readed = content.ReadAsStringAsync();
                xmlString = readed.Result;
               
                xmlDoc.LoadXml(xmlString);
                
                //byte[] byteArray = Encoding.ASCII.GetBytes(xmlString);
                //MemoryStream stream = new MemoryStream(byteArray);
                //XmlSerializer deSerializer = new XmlSerializer(typeof(List<ExchangeRate>));
                //object obj = deSerializer.Deserialize(stream);
                //ExchangeRateList XmlData = (ExchangeRateList)obj;
                //xmlString = response.Content.ReadAsAsync<string>().Result.ToString();
                //currencyList = response.Content.ReadAsAsync<List<ExchangeRates>>().Result.ToList();
            }
            return /*currencyList*/ xmlString;
        }
    }
}

//Just a quick note for the task for mews. Be consistent (same naming convention through whole solution, var/explicit variables), 
//make it selfcommenting and use comments when necessary, if there is some code written already like helper classes, try to use them

    