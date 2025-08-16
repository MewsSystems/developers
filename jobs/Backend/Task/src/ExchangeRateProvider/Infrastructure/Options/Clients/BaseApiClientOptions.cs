using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Infrastructure.Options.Clients;

public abstract class BaseApiClientOptions
{
    [Required]
    public string BaseUrl { get; set; } = default!;
    
    [Required]
    [Range(0, int.MaxValue)]
    public int? TimeoutInMs { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int? MaxRetries { get; set; }
}
