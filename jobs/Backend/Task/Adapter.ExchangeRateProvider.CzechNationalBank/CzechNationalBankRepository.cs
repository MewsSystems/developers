using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;
using Flurl;
using Serilog;
using Polly;
using System.Reflection;
using System;

namespace Adapter.ExchangeRateProvider.CzechNationalBank;

public class CzechNationalBankRepository : IExchangeRateProviderRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public CzechNationalBankRepository(IHttpClientFactory? httpClientFactory, ILogger? logger)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<ExchangeRate>> GetDefaultUnitRates()
    {
        return await CallCzerchNationalBankApi( async () =>
        {
            var httpClient = CreateClient();

            var response = await httpClient.GetAsync(GetAllExchangeRatesAsTextUrl(DateTime.Now));

            using var contentStream = await response.Content.ReadAsStreamAsync();

            using var exchangeRatesTextParser = new ExchangeRatesTextParser(new StreamReader(contentStream), _logger);


            var rawData = await exchangeRatesTextParser.GetDefaultFormattedExchangeRatesAsync();

            return rawData.Select(dto => {
                var targetCurrency = new Currency("CZK");
                var sourceCurrency = new Currency(dto.CurrencyCode);
                // In case amount is 100 or something else.
                var rate = new PositiveRealNumber(dto.Rate / dto.Amount);
                return new ExchangeRate(sourceCurrency, targetCurrency, rate);
            });
        });
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency, DateTime from, DateTime to)
    {
        if (targetCurrency != "CZK") throw new NotSupportedException("Target currencies besides CZK are not yet supported.");


        return await CallCzerchNationalBankApi(async () =>
        {
            var httpClient = CreateClient();

            var response = await httpClient.GetAsync(GetExchangeRateAsTextUrl(from, to, sourceCurrency));

            using var contentStream = await response.Content.ReadAsStreamAsync();

            using var exchangeRatesTextParser = new ExchangeRatesTextParser(new StreamReader(contentStream), _logger);

            var rawData = await exchangeRatesTextParser.GetDefaultFormattedExchangeRatesForCurrencyAsync(sourceCurrency);


            return rawData.OrderBy(data => data.DateTime).Select(dto =>
            {
                var targetCurrency = new Currency("CZK");
                var sourceCurrency = new Currency(dto.CurrencyCode);
                // In case amount is 100 or something else.
                var rate = new PositiveRealNumber(dto.Rate / dto.Amount);
                return new ExchangeRate(sourceCurrency, targetCurrency, rate);
            });
        });
    }

    // Internal just for tests
    internal async Task<T> CallCzerchNationalBankApi<T>(Func<Task<T>> action)
    {
        return await GetRetryPolicy().ExecuteAsync(action.Invoke);
    }

    

    // To be overriden in tests.
    protected virtual TimeSpan[] GetRetrySleepTimes()
    {
        return new TimeSpan[]
        {
            TimeSpan.FromSeconds(30),
            TimeSpan.FromSeconds(60)
        };
    }


    private AsyncPolicy GetRetryPolicy()
    {
        return Policy.Handle<HttpRequestException>()
                     .Or<FormatException>()
                     .WaitAndRetryAsync(GetRetrySleepTimes(), (exception, _, context) =>
                     {
                         OnRetry(exception, context);
                     });
    }

    protected virtual void OnRetry(Exception exception, Context context)
    {
        _logger.Warning(exception, "Retry number {RetryCounter}: {Operation}", context.Count, context.OperationKey);
    }

    private HttpClient CreateClient()
    {
        return _httpClientFactory.CreateClient("ExchangeRateUpdater-http-client");
    }

    private Url GetAllExchangeRatesAsTextUrl(DateTime date)
    {
        return "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt".SetQueryParam("date", date.Date);
    }

    private Url GetExchangeRateAsTextUrl(DateTime from, DateTime to, Currency currency)
    {
        return "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/selected.txt"
            .SetQueryParams(
                new 
                { 
                    from = from.Date.ToString("dd.MM.yyyy"),  
                    to=to.Date.ToString("dd.MM.yyyy"), 
                    currency=currency.CurrencyCode, 
                    format="txt"
                }
             );
    }
}
