
using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Services.Interfaces;
using ExchangeRateUpdater.Services.Models;
using ExchangeRateUpdater.Services.Models.External;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ExchangeRateUpdater.Jobs
{
    public class ExchangeRateRefreshJob(
        IApiClient<CnbRate> apiClient,
        IExchangeRateCacheService cacheService,
        IDateTimeSource dateTimeSource,
        ILogger<ExchangeRateRefreshJob> logger) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var rates = await apiClient.GetExchangeRatesAsync();
                var exchangeRates = rates
                    .Select(rate => new ExchangeRate(
                        new Currency(rate.CurrencyCode),
                        new Currency("CZK"),
                        rate.Amount == 1 ? rate.Rate : rate.Rate / rate.Amount));

                cacheService.SetRates(exchangeRates);
                logger.LogInformation("Exchange rates succesfully refreshed at [{UTCTime}] UTC", dateTimeSource.UtcNow);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while trying to refresh exchange rates at [{UTCTime}] UTC",
                    dateTimeSource.UtcNow);
            }
        }
    }
}
