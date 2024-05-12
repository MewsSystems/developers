namespace ExchangeRateUpdater.Core.Models.CzechNationalBank
{
    public class ExchangeRatesDailyDto
    {
        public List<ExchangeRateResponse> Rates { get; set; } = new List<ExchangeRateResponse>();
    }
}
