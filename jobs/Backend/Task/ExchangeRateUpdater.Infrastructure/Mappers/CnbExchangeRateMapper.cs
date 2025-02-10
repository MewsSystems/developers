using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.Providers;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Infrastructure.Mappers;

/// <summary>
/// Provides mapping methods to convert CNB API responses into domain exchange rate models.
/// </summary>
public static class CnbExchangeRateMapper
{
    private const string SourceCurrencyCode = "CZK";

    /// <summary>
    /// Maps the CNB API response to a collection of domain-level exchange rate models.
    /// </summary>
    /// <param name="cnbApiResponse">The response from the CNB API.</param>
    /// <returns>A collection of <see cref="ExchangeRate"/> models.</returns>
    public static IEnumerable<ExchangeRate> MapToDomainModel(CnbApiResponse? cnbApiResponse)
    {
        if (cnbApiResponse?.Rates == null)
        {
            return [];
        }

        return cnbApiResponse.Rates.Select(rate => new ExchangeRate(
            new Currency(SourceCurrencyCode),
            new Currency(rate.CurrencyCode),
            rate.Rate / rate.Amount
        ));
    }
}
