using ExchangeRatesGetterWorkerService.Context;
using ExchangeRatesGetterWorkerService.Models;
using ExchangeRatesGetterWorkerService.Helpers;
using Cronos;
using System.Globalization;

namespace ExchangeRatesGetterWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CnbHelper _cnbHelper;
        private static TimeZoneInfo cestZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
        private static readonly CronExpression _cronDaily = CronExpression.Parse("30 14 * * MON-FRI");
        private static readonly CronExpression _cronMonthly = CronExpression.Parse("0 0 1 * *");


        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _cnbHelper = new CnbHelper(_logger);


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int runIndex = 0;
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime now = DateTime.UtcNow;
                if (runIndex == 0)
                {
                    GetAndWriteMainRates(dbContext, false);
                    GetAndWriteOtherRates(dbContext, false);
                    runIndex++;
                }


                var nextDayUtc = _cronDaily.GetNextOccurrence(now, cestZone);
                var nextMonthUtc = _cronMonthly.GetNextOccurrence(now, cestZone);

                Task dailyTask = Task.Run(() =>
                {
                    Task.Delay(nextDayUtc.Value - now, stoppingToken);
                    GetAndWriteMainRates(dbContext, true);

                }, stoppingToken);

                Task monthlyTask = Task.Run(() => 
                {                    
                    Task.Delay(nextMonthUtc.Value - now, stoppingToken);
                    GetAndWriteOtherRates(dbContext, true);
                }, stoppingToken);


                Task.WaitAny(dailyTask, monthlyTask);

            }
        }

        private void GetAndWriteMainRates(AppDbContext dbContext, bool isFirstRun)
        {
            Rate[] rates = _cnbHelper.GetMainCurrenciesValidRates().Result;
            List<ExchangeRateData> exchangeRatesMain = new List<ExchangeRateData>();

            while(
                rates == null ||
                rates.Length == 0 ||
                (!isFirstRun && DateTime.ParseExact(rates[0].validFor, "yyyy-MM-dd", CultureInfo.InvariantCulture).Day != DateTime.Now.Day)
                )
            {
                Task.Delay(1000);
                rates = _cnbHelper.GetMainCurrenciesValidRates().Result;
            }

            foreach (var rate in rates)
            {
                if (rate.rate != 0)
                {
                    exchangeRatesMain.Add(ExchangeRateData.CreateFromMainCurrencyCnbRate(rate));
                }

            }
            DbHelper.WriteRates(dbContext, exchangeRatesMain.ToArray());
        }
        private void GetAndWriteOtherRates(AppDbContext dbContext, bool isFirstRun)
        {
            Rate[] rates = _cnbHelper.GetOtherCurrenciesValidRates().Result;
            List<ExchangeRateData> exchangeRatesOther = new List<ExchangeRateData>();

            while (
                rates == null ||
                rates.Length == 0 ||
                (!isFirstRun && DateTime.ParseExact(rates[0].validFor, "yyyy-MM-dd", CultureInfo.InvariantCulture).Month != (DateTime.Now.Month-1))
                )
            {
                Task.Delay(1000);
                rates = _cnbHelper.GetOtherCurrenciesValidRates().Result;
            }

            foreach (var otherRate in rates)
            {
                if (otherRate.rate != 0)
                {
                    exchangeRatesOther.Add(ExchangeRateData.CreateFromOtherCurrencyCnbRate(otherRate));
                }
            }

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            DbHelper.WriteRates(dbContext, exchangeRatesOther.ToArray());
        }
        
    }
}