namespace ExchangeRateService.Client.Model.CNB;

public class ExratesDailyResponse
{
    public IEnumerable<ExchangeRateBody> Rates { get; set; }
}