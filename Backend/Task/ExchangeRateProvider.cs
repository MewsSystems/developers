using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var result = string.Empty;

            //to set security protocol policy when using HTTP APIs in the .NET Framework
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Call mews-systems connector API for getting exchange rates
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://demo.mews.li/api/connector/v1/exchangeRates/getAll");

            //API only accepts "POST" requests with content type = "application/json"
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            //writing post data to the body of the request
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string postData = "{\"ClientToken\":\"E0D439EE522F44368DC78E1BFB03710C-D24FB11DBE31D4621C4817E028D9E1D\"," +
                              "\"AccessToken\":\"C66EF7B239D24632943D115EDE9CB810-EA00F8FD8294692C940F6B5A8F9453D\","
                              + "\"Client\":\"Client_Viral\"}";

                streamWriter.Write(postData);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                    //Reading response from the API
                    result = streamReader.ReadToEnd();
            }

            //converting string result to JSON
            JObject parsedJson = JObject.Parse(result);

            //getting the root element of the JSON
            JArray jsonArrays = (JArray)parsedJson["ExchangeRates"];

            //creating a Enumerable for returning the exchange rates
            List<ExchangeRate> ExchangeRateList = new List<ExchangeRate>();

            for(int i = 0;i< jsonArrays.Count;i++)
            {
                //deserializing object, creating ExchangeRate objects and adding to ExchangeRateList
                dynamic results = JsonConvert.DeserializeObject<dynamic>(jsonArrays[i].ToString());
                ExchangeRate tempExchangeRateObject = new ExchangeRate(new Currency(results.SourceCurrency.ToString()), new Currency(results.TargetCurrency.ToString()), (decimal)results.Value);
                ExchangeRateList.Add(tempExchangeRateObject);    
            }
            return ExchangeRateList;
        }
    }
}
