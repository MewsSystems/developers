using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using ExchangeRateUpdater.Models;
using RestSharp;

namespace ExchangeRateUpdater.Providers;

/// <summary>
///     Exchange Rate Provider for the Czech National Bank.
/// </summary>
public class CnbExchangeRateProvider : IExchangeRateProvider, IDisposable
{
    /// <summary>
    ///     Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    ///     by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    ///     do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    ///     some of the currencies, ignore them.
    /// </summary>
    private readonly RestClient _client;

    public CnbExchangeRateProvider()
    {
        _client = new RestClient("https://api.cnb.cz/cnbapi/");
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this); // Prevents garbage collector calling object finalizer.
    }

    public async IAsyncEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies) //TODO: Abstract RestSharp and Data Source
    {
        var request = new RestRequest("exrates/daily");
        var response = await _client.ExecuteGetAsync(request);
        if (!response.IsSuccessful || response.Content == null)
            throw new ApplicationException($"Error fetching exchange rates: {response.ErrorMessage}");

        dynamic exchangeRatesParsed = JsonNode.Parse(response.Content) ?? throw new ApplicationException($"Error parsing exchange rates: {response.ErrorMessage}");
        var currenciesList = currencies.ToList();

        foreach (var rate in exchangeRatesParsed["rates"])
        {
            if (currenciesList.Any(c => c.Code == rate["currencyCode"].ToString()))
                yield return new ExchangeRate
                {
                    CurrencyCode = rate["currencyCode"].ToString(),
                    CurrencyValue = rate["rate"].GetValue<decimal>()
                };
        }
    }

    public async void PrintExchangeRates(IEnumerable<Currency> currencies)
    {
        await foreach (var exchangeRate in GetExchangeRates(currencies)) Console.WriteLine(exchangeRate.ToString());
    }
}