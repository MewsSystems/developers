using ExchangeRates.Api;
using Serilog;

var builder = Host.CreateDefaultBuilder(args);

builder
    .ConfigureLogging(logginBuilder =>
    {
        logginBuilder.ClearProviders();
    })
    .ConfigureWebHostDefaults(webhostBuilder =>
    {
        webhostBuilder
            .UseStartup<Startup>();
    })
    .UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom
            .Configuration(context.Configuration);
    });

await builder
        .Build()
        .RunAsync();

