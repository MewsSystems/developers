using Microsoft.EntityFrameworkCore;

namespace ExchangeRateFinder.Infrastructure.Models
{
    [PrimaryKey(nameof(SourceCurrency), nameof(Code))]
    public class ExchangeRate
    {
        public string Code { get; set; } 
        public string Country { get; set; }
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public int Amount { get; set; }
        public decimal Rate { get; set; }
    }
}
