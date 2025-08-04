namespace ExchangeRateUpdater.Core.Models.CzechNationalBank
{
    public class ExchangeRateResponse
    {
        public string ValidFor { get; set; } = string.Empty;

        public int Order { get; set; }

        public string Country { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public int Amount { get; set; }

        public string CurrencyCode { get; set; } = string.Empty;
        
        public decimal Rate { get; set; }
    }
}
