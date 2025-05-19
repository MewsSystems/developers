using Asp.Versioning;
using ExchangeRateUpdater.Api.Extensions;
using ExchangeRateUpdater.Api.HealthChecks;
using ExchangeRateUpdater.Core.Extensions;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Queries;
using ExchangeRateUpdater.Infrastructure.Extensions;
using ExchangeRateUpdater.Infrastructure.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.ConfigureSerilogLogging();

// Add services to the container
builder.Services.AddControllers();

// Add Core services
builder.Services.AddCoreServices();

// Add Infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

// API Versioning and OpenAPI
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Configure Swagger documentation with examples
builder.Services.AddSwaggerDocumentation();

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<RedisHealthCheck>("redis_health_check", tags: new[] { "ready" })
    .AddCheck<CnbApiHealthCheck>("cnb_api_health_check", tags: new[] { "ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();

    // Run startup test in development mode
    using var scope = app.Services.CreateScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Running startup test for batch exchange rates...");

        var testCurrencies = new[]
        {
            "USD",
            "EUR",
            "CZK",
            "JPY",
            "KES",
            "RUB",
            "THB",
            "TRY",
            "XYZ"
        };

        var currencyPairs = testCurrencies
            .Select(code => $"{code}/CZK")
            .ToList();

        var request = new BatchRateRequest
        {
            CurrencyPairs = currencyPairs
        };

        var result = await mediator.Send(new GetBatchExchangeRatesQuery(request));

        logger.LogInformation("Startup test completed successfully. Found {Count} exchange rates:", result.Rates.Count);
        foreach (var rate in result.Rates)
        {
            logger.LogInformation("{Source}/{Target}={Rate}",
                rate.SourceCurrency,
                rate.TargetCurrency,
                rate.Rate);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error during startup test");
    }
}

app.UseHttpsRedirection();

// Register global exception handler middleware
app.UseGlobalExceptionHandler();

// Health check endpoints
app.MapHealthChecks("/health",
    new HealthCheckOptions
    {
        Predicate = check => !check.Tags.Contains("ready")
    }); // Basic health check

app.MapHealthChecks("/health/ready",
    new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("ready")
    }); // Readiness check (Redis, CNB API)

app.MapControllers();

app.Run();