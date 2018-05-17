using System.Collections.Generic;
using System.Net.Http;

namespace ExchangeRateUpdater.Utils
{
    public static class HttpClientProvider
    {
        private static readonly Dictionary<string, HttpClient> Clients = new Dictionary<string, HttpClient>();

        public static HttpClient GetClient(string apiId)
        {
            if (!Clients.ContainsKey(apiId))
            {
                Clients.Add(apiId, new HttpClient());
            }
            return Clients[apiId];
        }
    }
}
