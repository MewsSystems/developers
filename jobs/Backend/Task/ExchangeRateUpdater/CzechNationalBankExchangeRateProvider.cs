using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
{
    private static readonly CsvConfiguration _csvConfiguration = new(CultureInfo.InvariantCulture)
    {
        DetectDelimiter = true
    };

    private readonly HttpClient _client;
    private readonly CzechNationalBankExchangeRateProviderOptions _options;

    public CzechNationalBankExchangeRateProvider(HttpClient client, IOptions<CzechNationalBankExchangeRateProviderOptions> optionsAccessor)
    {
        _client = client;
        _options = optionsAccessor.Value;
    }

    public async Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        await using var rawStream = await _client.GetStreamAsync(_options.ExchangeRatesEndpoint);
        using var reader = new StreamReader(rawStream);

        // Skip first line that contains unwanted data
        // For example: 07 Oct 2022 #195
        _ = await reader.ReadLineAsync();

        var currenciesCodes = currencies
            .Select(x => x.Code)
            .ToList();

        using var csvReader = new CsvReader(reader, _csvConfiguration);
        return await csvReader
            .GetRecordsAsync<CzechNationalBankExchangeRate>()
            .Where(x => currenciesCodes.Contains(x.Code, StringComparer.OrdinalIgnoreCase))
            .Select(x => new ExchangeRate(new(x.Code.ToUpperInvariant()), new("CZK"), x.Rate / x.Amount))
            .ToListAsync();
    }

    private class CzechNationalBankExchangeRate
    {
        [Index(0)]
        public string Country { get; set; }

        [Index(1)]
        public string Currency { get; set; }

        [Index(2)]
        public decimal Amount { get; set; }

        [Index(3)]
        public string Code { get; set; }

        [Index(4)]
        public decimal Rate { get; set; }
    }
}
