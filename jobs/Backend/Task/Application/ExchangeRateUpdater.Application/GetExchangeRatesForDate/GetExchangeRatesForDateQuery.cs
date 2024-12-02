using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.Interface.ExternalAPIs;

namespace ExchangeRateUpdater.Application.GetExchangeRatesForDate;

public class GetExchangeRatesForDateQuery
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetExchangeRatesForDateQuery(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<IEnumerable<ExchangeRate>> Execute(DateTime date, IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        var rates = await _exchangeRateProvider.GetExchangeRates(date, currencies, cancellationToken);
        return rates;
    }
}