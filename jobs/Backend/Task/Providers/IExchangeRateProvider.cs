﻿using System.Collections.Generic;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Providers;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);

    /// <summary>
    /// Set new endpoints that will be used while fetching for new data
    /// </summary>
    void SetEndpoints(IEnumerable<IExchangeRateEndpoint> endpoints);

    /// <summary>
    /// Fetch exchange rates
    /// The method should be called after the object is created
    /// </summary>
    void FetchRates();
}