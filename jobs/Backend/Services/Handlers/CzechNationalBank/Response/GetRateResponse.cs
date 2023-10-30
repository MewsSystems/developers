using Data;

namespace Services.Handlers.CzechNationalBank.Response;

public class GetRateResponse
{
    public GetRateResponse(List<ExchangeRate> exchangeRates)
    {
        ExchangeRates = exchangeRates;
    }

    public List<ExchangeRate> ExchangeRates { get; }
}
