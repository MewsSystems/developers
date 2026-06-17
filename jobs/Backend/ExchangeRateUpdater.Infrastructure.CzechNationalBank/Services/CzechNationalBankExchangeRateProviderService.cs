using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.ApiClients;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.ExtensionMethods;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.Services
{
    public class CzechNationalBankExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;
        private readonly IApiClient _apiClient;
        private static readonly Currency TargetCurrency = new("CZK");

        public CzechNationalBankExchangeRateProviderService(ILogger logger, IMemoryCache cache, IApiClient apiClient)
        {
            _logger = logger;
            _cache = cache;
            _apiClient = apiClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<NonNullResponse<Dictionary<string, ExchangeRate>>> GetExchangeRates()
        {
            try
            {
                var cacheKey = DateTime.UtcNow.ToCacheKeyReferenceString();
                var cacheResult =  TryGetExchangesFromCache(cacheKey);
                if(cacheResult.IsSuccess)
                    return NonNullResponse<Dictionary<string, ExchangeRate>>.Success(cacheResult.ExchangeRates);

                var apiResult = await TryGetExchangesFromApi();
                if (apiResult.IsSuccess)
                {
                    _cache.Set(cacheKey, apiResult.ExchangeRates, TimeSpan.FromMinutes(10));
                    return NonNullResponse<Dictionary<string, ExchangeRate>>.Success(apiResult.ExchangeRates);
                }
                return NonNullResponse<Dictionary<string, ExchangeRate>>.Fail(new Dictionary<string, ExchangeRate>(), "We are having issues retrieving exchange rates");
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Error while retrieving exchanges");
                return NonNullResponse<Dictionary<string, ExchangeRate>>.Fail(new Dictionary<string, ExchangeRate>(), exception.Message);
            }
        }

        #region Helper Methods

        private (bool IsSuccess, Dictionary<string, ExchangeRate> ExchangeRates) TryGetExchangesFromCache(string date)
        {
            if (_cache.TryGetValue(date, out Dictionary<string, ExchangeRate>? cachedExchangeRates))
            {
                if (cachedExchangeRates != null)
                    return (true, cachedExchangeRates);
            }
            return (false, new Dictionary<string, ExchangeRate>());
        }

        private async Task<(bool IsSuccess, Dictionary<string,ExchangeRate> ExchangeRates)> TryGetExchangesFromApi()
        {
            var centralBankRatesFromApiResult = await _apiClient.GetCentralBankRates(DateTime.UtcNow.ToCzechNationalBankExchangeNowString());
            var otherCurrenciesRatesFromApiResult = await _apiClient.GetOtherCurrenciesRates(DateTime.UtcNow.ToOtherCurrenciesExchangeNowString());

            if (!centralBankRatesFromApiResult.IsSuccess || !otherCurrenciesRatesFromApiResult.IsSuccess)
            {
                if (!centralBankRatesFromApiResult.IsSuccess && !otherCurrenciesRatesFromApiResult.IsSuccess)
                {
                    _logger.Error(
                        "There is a problem retrieving rates, daily FX result {@centralBankRates}, other FX {@otherCurrencyRates}",
                        centralBankRatesFromApiResult, otherCurrenciesRatesFromApiResult);
                    return (false, new Dictionary<string, ExchangeRate>());
                }
                if (!centralBankRatesFromApiResult.IsSuccess)
                    _logger.Error("There is a problem retrieving rates, daily FX result {@centralBankRates}", centralBankRatesFromApiResult);
                if (!otherCurrenciesRatesFromApiResult.IsSuccess)
                    _logger.Error("There is a problem retrieving rates, other FX result {@otherCurrencyRates}", otherCurrenciesRatesFromApiResult);
            }

            return (true,MapResultsToDictionary(centralBankRatesFromApiResult.Content,otherCurrenciesRatesFromApiResult.Content));
        }      

        private static Dictionary<string, ExchangeRate> MapResultsToDictionary(IEnumerable<RateDto> centralBankRates, IEnumerable<RateDto> otherCurrenciesRates)
        {
            var exchangeRates = new Dictionary<string, ExchangeRate>();

            foreach (var centralBankRate in centralBankRates)
            {
                exchangeRates.Add(centralBankRate.CurrencyCode, new ExchangeRate(new Currency(centralBankRate.CurrencyCode), TargetCurrency, centralBankRate.Rate));
            }
            foreach (var otherCurrencyRate in otherCurrenciesRates)
            {
                exchangeRates.Add(otherCurrencyRate.CurrencyCode, new ExchangeRate(new Currency(otherCurrencyRate.CurrencyCode), TargetCurrency, otherCurrencyRate.Rate));
            }
            return exchangeRates;
        }

        #endregion
    }
}
