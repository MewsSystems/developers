using ExchangeRateUpdater.Domain.Model;
using ExchangeRateUpdater.Interface.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Implementation.Services
{
    public class ExchangeRatesCacheService : IExchangeRatesCacheService
    {
        private ILogger<IExchangeRatesCacheService> _logger;
        private readonly ICnbApiService _cnbApiService;
        private readonly IMemoryCache _cache;

        public ExchangeRatesCacheService(ILogger<IExchangeRatesCacheService> logger, ICnbApiService cnbApiService, IMemoryCache cache)
        {
            _logger = logger;
            _cnbApiService = cnbApiService;
            _cache = cache;
        }

        public async Task<IEnumerable<ExchangeRateEntity>?> GetOrCreateExchangeRatesAsync()
        {
            var cacheKey = DateTime.UtcNow.ToShortDateString();
            var expirationPolicy = DateTimeOffset.Now.Date.AddDays(1);

            _logger.LogInformation("Getting exchange rates information from cache if already exists data retrieved today.");

            return await _cache.GetOrCreateAsync(cacheKey, entry => 
            { 
                entry.AbsoluteExpiration = expirationPolicy;
                return _cnbApiService.GetExchangeRates();
            });
        }
    }
}
