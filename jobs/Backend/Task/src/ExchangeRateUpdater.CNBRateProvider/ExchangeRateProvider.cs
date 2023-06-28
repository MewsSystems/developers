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

    public async Task<Result<IReadOnlyCollection<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies,
        CancellationToken cancellationToken)
    {
        var exchangeRatesResult = await _cnbClient.GetDailyExchangeRateToCzk(DateTime.UtcNow, cancellationToken);

        if (exchangeRatesResult.IsFailed)
        {
            return Result.Fail(exchangeRatesResult.Errors);
        }

        var exchangeRates = exchangeRatesResult.Value;
        var filteredRates = new List<ExchangeRate>();
        foreach (var currency in currencies)
        {
            var matchingRates = exchangeRates.Where(rate => rate.SourceCurrency.Equals(currency) || rate.TargetCurrency.Equals(currency));
            // TODO: avoid duplicates
            if (matchingRates is not null)
            {
                filteredRates.AddRange(matchingRates);
            }
        }

        return Result.Ok<IReadOnlyCollection<ExchangeRate>>(filteredRates);
    }
}

