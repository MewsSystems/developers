using System.Collections.Generic;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class RateSourceProvider : IRateSourceProvider
    {
        /// <summary>
        /// Provides raw currency rate sources
        /// </summary>
        /// <param name="sourceUrls">Urls to retrieve exchange rate source</param>
        /// <returns>List of raw currency rate source</returns>
        public IEnumerable<string> GetRateSourcesByUrl(IEnumerable<string> sourceUrls)
        {
            var result = new List<string>();
            
            using (HttpClient httpCLient = new HttpClient())
            {
                foreach (var sourceUrl in sourceUrls)
                {
                    var response = httpCLient.GetAsync(sourceUrl).Result;

                    response.EnsureSuccessStatusCode();
                    result.Add(response.Content.ReadAsStringAsync().Result);
                }
            }
            return result;
        }
    }
}
