using ExchangeRateUpdater.Domain.Models;
using FluentResults;

namespace ExchangeRateUpdater.CNBRateProvider.Client;

internal class CnbClient : ICnbClient
{
    public Task<Result<IEnumerable<ExchangeRate>>> GetDailyExchangeRate(DateTime dateTime) => throw new NotImplementedException();
}
