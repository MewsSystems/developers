namespace ExchangeRateService.Client.Model.CNB;

public class FXRatesDailyMonthResponse
{
    public IEnumerable<ExchangeRateBody> Rates { get; set; } = new List<ExchangeRateBody>();
}