using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Worker;
using ExchangeRateUpdater.Worker.Jobs;
using Quartz;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplicationServices();
        services.AddInfrastructure(hostContext.Configuration);
        services.AddExchangeRateWorkerServices();

        services.AddQuartz(q =>
        {
            //All this part could be improved by using a loop getting all Jobs via reflection in case there were many jobs to register.

            var backfillJobKey = new JobKey("ExchangeRatesBackfillJob");
            q.AddJob<ExchangeRatesBackfillJob>(opts => opts.WithIdentity(backfillJobKey));
            q.AddTrigger(opts => opts
                .ForJob(backfillJobKey)
                .WithIdentity("ExchangeRatesBackfillJob")
                .StartNow()
            );
            var dailyJobKey = new JobKey("DailyExchangeRatesRefreshJob");
            q.AddJob<DailyExchangeRatesRefreshJob>(opts => opts.WithIdentity(dailyJobKey));
            q.AddTrigger(opts => opts
                .ForJob(dailyJobKey)
                .WithIdentity("DailyExchangeRatesRefreshJob")
                .WithCronSchedule("0 0 0 ? * * *")
            );
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    })
    .Build();

await builder.RunAsync();