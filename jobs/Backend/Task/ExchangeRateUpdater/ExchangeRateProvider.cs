﻿using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Clients.Models;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICzechNationalBankExchangeRateClient _cnbExchangeRateClient;

    public ExchangeRateProvider(ICzechNationalBankExchangeRateClient cnbExchangeRateClient)
    {
        _cnbExchangeRateClient = cnbExchangeRateClient;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IReadOnlyCollection<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var cnbExchangeRates = _cnbExchangeRateClient.GetCurrentRates();
        var exchangeRates = MapCnbExchangeRateToExchangeRate(cnbExchangeRates);
        return FilterRatesByCurrencies(currencies, exchangeRates);
    }

    private static IReadOnlyCollection<ExchangeRate> FilterRatesByCurrencies(IEnumerable<Currency> currencies,
        IEnumerable<ExchangeRate> exchangeRates) =>
        exchangeRates.Where(x => currencies.Contains(x.TargetCurrency)).ToList();

    private IReadOnlyCollection<ExchangeRate> MapCnbExchangeRateToExchangeRate(CnbExchangeRates cnbExchangeRates)
    {
        var result = new List<ExchangeRate>();
        var sourceCurrency = new Currency("CZK");
        foreach (var cnbExchangeRate in cnbExchangeRates.Rates)
        {
            var targetCurrency = new Currency(cnbExchangeRate.CurrencyCode);
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, cnbExchangeRate.Rate);
            result.Add(exchangeRate);
        }

        return result;
    }
}