using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class HttpHelper
    {
        public async static Task<string> LoadData(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var r = await httpClient.GetAsync(new Uri(url)))
                    {
                        return await r.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Došlo k následující chybě během zpracování požadavku na adrese '{url}': {ex.ToString()}.");
            }
        }
    }
}
