 using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;
using Flurl;
using Serilog;
using Polly;

namespace Adapter.ExchangeRateProvider.CzechNatBank;

/// <summary>
/// Repository responsible to extract exchange rates data from CzechNationalBank.
/// </summary>
public class CzechNationalBankRepository : IExchangeRateProviderRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClientFactory">Instance of <see cref="IHttpClientFactory"/></param>
    /// <param name="logger">Instance of <see cref="ILogger"/></param>
    /// <exception cref="ArgumentNullException">throws when logger or httpClientFactory is null.</exception>
    public CzechNationalBankRepository(IHttpClientFactory? httpClientFactory, ILogger? logger)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get All Fx rates from Czech National Bank.
    /// </summary>
    /// <param name="exhangerRateDate">The date to request fx rates.</param>
    /// <param name="cancellationToken">Instance of <see cref="CancellationToken"/></param>
    /// <returns>Get all fx rates for either the specified or an earlier date.</returns>
    public async Task<IEnumerable<ExchangeRate>> GetAllFxRates(DateTime exhangerRateDate, CancellationToken cancellationToken)
    {
        return await CallCzerchNationalBankApi(async () =>
        {
            var httpClient = CreateClient();

            var response = await httpClient.GetAsync(GetAllExchangeRatesAsTextUrl(exhangerRateDate), cancellationToken);

            using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            using var exchangeRatesTextParser = new ExchangeRatesTextParser(new StreamReader(contentStream), _logger);


            var rawData = await exchangeRatesTextParser.GetDefaultFormattedExchangeRatesAsync(cancellationToken);

            return rawData.Select(dto =>
            {
                var targetCurrency = new Currency("CZK");
                var sourceCurrency = new Currency(dto.CurrencyCode);
                // In case amount is 100 or something else.
                var rate = new PositiveRealNumber(dto.Rate / dto.Amount);
                return new ExchangeRate(sourceCurrency, targetCurrency, rate);
            });
        });
    }

    /// <summary>
    /// Get Exchange Rates for Source/Target Currency and specified date interval.
    /// </summary>
    /// <param name="sourceCurrency">The source currency of fx rates.</param>
    /// <param name="targetCurrency">The target currency of fx rates.</param>
    /// <param name="from">The beginning of search date interval.</param>
    /// <param name="to">The end of search date interval.</param>
    /// <param name="cancellationToken">Instance of <see cref="CancellationToken"/></param>
    /// <returns>Returns the exchange rates for the specified currencies and date interval <see cref="ExchangeRate"/></returns>
    /// <exception cref="NotSupportedException"></exception>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency, 
                                                                                   DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        if (targetCurrency != "CZK") throw new NotSupportedException("Target currencies besides CZK are not yet supported.");


        return await CallCzerchNationalBankApi(async () =>
        {
            var httpClient = CreateClient();

            var response = await httpClient.GetAsync(GetExchangeRateAsTextUrl(from, to, sourceCurrency), cancellationToken);

            using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            using var exchangeRatesTextParser = new ExchangeRatesTextParser(new StreamReader(contentStream), _logger);

            var rawData = await exchangeRatesTextParser.GetDefaultFormattedExchangeRatesForCurrencyAsync(sourceCurrency, cancellationToken);


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

    // To be overriden in tests.
    protected virtual void OnRetry(Exception exception, Context context)
    {
        _logger.Warning(exception, "Retry number {RetryCounter}: {Operation}", context.Count, context.OperationKey);
    }

    private HttpClient CreateClient()
    {
        return _httpClientFactory.CreateClient("ExchangeRateUpdater-http-client");
    }

    // To be overriden in tests.
    protected virtual Url GetAllExchangeRatesAsTextUrl(DateTime date)
    {
        return "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt".SetQueryParam("date", date.Date.ToString("dd.MM.yyyy"));
    }

    // To be overriden in tests.
    protected virtual Url GetExchangeRateAsTextUrl(DateTime from, DateTime to, Currency currency)
    {
        return "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/selected.txt"
            .SetQueryParams(
                new
                {
                    from = from.Date.ToString("dd.MM.yyyy"),
                    to = to.Date.ToString("dd.MM.yyyy"),
                    currency = currency.CurrencyCode,
                    format = "txt"
                }
             );
    }
}
