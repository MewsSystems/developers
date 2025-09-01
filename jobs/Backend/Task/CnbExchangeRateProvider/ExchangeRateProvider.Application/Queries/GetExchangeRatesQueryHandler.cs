using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Domain.Interfaces;
using MediatR;

namespace ExchangeRateProvider.Application.Queries;

/// <summary>
/// Query handler for getting exchange rates.
/// Uses the registered exchange rate provider service.
/// </summary>
public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, IEnumerable<ExchangeRate>>
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetExchangeRatesQueryHandler(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider ?? throw new ArgumentNullException(nameof(exchangeRateProvider));
    }

    public async Task<IEnumerable<ExchangeRate>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        if (request.Currencies == null || !request.Currencies.Any())
        {
            return [];
        }

        var rates = await _exchangeRateProvider.GetExchangeRatesAsync(request.Currencies, cancellationToken);

        // Filter to only requested currencies
        return rates.Where(r => request.Currencies.Any(c => c.Code == r.SourceCurrency.Code));
    }
}

