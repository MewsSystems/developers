using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Exceptions;

namespace ExchangeRateUpdater.Contracts;

public interface ICurrencyRateService
{
    /// <summary>
    /// Gets currency exchange rates using existing rate provider
    /// </summary>
    /// <param name="providerName">Exchange provider name</param>
    /// <param name="fromCurrencies">Source currencies</param>
    /// <returns>Exchange rates</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NutSupportedRateProviderException"></exception>
    public Task<IEnumerable<ExchangeRate>> GetCurrencyRatesAsync(
        string providerName,
        IEnumerable<Currency> fromCurrencies);
}