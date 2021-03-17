using System.Net;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;


namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// I used special api from European Central bank and also I installed package System.Text.Json.
        /// The link is https://exchangeratesapi.io
        /// Firstly I am creating a list of Exchange Rates, which I will return.
        /// And then script goes throw all currencies and checks isn't the pair of currenies are equal.
        /// Next block creating a Get request from defined link, gets information and read it
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {

            List<ExchangeRate> MainRates = new List<ExchangeRate>();
            
        
            foreach (Currency baseCurrency in currencies)
            {
                foreach (Currency targetCurrency in currencies)
                {
                    if (baseCurrency.ToString() != targetCurrency.ToString())
                    {
                        try
                        {
                            string apiAddress = "https://api.exchangeratesapi.io/latest?base=" + baseCurrency.ToString();
                            WebRequest request = WebRequest.Create(apiAddress);
                            WebResponse response = request.GetResponse();

                            string requwstedData = "";
                            using (Stream dataStream = response.GetResponseStream())
                            {

                                StreamReader reader = new StreamReader(dataStream);
                                requwstedData = reader.ReadToEnd();

                            }


                            using (JsonDocument doc = JsonDocument.Parse(requwstedData))
                            {
                                decimal rate = doc.RootElement.GetProperty("rates").GetProperty(targetCurrency.ToString()).GetDecimal();
                                MainRates.Add(new ExchangeRate(baseCurrency, targetCurrency, rate));

                            }
                        }

                        catch
                        {

                        }
                    }
                    
                }
            }

            

            return MainRates;
        }
    }
}
