using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Helpers
{
    public class HttpClientHelper
    {
        public static async Task<string> GetResponseFromUrl(string url)
        {
            var content = string.Empty;

            try
            {
                var response = await new HttpClient().GetAsync(url);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return content;
        }
    }
}