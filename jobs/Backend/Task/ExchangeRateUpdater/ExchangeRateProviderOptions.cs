using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater;

public class ExchangeRateProviderOptions
{
    // 💡 in application using host builder, this would be read from configuration including validation of given input
    //    (validation would require introduction of custom validation attribute for `TimeSpan`)
    [Required]
    public TimeSpan CacheTtl { get; init; }
}
