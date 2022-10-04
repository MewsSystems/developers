using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

internal class ExchangeRateLoader : IExchangeRateLoader
{
    private readonly Uri _url;
    
    public ExchangeRateLoader(Uri url)
    {
        _url = url;
    }

    public async Task<Stream> ReadAsync()
    {
        var client = new HttpClient();
        var response = await client.GetAsync(_url).ConfigureAwait(false);
        
        return await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync().ConfigureAwait(false);
    }
}