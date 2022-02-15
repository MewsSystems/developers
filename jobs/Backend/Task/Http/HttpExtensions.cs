using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Fluent;

namespace ExchangeRateUpdater.Http
{
    public static class HttpExtensions
    {
        public static async Task<Fluent<Stream>> ReadStreamFromHttp(this FluentState fluentState, 
            Uri requestUrl)
        {
            using var client = new HttpClient();
            fluentState.AddDisposable(client);
            
            var response = await client.GetAsync(requestUrl);

            var stream = await response.Content.ReadAsStreamAsync();
            fluentState.AddDisposable(stream);
            
            return fluentState.Create(stream);
        }
    }
}