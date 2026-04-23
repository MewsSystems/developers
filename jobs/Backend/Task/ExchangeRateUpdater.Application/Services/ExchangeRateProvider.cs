using AutoMapper;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastructure.Interfaces;

namespace ExchangeRateUpdater.Application.Services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private IExchangeRateApi _exchangeRateApi;
    private readonly IMapper _mapper;

    public ExchangeRateProvider(IExchangeRateApi exchangeRateApi, IMapper mapper)
    {
        _exchangeRateApi = exchangeRateApi;
        _mapper = mapper;
    }

    public void SetExchangeRateApi(IExchangeRateApi exchangeRateApi)
    {
        _exchangeRateApi = exchangeRateApi;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateOnly? date, Language language)
    {
        var exchangeRates = await _exchangeRateApi.GetExchangeRatesAsync(date, language);

        return _mapper.Map<IEnumerable<ExchangeRate>>(exchangeRates)
            .Where(rate => currencies.Any(currency => currency.Code == rate.TargetCurrency.Code));
    }
}
