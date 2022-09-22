using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

internal class ExchangeRateLoader : IExchangeRateLoader
{
    private static Uri Url => new("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt");
    
    public async Task<Stream> ReadAsync()
    {
        var client = new HttpClient();
        var response = await client.GetAsync(Url).ConfigureAwait(false);
        
        return await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync().ConfigureAwait(false);
    }
}