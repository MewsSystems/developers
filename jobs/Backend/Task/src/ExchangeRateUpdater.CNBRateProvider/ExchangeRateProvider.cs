using ExchangeRateUpdater.CNBRateProvider.Client;
using ExchangeRateUpdater.Domain.Models;
using FluentResults;

namespace ExchangeRateUpdater.CNBRateProvider;

internal class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICnbClient _cnbClient;
    public ExchangeRateProvider(ICnbClient cnbClient) {
        _cnbClient = cnbClient;
    }

    public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies,
        CancellationToken cancellationToken)
    {
        var exchangeRates = await _cnbClient.GetDailyExchangeRate(DateTime.UtcNow, cancellationToken);

        // TODO: filter out by requested currencies

        return exchangeRates;
    }
}

