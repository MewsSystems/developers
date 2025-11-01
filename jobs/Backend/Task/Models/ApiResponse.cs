using System.Collections.Generic;

namespace ExchangeRateUpdater.Models;

/// <summary>
///     Represents the response from the external exchange rate API.
///     Contains a collection of exchange rates for different currencies.
/// </summary>
public class ExchangeRateResponseDto
{
    public List<RateDto> Rates { get; init; }
}

/// <summary>
///     Represents a single currency exchange rate entry from the API response.
///     Contains the conversion rate and metadata for a specific currency.
/// </summary>
public class RateDto
{
    public string ValidFor { get; set; }
    public int Order { get; set; }
    public string Country { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public string CurrencyCode { get; set; }
    public decimal Rate { get; set; }
}