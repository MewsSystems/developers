using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Infrastructure.Options;

public class ExchangeRatesOptions
{
    [Required]
    public IEnumerable<string>? SupportedCurrencies { get; set; }
}
