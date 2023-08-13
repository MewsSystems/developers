
using ExchangeRatesGetterWorkerService.Context;
using ExchangeRatesGetterWorkerService.Models;
using ExchangeRatesGetterWorkerService.Helpers;
using Cronos;
using System.Globalization;

namespace ExchangeRatesGetterWorkerService
{
    /// <summary>   A worker. </summary>
    ///
    /// <remarks>   , 13.08.2023. </remarks>

    public class Worker : BackgroundService
    {
        /// <summary>   (Immutable) the logger. </summary>
        private readonly ILogger<Worker> _logger;
        /// <summary>   (Immutable) the service scope factory. </summary>
        private readonly IServiceScopeFactory _serviceScopeFactory;
        /// <summary>   (Immutable) the cnb helper. </summary>
        private readonly CnbHelper _cnbHelper;
        /// <summary>   The cest zone. </summary>
        private static TimeZoneInfo cestZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
        /// <summary>   (Immutable) the cron daily. </summary>
        private static readonly CronExpression _cronDaily = CronExpression.Parse("30 14 * * MON-FRI");
        /// <summary>   (Immutable) the cron monthly. </summary>
        private static readonly CronExpression _cronMonthly = CronExpression.Parse("0 0 1 * *");

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   , 13.08.2023. </remarks>
        ///
        /// <param name="logger">               The logger. </param>
        /// <param name="serviceScopeFactory">  The service scope factory. </param>

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _cnbHelper = new CnbHelper(_logger);
            DbHelper.Init(_logger);

        }

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" />
        /// starts. The implementation should return a task that represents the lifetime of the long
        /// running operation(s) being performed.
        /// </summary>
        ///
        /// <remarks>   , 13.08.2023. </remarks>
        ///
        /// <param name="stoppingToken">    Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" />
        ///                                 is called. </param>
        ///
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.
        /// </returns>

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            DbHelper.ClearTable(dbContext);

            ActualizeMainRates(dbContext, true);
            ActualizeOtherRates(dbContext, true);


            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                DateTime now = DateTime.UtcNow;



                var nextDayUtc = _cronDaily.GetNextOccurrence(now, cestZone);
                var nextMonthUtc = _cronMonthly.GetNextOccurrence(now, cestZone);

                Task dailyTask = Task.Run(async () =>
                {
                    await Task.Delay(nextDayUtc.Value - now, stoppingToken);
                    ActualizeMainRates(dbContext, false);

                }, stoppingToken);

                Task monthlyTask = Task.Run(async () => 
                {                    
                    await Task.Delay(nextMonthUtc.Value - now, stoppingToken);
                    ActualizeOtherRates(dbContext, false);
                }, stoppingToken);


                Task.WaitAny(dailyTask, monthlyTask);

            }
        }

        /// <summary>   Actualize main rates. </summary>
        ///
        /// <remarks>   , 13.08.2023. </remarks>
        ///
        /// <param name="dbContext">    Context for the database. </param>
        /// <param name="isFirstRun">   True if is first run, false if not. </param>

        private void ActualizeMainRates(AppDbContext dbContext, bool isFirstRun)
        {
            Rate[] rates = _cnbHelper.GetMainCurrenciesValidRates().Result;
            List<ExchangeRateData> exchangeRatesMain = new List<ExchangeRateData>();

            while(
                rates == null ||
                rates.Length == 0 ||
                // This check on scheduled run (not first run) is perfomed to make sure that CNB has published desired rates, if not, than wait one second
                (!isFirstRun && DateTime.ParseExact(rates[0].validFor, "yyyy-MM-dd", CultureInfo.InvariantCulture).Day != DateTime.Now.Day)
                )
            {
                Task.Delay(1000);
                rates = _cnbHelper.GetMainCurrenciesValidRates().Result;
            }

            foreach (var rate in rates)
            {
                if (rate.rate != 0 && rate.amount == 1)
                {
                    exchangeRatesMain.Add(ExchangeRateData.CreateFromMainCurrencyCnbRate(rate));
                }

            }

            if(!isFirstRun)
            {
                DbHelper.CleanupRates(dbContext, true);
            }
            
            DbHelper.WriteRates(dbContext, exchangeRatesMain.ToArray());
        }

        /// <summary>   Actualize other rates. </summary>
        ///
        /// <remarks>   , 13.08.2023. </remarks>
        ///
        /// <param name="dbContext">    Context for the database. </param>
        /// <param name="isFirstRun">   True if is first run, false if not. </param>

        private void ActualizeOtherRates(AppDbContext dbContext, bool isFirstRun)
        {
            Rate[] rates = _cnbHelper.GetOtherCurrenciesValidRates().Result;
            List<ExchangeRateData> exchangeRatesOther = new List<ExchangeRateData>();

            while (
                rates == null ||
                rates.Length == 0 ||
                // This check on scheduled run (not first run) is perfomed to make sure that CNB has published desired rates, if not, than wait one second
                (!isFirstRun && DateTime.ParseExact(rates[0].validFor, "yyyy-MM-dd", CultureInfo.InvariantCulture).Month != (DateTime.Now.Month-1))
                )
            {
                Task.Delay(1000);
                rates = _cnbHelper.GetOtherCurrenciesValidRates().Result;
            }

            foreach (var otherRate in rates)
            {
                if (otherRate.rate != 0 && otherRate.amount == 1)
                {
                    exchangeRatesOther.Add(ExchangeRateData.CreateFromOtherCurrencyCnbRate(otherRate));
                }
            }            

            if(!isFirstRun)
            {
                DbHelper.CleanupRates(dbContext, false);
            }
            
            DbHelper.WriteRates(dbContext, exchangeRatesOther.ToArray());
        }
        
    }
}