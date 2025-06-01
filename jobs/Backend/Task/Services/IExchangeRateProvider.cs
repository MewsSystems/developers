namespace Services;

public interface IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken ct = default);
}