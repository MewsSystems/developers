using ExchangeRatesGetterWorkerService.Context;
using ExchangeRatesGetterWorkerService.Models;
using ExchangeRatesGetterWorkerService.Helpers;

namespace ExchangeRatesGetterWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();


                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                DbHelper.InitDb(dbContext);

                List<ExchangeRateData> rates = DbHelper.GetRates(dbContext);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}