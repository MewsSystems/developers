namespace ExchangeRateUpdater.Domain.Models.Dto
{
    public class ExchangeRatesResponseDto
    {
        public IEnumerable<ExchangeRateDto> ExchangeRates { get; set; }
    }
}
