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
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater;

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
        builder.Services.AddTransient<ICzechNationalBankClient, CzechNationalBankClient>();
        builder.Services.AddTransient<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
        builder.Services.AddTransient<IExchangeRateProviderFactory, ExchangeRateProviderFactory>();
        builder.Services.AddTransient<IExchangeRateService, HostedExchangeRateService>();
        builder.Services.AddSingleton<IExchangeRateRepository, InMemoryExchangeRateRepository>();
        builder.Services.AddHangfire(configuration => configuration
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseStorage(new MemoryStorage()));
        builder.Services.AddHangfireServer();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseHangfireDashboard();

        app.Services.GetRequiredService<IExchangeRateService>().SetupExchangeRateUpdaterWorker();

        app.Run();
    }
}