using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Infrastructure.Options;

public class CnbExchangeRatesUpdaterOptions
{
    [Required]
    public TimeOnly? DailyRefreshUtcTime { get; set; }
    
    [Required]
    public int? RetryOnRefreshFailureInSeconds { get; set; }
}
