namespace ExchangeRateUpdater.Contracts.Responses;

public class ExchangeRateResponse : Response
{
    public List<ExchangeRate> ExchangeRates { get; set; } = new();
}
