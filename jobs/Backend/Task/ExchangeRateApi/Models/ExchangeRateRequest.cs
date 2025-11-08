using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateApi.Models;

/// <summary>
/// Request model for getting exchange rates
/// </summary>
[SwaggerSchema("Request for exchange rates containing currency codes and optional target currency")]
public class ExchangeRateRequest
{
    /// <summary>
    /// List of currency codes to get exchange rates for (e.g., ["USD", "EUR", "JPY"])
    /// </summary>
    /// <example>["USD", "EUR", "JPY", "GBP"]</example>
    [Required]
    [SwaggerSchema("List of ISO 4217 currency codes")]
    public List<string> CurrencyCodes { get; set; } = new();

    /// <summary>
    /// The target currency to get rates for (defaults to "CZK")
    /// </summary>
    /// <example>CZK</example>
    [SwaggerSchema("ISO 4217 target currency code (defaults to CZK if not specified)")]
    public string? TargetCurrency { get; set; }
}