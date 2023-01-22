using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.BankRatesManagers;

/// <summary>
/// Bank rate manager that supports implementation for bank of concrete state.
/// </summary>
public interface IBankRatesManager
{
    /// <summary>
    /// Gets object exchange rate from input string
    /// </summary>
    /// <param name="line">Input string line</param>
    /// <returns>Object ExchangeRate parsed from input</returns>
    ExchangeRate ParseLine(string line);

    /// <summary>
    /// Gets collection of objects exchange rate from input
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Collection of objects exchange rate parsed from input</returns>
    IEnumerable<ExchangeRate> Parse(string input);

    /// <summary>
    /// Returns uri for daily data source of bank
    /// </summary>
    /// <returns>Uri for daily data source of bank</returns>
    Uri GetDailyDataSourceUri();
}