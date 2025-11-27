namespace ExchangeRateUpdater.Application.ExchangeRates.Query.GetExchangeRatesDaily;

using Common.Interfaces;
using Common.Mappings;
using Common.Utils;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.ValueObjects;
using Dtos;
using Mediator;

/// <summary>
/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
/// some of the currencies, ignore them.
/// </summary>
public class GetExchangesRatesByDateQueryHandler : IQueryHandler<GetExchangesRatesByDateQuery, List<ExchangeRateDto>>
{
    private readonly ICacheRepository _redisRepository;

    public GetExchangesRatesByDateQueryHandler(ICacheRepository redisRepository)
    {
        Ensure.Argument.NotNull(redisRepository, nameof(redisRepository));
        _redisRepository = redisRepository;
    }

    public async ValueTask<List<ExchangeRateDto>> Handle(GetExchangesRatesByDateQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = GetCacheKey(request);
        
        var requestedCurrencies = request.CurrencyCodes.Select(currencyCode => new Currency(currencyCode));
        var exchangeRates = await _redisRepository
            .GetRatesListAsync(cacheKey);

        var requestedExchangeRates = (exchangeRates ?? Enumerable.Empty<ExchangeRate>())
            .Where(rates => requestedCurrencies.Any(currency => currency == rates.SourceCurrency))
            .Select(exchangeRate => exchangeRate.ToDto()).ToList();

        return requestedExchangeRates;
    }

    private string GetCacheKey(GetExchangesRatesByDateQuery request)
    {
        var requestedDate = request.Date ?? DateTime.UtcNow.Date;
        var requestedLanguage = request.Language ?? Language.CZ;
        return CacheKeyHelper.RatesKey(requestedLanguage, requestedDate);
    }
}