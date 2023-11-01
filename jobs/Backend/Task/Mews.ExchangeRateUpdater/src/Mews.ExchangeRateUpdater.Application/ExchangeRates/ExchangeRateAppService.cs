using AutoMapper;
using Mews.ExchangeRateUpdater.Application.ExchangeRates.Dto;
using Mews.ExchangeRateUpdater.Application.Interfaces;
using Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;

namespace Mews.ExchangeRateUpdater.Application.ExchangeRates;

/// <summary>
/// Exchange rate application service definition.
/// </summary>
public class ExchangeRateAppService : IExchangeRateAppService
{
    private readonly IMapper _mapper;
    private readonly IExchangeRateRepository _exchangeRateRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="exchangeRateRepository"></param>
    public ExchangeRateAppService(IMapper mapper, IExchangeRateRepository exchangeRateRepository)
    {
        _mapper = mapper;
        _exchangeRateRepository = exchangeRateRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRateDto>> GetTodayExchangeRatesAsync(List<Currency>? currencies = null)
    {
        var exchangeRates = await _exchangeRateRepository.GetCachedTodayExchangeRatesAsync();

        if (currencies is not null && currencies.Any())
        {
            exchangeRates = exchangeRates.Where(exchangeRate =>
                currencies.Any(currency => currency.Code == exchangeRate.SourceCurrency.Code));
        }

        return _mapper.Map<IEnumerable<ExchangeRateDto>>(exchangeRates);
    }
}
