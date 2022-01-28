using ExchangeRateUpdater.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace ExchangeRateUpdater.Services
{
    public class HttpClientLineReader : IHttpClientLineReader
    {
        public async IAsyncEnumerable<string> ReadLines(string url)
        {
            using var client = new System.Net.Http.HttpClient();
            using var httpGETResult = await client.GetAsync(url);
            using var dataStream = await httpGETResult.Content.ReadAsStreamAsync();
            using var streamReader = httpGETResult.IsSuccessStatusCode ? new StreamReader(dataStream) : StreamReader.Null;

            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync();
                yield return line;
            };
        }
    }
}
