using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.Interface.ExternalAPIs;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class GetExchangeRatesQuery
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetExchangeRatesQuery(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<IEnumerable<ExchangeRate>> Execute(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        var rates = await _exchangeRateProvider.GetExchangeRates(currencies, cancellationToken);
        return rates;
    }
}