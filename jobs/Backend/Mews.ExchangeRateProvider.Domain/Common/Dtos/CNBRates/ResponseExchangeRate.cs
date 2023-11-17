namespace Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates
{
    public class ResponseExchangeRate
    {
        public string ValidFor { get; set; } = null!;

        public int Order { get; set; }

        public string Country { get; set; } = null!;

        public string Currency { get; set; } = null!;

        public int Amount { get; set; } 

        public string CurrencyCode { get; set; } = null!;

        public decimal Rate { get; set; }
    }
}
