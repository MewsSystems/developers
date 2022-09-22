using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

internal class ExchangeRateParser : IExchangeRateParser
{
    private static Currency Source => new("CZK");
    
    public async Task<IEnumerable<ExchangeRate>> ParseAsync(Stream values)
    {
        using var sr = new StreamReader(values, System.Text.Encoding.UTF8);
        var content = await sr.ReadToEndAsync().ConfigureAwait(false);
        var cleared = content.Trim();
        if (string.IsNullOrEmpty(cleared))
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        var rows = cleared.Split("\n");
        if (rows.Length < 3)
        {
            return Enumerable.Empty<ExchangeRate>();
        }
        
        var parts = rows[2..];
        return parts.Select(ParseLine).Where(w => w != null);
    }

    private static ExchangeRate ParseLine(string line)
    {
        if (string.IsNullOrEmpty(line))
        {
            return null;
        }

        var parts = line.Split('|');
        if (parts.Length < 5)
        {
            return null;
        }
        
        var target = new Currency(parts[3]);
        var rate = decimal.Parse(parts[4]);
        var amount = int.Parse(parts[2]);
        
        return new ExchangeRate(Source, target, rate / amount);
    }
}