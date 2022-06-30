using System.Globalization;
using ExchangeRate.Provider.Base.Interfaces;
using ExchangeRate.Provider.Cnb.Factory;
using ExchangeRate.Provider.Cnb.Interfaces;
using ExchangeRate.Provider.Cnb.Models.Configuration;
using ExchangeRate.Service.Enums;
using ExchangeRate.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ExchangeRate.Service;

public class ExchangeRateService : IExchangeRateService
{
    #region Fields

    private readonly ICnbHttpClient _cnbHttpClient;
    private readonly IOptions<CnbProviderConfiguration> _configuration;
    private readonly bool _isCacheEnabled;
    private readonly IMemoryCache _memoryCache;
    private readonly string _timeZoneId;

    #endregion

    #region Constructors

    public ExchangeRateService(ICnbHttpClient cnbHttpClient, IMemoryCache memoryCache, IOptions<CnbProviderConfiguration> configuration)
    {
        configuration.Value.Validate();

        _cnbHttpClient = cnbHttpClient;
        _memoryCache = memoryCache;
        _configuration = configuration;

        _isCacheEnabled = _configuration.Value.Cache is not null &&
                          _configuration.Value.Cache.IsEnabled.HasValue &&
                          _configuration.Value.Cache.IsEnabled.Value;

        _timeZoneId = _configuration.Value.Cache?.TimeZoneId ?? "Central Europe Standard Time";
    }

    #endregion

    #region Properties

    private string CurrentCacheKey => CacheKeys.ExchangeRateKey(TimeZoneInfo.ConvertTime(DateTime.UtcNow,
        TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId)).Date.ToString(CultureInfo.CurrentCulture));

    #endregion

    public async Task<IEnumerable<Models.ExchangeRate>> GetExchangeRates(ProviderSource providerSource)
    {
        // Check for cached record for today
        if (_isCacheEnabled && _memoryCache.TryGetValue<IEnumerable<Models.ExchangeRate>>(CurrentCacheKey, out var cachedExchangeRates))
            return cachedExchangeRates;

        var result = (await GetProvider(providerSource).GetExchangeRates())
            .Where(x => _configuration.Value.GetCurrencies().Select(currency => currency.Code).Contains(x.SourceCurrency.Code));

        if (_isCacheEnabled)
            _memoryCache.Set(CurrentCacheKey, result);

        return result;
    }

    private IExchangeRateProvider GetProvider(ProviderSource providerSource)
    {
        var creator = providerSource switch
        {
            ProviderSource.Cnb => new CnbProviderCreator(_cnbHttpClient, _configuration),
            _                  => throw new ArgumentOutOfRangeException($"{nameof(ProviderSource)} is out of range")
        };

        return creator.CreateProvider();
    }
}