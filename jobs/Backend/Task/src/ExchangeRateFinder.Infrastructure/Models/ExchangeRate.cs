using Microsoft.EntityFrameworkCore;

namespace ExchangeRateFinder.Infrastructure.Models
{
    [PrimaryKey(nameof(SourceCurrencyCode), nameof(TargetCurrencyCode))]
    public class ExchangeRate
    {
        public string SourceCurrencyCode { get; set; } = string.Empty;
        public string TargetCurrencyCode { get; set; } = string.Empty;
        public string TargetCurrencyName { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public int Amount { get; set; }
        public decimal Value { get; set; } 
    }
}
