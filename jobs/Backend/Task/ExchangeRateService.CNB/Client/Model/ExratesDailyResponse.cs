namespace ExchangeRateService.CNB.Client.Model;

public class ExratesDailyResponse
{
    public IEnumerable<ExchangeRateBody> Rates { get; set; } = new List<ExchangeRateBody>();
}