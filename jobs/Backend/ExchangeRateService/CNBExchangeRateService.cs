namespace ExchangeRateService
{
    using Caching;
    using CurrencyExchangeService;
    using CurrencyExchangeService.Extentions;
    using CurrencyExchangeService.Interfaces;
    using CurrencyExchangeService.Models;
    using ExchangeRateService.Models;
    using Logger;

    public class CNBExchangeRateService : IExchangeRateService<ExchangeRate, Currency>
    {
        private readonly IExchangeRateProvider<string> _provider;
        private readonly ISerializationHelper<CurrencyRateXmlResponse> _serilizator;
        private readonly ILogger _logger;
        private readonly ICacheService<string, string> _cache;

        public CNBExchangeRateService(IExchangeRateProvider<string> provider,
            ISerializationHelper<CurrencyRateXmlResponse> serilizator, ILogger logger,
            ICacheService<string, string> cache)
        {
            this._provider = provider;
            this._serilizator = serilizator;
            this._logger = logger;
            this._cache = cache;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            var stringResponse = String.Empty;

            // Currency codes could be added to anum in the future
            // Current API return only CZK to other currencies rates, so if no CZK in the list, wont be any results
            if (currencies == null || !currencies.Any() || currencies.Select(x => x.Code == Constants.CZKCurrencyCode) == null)
            {
                return exchangeRates;
            }

            // Remove duplicates
            currencies = currencies.Distinct().ToList();

            try
            {
                stringResponse = this._cache.Get(Constants.ApiResponseCacheKey);
            }
            catch (Exception ex)
            {
                this._logger.Error($"CNBExchangeRateProvider:GetExchangeRates: Failed to get cache with error {ex.Message}");
            }

            if (String.IsNullOrWhiteSpace(stringResponse))
            {
                this._logger.Info($"CNBExchangeRateProvider:GetExchangeRates: Cache value is empty");
                stringResponse = await _provider.GetExchangeRates();

                // Based on https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/
                // CNB update curencies ones a day at 14:30
                this._cache.Add(Constants.ApiResponseCacheKey, stringResponse, DateTime.Now.TimeToHourMinuteSecond(14, 30, 00));
            }
            else
            {
                this._logger.Info($"CNBExchangeRateProvider:GetExchangeRates: Get responce from cache {stringResponse}");
            }

            var response = this._serilizator.Deserialize(stringResponse);

            foreach (var currency in currencies)
            {
                // For CNB api we are using CZK will always be sourse currency
                if (currency.Code == Constants.CZKCurrencyCode)
                {
                    continue;
                }

                try
                {
                    var bankCurrency = response.Currencies.CurrencyItem.First(x => x.Code == currency.Code);

                    if (bankCurrency != null)
                    {
                        exchangeRates.Add(new ExchangeRate(new Currency(Constants.CZKCurrencyCode), new Currency(currency.Code), bankCurrency.Rate));
                    }
                }
                catch (Exception ex)
                {
                    this._logger.Error($"CNBExchangeRateService:GetExchangeRatesAsync: Cant get currency {currency.Code} from response, failde with {ex.Message}");
                    continue;
                }
            }

            return exchangeRates;
        }
    }
}
