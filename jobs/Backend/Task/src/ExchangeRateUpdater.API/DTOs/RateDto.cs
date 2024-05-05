using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.API.DTOs;

public record RateDto
{
    [Required]
    public string Currency { get; set; } = default!;
    public DateTime? Date { get; set; }
}
