using ExchangeRateProvider.Api.Authentication;
using ExchangeRateProvider.Api.Endpoints;
using ExchangeRateProvider.Api.Extensions;
using ExchangeRateProvider.Application;
using ExchangeRateProvider.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

try
{
    Log.Information("Starting Exchange Rate Provider API...");
    Log.Information("Environment: {Environment}", builder.Environment.EnvironmentName);

    builder.Services
        .AddOpenApi()
        .AddAuth0Authentication(builder.Configuration)
        .AddAuthorization()
        .AddExchangeRatesRateLimiting()
        .AddApplicationServices()
        .AddInfrastructureServices(builder.Configuration);

    builder.Services.AddHealthChecks();

    var app = builder.Build();

    app.UseRequestResponseLogging();

    app.UseAuthentication();
    app.UseAuthorization();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();

        Log.Information("Development mode: OpenAPI and Scalar documentation enabled");
    }

    app.UseHttpsRedirection();

    app.MapExchangeRatesEndpoints();
    app.MapHealthChecks("/health");

    Log.Information("Exchange Rate Provider API configured successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
