using ExchangeRateUpdater.ExchangeRate.Factory;
using ExchangeRateUpdater.ExchangeRate.Provider;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank;
using ExchangeRateUpdater.ExchangeRate.Repository;
using ExchangeRateUpdater.ExchangeRate.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater;
using Quartz;
using ExchangeRateUpdater.ExchangeRate.Job;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        // Setup configuration
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
        });
        builder.Services.AddHttpClient();
        builder.Services.Configure<CzechNationalBankConfig>(configuration.GetSection("CzechNationalBank"));
        builder.Services.Configure<DefaultExchangeRateProviderConfig>(configuration.GetSection("DefaultExchangeRateProvider"));
        builder.Services.AddTransient<IHttpService, HttpService>();
        builder.Services.AddTransient<ICzechNationalBankClient, CzechNationalBankClient>();
        builder.Services.AddTransient<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
        builder.Services.AddTransient<IExchangeRateProviderFactory, ExchangeRateProviderFactory>();
        builder.Services.AddTransient<IExchangeRateService, HostedExchangeRateService>();
        builder.Services.AddSingleton<IExchangeRateRepository, InMemoryExchangeRateRepository>();
        builder.Services.AddQuartz(q =>
        {
            //Czech National Bank updates exchange rates every working day at 14:30 local time
            //Source: https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/
            var jobId = "CzechNationalBankExchangeRateUpdateJob";
            var jobTriggerId = "CzechNationalBankExchangeRateUpdateJobTrigger";
            var jobKey = new JobKey(jobId);
            q.AddJob<CzechNationalBankExchangeRateUpdateJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobTriggerId)
                .WithCronSchedule("35-59/5 14 * * * ?", //Runs every 5 minutes between 14:35 and 15:59 Czech local time
                x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"))
                ));
        });

        builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}