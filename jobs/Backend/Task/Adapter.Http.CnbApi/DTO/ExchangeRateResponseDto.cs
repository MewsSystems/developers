namespace Adapter.Http.CnbApi.DTO
{
    public class ExchangeRateResponseDto
    {
        public int Amount { get; set; }
        public required string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }

    public class ExchangeRatesResponseDto
    {
        public required List<ExchangeRateResponseDto> Rates { get; set; }
    }
}
