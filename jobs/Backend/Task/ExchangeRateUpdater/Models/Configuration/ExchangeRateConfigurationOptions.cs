using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Models.Configuration;

public class ExchangeRateConfigurationOptions
{
    /// <summary>
    /// Three-letter ISO 4217 code of the desired Source Currency.
    /// </summary>
    [Required]
    [Length(3,3)]
    public string SourceCurrency { get; init; }
}