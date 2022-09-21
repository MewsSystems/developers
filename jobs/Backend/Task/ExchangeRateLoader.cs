using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

internal class ExchangeRateLoader : IExchangeRateLoader
{
    private readonly Uri URL = new("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt");
    
    public async Task<Stream> ReadAsync()
    {
        var client = new HttpClient();
        var today = DateTime.Today.ToString("d", new CultureInfo("cs-CZ"));
        var response = await client.GetAsync($"{URL}?date={today}").ConfigureAwait(false);
        
        return await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync();
    }
}