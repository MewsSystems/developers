using AutoMapper;
using ExchangeRateUpdater.ApplicationServices.ExchangeRates.Dto;
using ExchangeRateUpdater.ApplicationServices.Interfaces;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.ApplicationServices.ExchangeRates;

/// <summary>
/// Exchange rate application service.
/// The service layer intermediates between internal model and exposed API model.
/// 
/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
/// some of the currencies, ignore them.
/// </summary>
public class ExchangeRateService : IExchangeRateService
{
    private readonly IMapper _mapper;
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public ExchangeRateService(IMapper mapper, IExchangeRateRepository exchangeRateRepository)
    {
        _mapper = mapper;
        _exchangeRateRepository = exchangeRateRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime date)
    {
        var exchangeRates = (await _exchangeRateRepository.GetExchangeRatesAsync(date)).Where(e => currencies.Any(c => c.Code == e.SourceCurrency.Code));

        return _mapper.Map<IEnumerable<ExchangeRateDto>>(exchangeRates);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(DateTime date)
    {
        var exchangeRates = await _exchangeRateRepository.GetExchangeRatesAsync(date);
        return _mapper.Map<IEnumerable<ExchangeRateDto>>(exchangeRates);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRateDto>> GetTodayExchangeRatesAsync(IEnumerable<Currency>? currencies = null)
    {
        var exchangeRates = await _exchangeRateRepository.GetTodayExchangeRatesAsync();

        exchangeRates = currencies != null && currencies.Any() ?
                exchangeRates.Where(e => currencies.Any(c => c.Code == e.SourceCurrency.Code)) :
                exchangeRates;

        return _mapper.Map<IEnumerable<ExchangeRateDto>>(exchangeRates);
    }
}
