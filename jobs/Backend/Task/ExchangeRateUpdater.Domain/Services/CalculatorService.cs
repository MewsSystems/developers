namespace ExchangeRateUpdater.Domain.Services;

public class CalculatorService : ICalculatorService
{
    public List<ExchangeRate> CalculateRates(List<ExchangeRate> rates)
    {
        ExchangeRateResponse response = new();
        foreach (var rate in rates)
        {
            if (rate.Amount > 1)
            {
                rate.Value /= rate.Amount;
                rate.Amount = 1;
            }
        }
        response.ExchangeRates.AddRange(rates);

        return response.ExchangeRates;
    }
}
