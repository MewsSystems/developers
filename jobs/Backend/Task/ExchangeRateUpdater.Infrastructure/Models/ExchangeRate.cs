using System.ComponentModel.DataAnnotations;

namespace ExchangeRateFinder.Infrastructure.Models
{
    public class ExchangeRate
    {
        [Key]
        public string Code { get; set; } // Unique property
        public string Country { get; set; }
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public int Amount { get; set; }
        public decimal Rate { get; set; }
    }
}
