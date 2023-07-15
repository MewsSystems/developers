using System.Collections.Generic;

namespace ExchangeRateUpdater.ApiClients.Responses;

/// <summary>
/// Represents the response for retrieving exchange rates.
/// </summary>
internal record GetExchangeRatesResponse(IEnumerable<ExchangeRateApiItem> Rates);

/// <summary>
/// Represents an exchange rate item from the API.
/// </summary>
internal record ExchangeRateApiItem(string CurrencyCode, int Amount, decimal Rate);