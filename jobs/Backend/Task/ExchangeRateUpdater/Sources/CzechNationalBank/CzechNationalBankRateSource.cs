using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Sources.CzechNationalBank;

public class CzechNationalBankRateSource : IRateSource
{
    // In real application this should be taken from a config file :)

    private static Uri MainDataSourceUrl = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
    private static Uri SecondaryDataSourceUrl = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt");

    private readonly ICzechNationalBankRateParser _parser;
    private readonly HttpClient _httpClient;

    public CzechNationalBankRateSource(ICzechNationalBankRateParser parser, HttpClient httpClient)
    {
        _parser = parser;
        _httpClient = httpClient;
        // Instead of injecting HttpClient directly, we can use an IHttpClientFactory.
        // Additionaly, we should add logging as a dependency, but for simplicity I will use Console.WriteLine for now
    }

    public string SourceName => "CzechNationalBank";
    public async ValueTask<IReadOnlyList<ExchangeRate>> GetRatesAsync(DateOnly targetDate)
    {
        var primaryRates = await GetPrimaryRatesAsync(_httpClient, targetDate);
        var secondaryRates = await GetSecondaryRatesAsync(_httpClient, targetDate);

        // We should also cache primary rates daily, and secondary rates monthly, and get the values from cache if they exist there.
        return primaryRates.Concat(secondaryRates).ToList();
    }

    private async Task<IReadOnlyList<ExchangeRate>> GetPrimaryRatesAsync(HttpClient httpClient, DateOnly targetDate)
    {
        try
        {
            var uri = CzechNationalBankRateUriHelper.BuildMainSourceUri(MainDataSourceUrl, targetDate);
            var response = await httpClient.GetAsync(uri);

            return await ParseBankResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while getting secondary rates. [Exception = {ex}]", ex);
            return [];
        }
    }


    private async Task<IReadOnlyList<ExchangeRate>> GetSecondaryRatesAsync(HttpClient httpClient, DateOnly targetDate)
    {
        try
        {
            var uri = CzechNationalBankRateUriHelper.BuildSecondarySourceUri(SecondaryDataSourceUrl, targetDate);
            var response = await httpClient.GetAsync(uri);
            return await ParseBankResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while getting secondary rates. [Exception = {ex}]", ex);
            return [];
        }
    }

    private async Task<IReadOnlyList<ExchangeRate>> ParseBankResponse(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseStr = await response.Content.ReadAsStringAsync();
        var parsedResponse = _parser.Parse(responseStr).ToList();

        return parsedResponse;
    }
}