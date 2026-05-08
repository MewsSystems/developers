using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Models.CzechNationalBank;

public class CzechNationalBankClientOptions
{
    [Required]
    public string BaseUri { get; set;  }
    public int TimeoutMs { get; set; }
    public int RetryCount { get; set; }
}