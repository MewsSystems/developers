using ExchangeRateUpdater.Domain.Models;
using FluentResults;

namespace ExchangeRateUpdater.CNBRateProvider.Client;

internal interface ICnbClient
{
    Task<Result<IEnumerable<ExchangeRate>>> GetDailyExchangeRateToCzk(DateTime dateTime, CancellationToken cancellationToken);
}
