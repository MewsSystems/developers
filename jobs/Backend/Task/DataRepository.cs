using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class DataRepository
    {
        public DataRepository() { }

        // read txt file data from czech national bank
        public virtual async Task<List<ExchangeRateModel>> GetData(List<string> currencyCodes, string url)
        { 
            var stream = await GetStream(url);

            var exchangeRateData = new List<ExchangeRateModel>();

            using (StreamReader reader = new StreamReader(stream))
            {

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.Contains("|"))
                    {

                        string[] lineData = line.Split("|");

                        var exchangeRate = new ExchangeRateModel
                        {
                            Country = lineData[0],
                            Currency = lineData[1],
                            Amount = lineData[2],
                            CurrencyCode = lineData[3],
                            Rate = lineData[4]

                        };


                        if (currencyCodes.Contains(exchangeRate.CurrencyCode))
                        {
                            exchangeRateData.Add(exchangeRate);
                        }
                    }

                }
            }

            return exchangeRateData;
        }

        // Makes API call to retrieve API data
        private static async Task<Stream> GetStream(string url)
        {
            var httpClient = new HttpClient();

            Stream stream = await httpClient.GetStreamAsync(url);

            return stream;
        }

        
    }
}
