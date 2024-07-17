namespace ExchangeRateUpdater.Models;

public class DailyExRatesResponse
{
    public List<DailyExRateItem> Rates { get; set; } = new();
}
