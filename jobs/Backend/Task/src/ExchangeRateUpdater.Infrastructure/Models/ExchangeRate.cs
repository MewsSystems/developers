using Microsoft.EntityFrameworkCore;

namespace ExchangeRateFinder.Infrastructure.Models
{
    [PrimaryKey(nameof(SourceCurrencyCode), nameof(TargetCurrencyCode))]
    public class ExchangeRate
    {
        public string SourceCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }
        public string TargetCurrencyName { get; set; }
        public string CountryName { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; } 
    }
}
