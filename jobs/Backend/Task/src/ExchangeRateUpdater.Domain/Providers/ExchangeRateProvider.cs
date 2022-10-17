using AutoMapper;
using ExchangeRateUpdater.Clients.Cnb;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Domain.Caches;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Options;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Domain.Providers;

public class ExchangeRateProvider
{
    private readonly ICnbClient _client;
    private readonly IMapper _mapper;
    private readonly IExchangeRateCache _cache;
    private readonly ApplicationOptions _applicationOptions;

    public ExchangeRateProvider(ICnbClient client, IMapper mapper, IExchangeRateCache cache, IOptions<ApplicationOptions> applicationOptions)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _applicationOptions = applicationOptions.Value ?? throw new ArgumentNullException(nameof(applicationOptions));
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        ExchangeRateResponse? response;
        IEnumerable<ExchangeRate>? exchangeRates;

        if (!_applicationOptions.EnableInMemoryCache)
        {
            response = await _client.GetExchangeRatesAsync();
            exchangeRates = _mapper.Map<IEnumerable<ExchangeRate>>(response);
        }
        else
        {
            exchangeRates = _cache.Get();

            if (exchangeRates == null)
            {
                response = await _client.GetExchangeRatesAsync();
                exchangeRates = _mapper.Map<IEnumerable<ExchangeRate>>(response);
                _cache.Set(response.CurrentDate, exchangeRates);
            }
        }

        return exchangeRates.Where(r => currencies.Any(c => c.Code == r.SourceCurrency.Code));
    }
}