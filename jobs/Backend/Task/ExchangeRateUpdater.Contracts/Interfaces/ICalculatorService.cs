namespace ExchangeRateUpdater.Contracts.Interfaces;

public interface ICalculatorService
{
    List<ExchangeRate> CalculateRates(List<ExchangeRate> rates);
}
