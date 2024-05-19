namespace ExchangeRateUpdater.Clients.CnbApi.DTOs
{
    public class GetExchangeRatesResponseDto
    {
        public IEnumerable<ExchangeRateResponseDto> Rates { get; set; }
    }

    public class ExchangeRateResponseDto
    {
        public string ValidFor { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }
}
