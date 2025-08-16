namespace ExchangeRateUpdater.Contracts;

public interface IExchangeRateProvider
{
    /// <summary>
    ///     Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    ///     by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    ///     do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    ///     some of the currencies, ignore them.
    /// </summary>
    /// <param name="currencies">The list of currencies</param>
    /// <returns>The list of exchange rates related to the input currencies</returns>
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);

    /// <summary>
    ///     Async version of the <see cref="GetExchangeRates" /> method
    /// </summary>
    /// <param name="currencies">The list of currencies</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The list of exchange rates related to the input currencies</returns>
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default);
}