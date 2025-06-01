namespace Services;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Takes a list of <see cref="Currency"/> and returns a list of <see cref="ExchangeRate"/> for the current UTC date.
    /// </summary>
    /// <param name="currencies">A list of <see cref="Currency"/> to get the rate to</param>
    /// <param name="ct">An optional <see cref="CancellationToken"/> to cancel the request</param>
    /// <returns>A list a ExchangeRate</returns>
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken ct = default);
}
