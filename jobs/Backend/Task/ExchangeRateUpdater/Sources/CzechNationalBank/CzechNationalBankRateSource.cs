using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Sources.CzechNationalBank;

public class CzechNationalBankRateSource : IRateSource
{
    // In production environment this should be taken from the config file :)
    private static Uri DataSourceUrl = new Uri( "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
    private readonly ICzechNationalBankRateParser _parser;
    

    public CzechNationalBankRateSource(ICzechNationalBankRateParser parser)
    {
        _parser = parser;
    }

    public string SourceName => "CzechNationalBank";
    public async ValueTask<IReadOnlyList<ExchangeRate>> GetRatesAsync(DateOnly targetDate)
    {
        using var httpClient = new HttpClient();
        var uri = CzechNationalBankRateUriBuilder.BuildUri(DataSourceUrl, targetDate);
        var response = await httpClient.GetAsync(uri);
        var responseStr = await response.Content.ReadAsStringAsync();
        var parsedResponse = _parser.Parse(responseStr).ToList();

        // We should also add caching, because the rate is not changing throughout the day.
        
        return parsedResponse;
    }
}