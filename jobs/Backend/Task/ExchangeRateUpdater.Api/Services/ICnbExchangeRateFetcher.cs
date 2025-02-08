using ExchangeRateUpdater.Contract.ExchangeRate;
using FuncSharp;

namespace ExchangeRateUpdater.Api.Services;

public enum CnbExchangeRatesFetchError
{
    NoData,
    DataFormat,
    Timeout,
    ServerException,
    NetworkIssues,
    Unknown
}

public interface ICnbExchangeRateFetcher
{
    Task<Try<IEnumerable<CnbExchangeRate>, CnbExchangeRatesFetchError>> FetchExchangeRatesAsync(CancellationToken cancellationToken);
}