namespace Mews.BackendDeveloperTask.ExchangeRates.Cnb;

public interface ICnbTextExchangeRateParser
{
    IEnumerable<ExchangeRate> Parse(string text);
}
