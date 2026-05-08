using ExchangeRateProvider.Application.Abstractions;
using ExchangeRateProvider.Application.DTOs;
using ExchangeRateProvider.Application.Queries;
using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Domain.ValueObjects;

namespace ExchangeRateProvider.Application.Handlers;

public class GetExchangeRatesQueryHandler(IExchangeRateServiceFactory serviceFactory) : IQueryHandler<GetExchangeRatesQuery, IList<ExchangeRateDto>>
{
    public async Task<IList<ExchangeRateDto>> HandleAsync(GetExchangeRatesQuery query, CancellationToken cancellationToken = default)
    {
        var exchangeRateService = serviceFactory.GetService(query.BaseCurrency);

        var rates = await exchangeRateService.GetExchangeRatesAsync(query.BaseCurrency, cancellationToken);

        IEnumerable<ExchangeRate> filteredRates = rates;

        if (query.QuoteCurrencies is not null && query.QuoteCurrencies.Count > 0)
        {
            var quoteCodes = query.QuoteCurrencies.Select(c => c.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
            filteredRates = rates.Where(r => quoteCodes.Contains(r.QuoteCurrency.Code));
        }

        return filteredRates.Select(r => new ExchangeRateDto(r.BaseCurrency, r.QuoteCurrency, r.Rate)).ToList();
    }
}
