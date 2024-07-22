using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CzechNationalBank.ApiClient
{
    public static class CNBApiConfiguration
    {
        public static void SetupHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.cnb.cz/cnbapi/");
        }

        public static string DailyExchangeRatesEndpoint => "exrates/daily?lang=EN";
    }
}
