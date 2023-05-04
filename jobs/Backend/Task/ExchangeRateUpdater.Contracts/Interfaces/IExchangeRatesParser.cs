namespace ExchangeRateUpdater.Contracts.Interfaces;

public interface IExchangeRatesParser
{
    IEnumerable<ExchangeRate> Parse(string text);
}
