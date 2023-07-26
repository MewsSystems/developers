using ExchangeRateUpdater.ApiClient.Client;
using ExchangeRateUpdater.ApiClient.Client.ExchangeDaily;
using ExchangeRateUpdater.ApiClient.Common;
using ExchangeRateUpdater.ApiClient.Exceptions;
using ExchangeRateUpdater.Features.Common;
using ExchangeRateUpdater.Features.Exceptions;
using ExchangeRateUpdater.Models.Domain;
using Mews.Caching;
using Microsoft.Extensions.Logging;
namespace ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICnbClient _cnbClient;
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly ICustomCache _cache;

        public ExchangeRateProvider(
             ICustomCacheFactory cacheFactory,
             ICnbClient cnbClient,
             ILogger<ExchangeRateProvider> logger)
        {
            _cache = cacheFactory.GetOrCreate(Constants.CacheName) ?? throw new ArgumentNullException(nameof(cacheFactory));
            _cnbClient = cnbClient ?? throw new ArgumentNullException(nameof(cnbClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        private static string GetCacheKey() => $"EX:{DateTime.UtcNow.ToString(Constants.CACHE_DATE_FORMAT)}";


        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            _logger.LogInformation("Handling in function '{function}' with currencies with Id '{id}'",
                          nameof(ExchangeRateProvider.GetExchangeRates), currencies.Select(elem => "'" + elem.Code + "'"));

            var exchangesResult = await _cache.GetOrAddAsync(GetCacheKey(),
                async () => await ApiCall());

            if (!exchangesResult.HasValue)
                throw new ExchangeRateUpdaterException($"something went wrong in the ExchangesRates");

            var listOfExchangeRate = exchangesResult.Value.Payload.Rates.ToExchangeRate(Constants.CZK).ToList();
            return ExchangeRateCurrenciesIntersection(listOfExchangeRate, currencies);
        }

        private async Task<ExchangeDailyCommand> ApiCall()
        {
            var exchangeResult = await _cnbClient.GetExchangesDaily(DateTime.UtcNow, Language.EN);
            if (!exchangeResult.IsSuccess)
                throw new ApiCnbException($"something went wrong in the ExchangesRates");

            return exchangeResult;

        }

        private IEnumerable<ExchangeRate> ExchangeRateCurrenciesIntersection(
            IEnumerable<ExchangeRate> listOfExchangeRate,
            IEnumerable<Currency> currencies)
        {
            return listOfExchangeRate.Where(x => currencies.Select(y => y.Code)
                                     .Contains(x.TargetCurrency.Code))
                                     .ToList();
        }

    }
}
