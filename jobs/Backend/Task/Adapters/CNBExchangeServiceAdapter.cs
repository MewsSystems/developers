using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Adapters
{
    public class CNBExchangeServiceAdapter : IExchangeRateService
    {
        public async Task<string> ExchangeRatesFromExternal()
        {
            var externalSource = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
            using HttpClient client = new HttpClient();
            using HttpResponseMessage response =  await client.GetAsync(externalSource);
            response.EnsureSuccessStatusCode();


            return await response.Content.ReadAsStringAsync();
        }
    }

}   