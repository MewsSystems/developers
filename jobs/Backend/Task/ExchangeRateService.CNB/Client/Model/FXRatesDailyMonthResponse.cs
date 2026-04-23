namespace ExchangeRateService.CNB.Client.Model;

public class FXRatesDailyMonthResponse
{
    public IEnumerable<ExchangeRateBody> Rates { get; set; } = new List<ExchangeRateBody>();
}