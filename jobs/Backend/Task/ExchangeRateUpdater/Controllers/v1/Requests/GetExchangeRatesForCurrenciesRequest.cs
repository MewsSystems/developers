namespace ExchangeRateUpdater.Controllers.v1.Requests;

public class GetExchangeRatesForCurrenciesRequest
{
    public string[] Currencies { get; init; }
}