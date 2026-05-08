using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.DataSource.Cnb.Dto;
using ExchangeRateUpdater.DataSources;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.Config;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.DataSource.Cnb;

internal class CnbExchangeRateLoader : IExchangeRateLoader
{
    private const string DailyExchangeRateUrlPath = "/cnbapi/exrates/daily?date={0}&lang=EN";
    private readonly IRequestHandler requestHandler;
    private readonly CnbRateConverter converter;
    private readonly ILogger<CnbExchangeRateLoader> logger;
    private readonly IDateTimeService dateTimeService;
    private readonly Uri baseApiUri;
    private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    public CnbExchangeRateLoader(IRequestHandler requestHandler,
    CnbRateConverter converter,
    ILogger<CnbExchangeRateLoader> logger,
    IRefreshScheduleFactory refreshScheduleFactory,
    IDateTimeService dateTimeService,
    [FromKeyedServices(Constants.CnbServiceKey)] ExchangeRateLoaderConfig config)
    {
        this.requestHandler = requestHandler;
        this.converter = converter;
        this.logger = logger;
        this.dateTimeService = dateTimeService;
        baseApiUri = new Uri(config.BaseApiUrl);
        RateRefreshSchedule = refreshScheduleFactory.CreateRefreshSchedule(config.RefreshScheduleConfig);
        logger.LogInformation("Initialized data source with URL {} and refresh schedule: {}", baseApiUri, RateRefreshSchedule);
    }

    public IRateRefreshScheduler RateRefreshSchedule { get; }

    // discussable: can consider adding "retry" logic inside this method. In this case the method will need CancellationToken parameter to stop pending retries.
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<string> currencies, DateTime date)
    {
        ArgumentNullException.ThrowIfNull(currencies);

        if (date >= dateTimeService.GetToday().AddDays(1))
            throw new ArgumentException("The date should not be in the future", nameof(date));

        if (!currencies.Any())
        {
            return [];
        }

        try
        {
            var dateParam = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var requestUri = new Uri(baseApiUri, string.Format(DailyExchangeRateUrlPath, dateParam)).ToString();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using var stream = await requestHandler.GetStreamAsync(requestUri);

            CnbRateResponse response = await JsonSerializer.DeserializeAsync<CnbRateResponse>(stream, serializerOptions);

            stopwatch.Stop(); // Further the elapsed time can be put into metrics
            logger.LogInformation("Loaded {} rates in {} ms", response.Rates.Length, stopwatch.ElapsedMilliseconds);

            var requestedCurrencies = new HashSet<string>(currencies);
            var result = response.Rates
                .Where(r => requestedCurrencies.Contains(r.CurrencyCode))
                .Select(converter.Convert)
                .Where(r => r != null)
                .ToList();

            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new RateLoaderException($"API responded with error status code: {ex.StatusCode}", ex);
        }
        catch (Exception ex)
        {
            throw new RateLoaderException("Unable load exchange rates from CNB", ex);
        }
    }
}
