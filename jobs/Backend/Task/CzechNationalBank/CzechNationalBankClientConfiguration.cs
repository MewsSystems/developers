using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    internal sealed class CzechNationalBankClientConfiguration
    {
        [Required]
        [Url]
        public string BaseUrl { get; init; } = default!;
    }
}