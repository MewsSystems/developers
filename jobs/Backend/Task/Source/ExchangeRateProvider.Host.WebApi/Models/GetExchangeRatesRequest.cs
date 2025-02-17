namespace ExchangeRateProvider.Host.WebApi.Models;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
///     Request model for fetching exchange rates.
/// </summary>
public class GetExchangeRatesRequest
{
    /// <summary>
    ///     The list of currency codes to be resolved.
    ///     Must be in ISO 4217 format (e.g., USD, EUR, CZK).
    /// </summary>
    [FromQuery]
    [SwaggerParameter(Description = "List of currency codes in ISO 4217 format (e.g., USD, EUR, CZK).")]
    public required IEnumerable<string> Items { get; set; }
}
