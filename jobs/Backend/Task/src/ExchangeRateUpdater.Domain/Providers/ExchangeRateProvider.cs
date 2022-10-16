using AutoMapper;
using ExchangeRateUpdater.Clients.Cnb;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Providers;

public class ExchangeRateProvider
{
    private readonly ICnbClient _cnbClient;
    private readonly IMapper _mapper;

    public ExchangeRateProvider(ICnbClient cnbClient, IMapper mapper)
    {
        _cnbClient = cnbClient ?? throw new ArgumentNullException(nameof(cnbClient));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var response = await _cnbClient.GetExchangeRatesAsync();
        var exchangeRates = _mapper.Map<IEnumerable<ExchangeRate>>(response);
        return exchangeRates.Where(r => currencies.Any(c => c.Code == r.SourceCurrency.Code));
    }
}