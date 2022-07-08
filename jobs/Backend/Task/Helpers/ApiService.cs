using ExchangeRateUpdater.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Helpers
{
    public class ApiService : IApiService
    {
        public async Task<string> GetApiDataAsStringAsync(string uri)
        {
            if (String.IsNullOrEmpty(uri)) return null;

            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.PostAsync(uri, null))
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception)
            {
                //LOG
                return null;
            }
        }
    }
}