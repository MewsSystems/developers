namespace ExchangeRateUpdater.Domain.Models.Dto
{
    public class ExchangeRatesResponseDto
    {
        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
    }
}
