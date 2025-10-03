using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CNB
{
    internal class APICall
    {
        readonly string path;

        readonly HttpClient client;
        readonly StringBuilder result;
        
        HttpResponseMessage response;

        public APICall()
        {
            path = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
            
            client = new();
            result = new();
        }

        public async Task<string> DayliExchange()
        {

            response = await client.GetAsync(path);
            
            if(response.IsSuccessStatusCode)
            {
                result.Append(await response.Content.ReadAsStringAsync());
            }

            return result.ToString();
        }
    }
}
