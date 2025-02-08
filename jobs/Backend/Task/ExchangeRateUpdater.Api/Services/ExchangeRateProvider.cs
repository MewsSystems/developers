using ExchangeRateUpdater.Contract;
using ExchangeRateUpdater.Contract.ExchangeRate;
using ExchangeRateUpdater.Lib.Collection;
using FuncSharp;

namespace ExchangeRateUpdater.Api.Services;

public enum GetExchangeRatesError
{
    DataIssues,
    ServiceUnavailable,
    Unknown
}

public interface IExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    Task<Try<IEnumerable<ExchangeRate>, GetExchangeRatesError>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken
    );
}

public sealed class ExchangeRateProvider(ICnbExchangeRateFetcher cnbExchangeRateFetcher) : IExchangeRateProvider
{
    public async Task<Try<IEnumerable<ExchangeRate>, GetExchangeRatesError>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken
    )
    {
        var currenciesSet = currencies.ToHashSet();

        if (currenciesSet.IsNullOrEmpty()) return Try.Success<IEnumerable<ExchangeRate>, GetExchangeRatesError>([]);

        var maybeCnbRates = await cnbExchangeRateFetcher.FetchExchangeRatesAsync(cancellationToken);

        return maybeCnbRates
            .Map(
                rates => rates.Where(r => currenciesSet.Contains(r.CurrencyCode)).Select(Map),
                error => error switch
                {
                    CnbExchangeRatesFetchError.NoData or CnbExchangeRatesFetchError.DataFormat => GetExchangeRatesError
                        .DataIssues,
                    CnbExchangeRatesFetchError.Timeout or CnbExchangeRatesFetchError.NetworkIssues
                        or CnbExchangeRatesFetchError.ServerException => GetExchangeRatesError.ServiceUnavailable,
                    CnbExchangeRatesFetchError.Unknown => GetExchangeRatesError.Unknown,
                    _ => throw new ArgumentOutOfRangeException(nameof(error), error,
                        "There is no mapping from the fetch error to the provider error.")
                });
    }

    private static ExchangeRate Map(CnbExchangeRate rate)
    {
        var value = rate.Rate / rate.Amount;

        return new ExchangeRate(rate.CurrencyCode, Currency.Czk, value);
    }
}