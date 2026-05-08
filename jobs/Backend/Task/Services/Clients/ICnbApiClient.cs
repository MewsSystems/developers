using FluentResults;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Clients;

public interface ICnbApiClient
{
    Task<Result<string>> GetExchangeRateDataAsync(CancellationToken cancellationToken = default);
}
