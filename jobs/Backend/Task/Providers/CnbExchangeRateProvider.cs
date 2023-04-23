using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Models;
using RestSharp;

namespace ExchangeRateUpdater.Providers;

/// <summary>
///     Exchange Rate Provider for the Czech National Bank.
/// </summary>
public class CnbExchangeRateProvider : IExchangeRateProvider
{
    /// <summary>
    ///     Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    ///     by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    ///     do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    ///     some of the currencies, ignore them.
    /// </summary>

    private readonly IExchangeRateClient _exchangeRateClient;

    public CnbExchangeRateProvider(IExchangeRateClient exchangeRateClient)
    {
        _exchangeRateClient = exchangeRateClient;
    }

    public async IAsyncEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies) //TODO: Abstract Data Source
    {
        var exchangeRates = await _exchangeRateClient.GetExchangeRateAsync();
        dynamic exchangeRatesParsed = JsonNode.Parse(exchangeRates) ?? throw new ApplicationException($"Error parsing exchange rates: {exchangeRates}");
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