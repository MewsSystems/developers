using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Worker.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

// Ensure REDIS_CONNECTION env var is visible in IConfiguration
var redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION");
var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        if (!string.IsNullOrWhiteSpace(redisConnection))
        {
            hostContext.Configuration["Redis:Connection"] = redisConnection;
        }
        services.AddApplicationServices();
        services.AddInfrastructure(hostContext.Configuration);

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionScopedJobFactory();

            var jobKey = new JobKey("RefreshRatesJob");
            q.AddJob<RefreshRatesJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("RefreshRatesTrigger")
                .WithCronSchedule("0 0/1 * * * ?"));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    })
    .Build();

await builder.RunAsync();
