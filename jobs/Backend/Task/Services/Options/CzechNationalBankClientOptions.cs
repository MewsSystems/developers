using System.ComponentModel.DataAnnotations;

namespace Services.Options;

internal class CzechNationalBankClientOptions
{
    public const string CzechNationalBankClient = "CzechNationalBankClient";

    [Required]
    public required string BaseUrl { get; set; }
    [Required]
    public required string ExchangeRatesEndpoint { get; set; }
    [Required]
    public required string ForexRatesEndpoint { get; set; }
}
