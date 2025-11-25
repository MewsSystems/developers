namespace ExchangeRateUpdater.Application.ExchangeRates.Query.GetExchangeRatesDaily;

using AutoMapper;
using Common.Interfaces;
using Common.Models;
using Domain.Common;
using Domain.Entities;
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
    private readonly ICacheRepository _cacheRepository;
    private readonly IExchangeRateApiClient _exchangeRateApiClient;
    private readonly IMapper _mapper;
    private const string CacheKey = "ExchangeRates";

    public GetExchangesRatesByDateQueryHandler(ICacheRepository cacheRepository,
        IExchangeRateApiClient exchangeRateApiClient,  IMapper mapper)
    {
        Ensure.Argument.NotNull(cacheRepository, nameof(cacheRepository));
        Ensure.Argument.NotNull(exchangeRateApiClient, nameof(exchangeRateApiClient));
        Ensure.Argument.NotNull(mapper, nameof(mapper));
        _cacheRepository = cacheRepository;
        _exchangeRateApiClient = exchangeRateApiClient;
        _mapper = mapper;
    }

    public async ValueTask<List<ExchangeRateDto>> Handle(GetExchangesRatesByDateQuery request,
        CancellationToken cancellationToken)
    {
        var requestedCurrencies = request.CurrencyCodes.Select(currencyCode => new Currency(currencyCode));
        var exchangeRates = _cacheRepository
            .GetFromCache<List<ExchangeRate>>(CacheKey);

        if (!exchangeRates.Any())
        {
            var exchangeRateApiDtos = await _exchangeRateApiClient.GetExchangeRatesAsync(request.Date, request.Language);
            exchangeRates = exchangeRateApiDtos.Select(x => _mapper.Map<ExchangeRate>(x)).ToList();
            _cacheRepository.SetCache(CacheKey, exchangeRates);
        }

        var requestedExchangeRates = exchangeRates
            .Where(rates => requestedCurrencies.Any(currency => currency == rates.SourceCurrency))
            .Select(exchangeRate => _mapper.Map<ExchangeRateDto>(exchangeRate)).ToList();

        return requestedExchangeRates;
    }
}