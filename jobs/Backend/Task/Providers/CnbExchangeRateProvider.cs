using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.ExchangeRateSource;
using ExchangeRateUpdater.Models;

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

    private readonly IExchangeRateSource _exchangeRateSource;

    public CnbExchangeRateProvider(IExchangeRateClient exchangeRateClient, IExchangeRateSource exchangeRateSource)
    {
        _exchangeRateClient = exchangeRateClient;
        _exchangeRateSource = exchangeRateSource;
    }

    /// <summary>
    ///     Gets the daily exchange rates from the Czech National Bank (CNB)
    /// </summary>
    /// <param name="currencies">The currencies to get the exchange rates of.</param>
    /// <returns>Returns exchange rates asynchronously as they're fetched.</returns>
    /// <exception cref="ApplicationException">Thrown when the exchanged rates from CNB are null.</exception>
    public async IAsyncEnumerable<ExchangeRate>
        GetExchangeRates(IEnumerable<Currency> currencies) //TODO: Abstract Data Source
    {
        var exchangeRates = await _exchangeRateClient.GetExchangeRateAsync();
        _ = exchangeRates ??
            throw new ApplicationException($"Error parsing exchange rates using {nameof(_exchangeRateClient)}");

        dynamic exchangeRatesParsed = JsonNode.Parse(exchangeRates);
        var currenciesList = currencies.ToList();

        foreach (var rate in exchangeRatesParsed!["rates"])
            if (currenciesList.Any(c => c.Code == rate["currencyCode"].ToString()))
                yield return new ExchangeRate
                {
                    SourceCurrencyCode = rate["currencyCode"].ToString(),
                    TargetCurrencyCode = _exchangeRateSource.CurrencyCode.Code,
                    CurrencyValue = rate["rate"].GetValue<decimal>()
                };
    }

    public async void PrintExchangeRates(IEnumerable<Currency> currencies)
    {
        await foreach (var exchangeRate in GetExchangeRates(currencies)) Console.WriteLine(exchangeRate.ToString());
    }
}