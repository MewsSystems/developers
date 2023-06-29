using ExchangeRateUpdater.CNBRateProvider.Client;
using ExchangeRateUpdater.Domain.Models;
using FluentResults;

namespace ExchangeRateUpdater.CNBRateProvider;

internal class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICnbClient _cnbClient;

    public ExchangeRateProvider(ICnbClient cnbClient)
    {
        _cnbClient = cnbClient;
    }

    public async Task<Result<IReadOnlyCollection<ExchangeRate>>> GetExchangeRates(
        IEnumerable<CurrencyPair> currencies,
        CancellationToken cancellationToken)
    {
        var exchangeRatesResult = await _cnbClient.GetDailyExchangeRateToCzk(DateTime.UtcNow, cancellationToken);

        if (exchangeRatesResult.IsFailed)
        {
            return Result.Fail(exchangeRatesResult.Errors);
        }

        var filteredRates = FilterExchangeRates(currencies, exchangeRatesResult.Value);

        return Result.Ok<IReadOnlyCollection<ExchangeRate>>(filteredRates);
    }

    private static List<ExchangeRate> FilterExchangeRates(IEnumerable<CurrencyPair> currencies, IEnumerable<ExchangeRate> exchangeRates)
    {
        var filteredRates = new List<ExchangeRate>();
        foreach (var currency in currencies)
        {
            var matchingRates = exchangeRates.Where(rate => rate.CurrencyPair.Equals(currency));
            if (matchingRates is not null)
            {
                filteredRates.AddRange(matchingRates);
            }
        }

        return filteredRates;
    }
}

