using Asp.Versioning;
using ExchangeRateUpdater.Api.Extensions;
using ExchangeRateUpdater.Api.HealthChecks;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Infrastructure.Caching;
using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Infrastructure.DataSources.Cnb;
using ExchangeRateUpdater.Infrastructure.Logging;
using ExchangeRateUpdater.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using NodaTime;
using Serilog;
using Polly;
using Polly.Extensions.Http;
using MediatR;
using ExchangeRateUpdater.Core.Common.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.ConfigureLogging();

// Configuration
builder.Services.Configure<ExchangeRateServiceOptions>(
    builder.Configuration.GetSection(ExchangeRateServiceOptions.SectionName));

// API Versioning and OpenAPI
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    }).
    AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Add services to the container
builder.Services.AddControllers();

// Configure Swagger documentation with examples
builder.Services.AddSwaggerDocumentation();

// Configure Redis Cache
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = builder.Configuration["Redis:Configuration"]; });

// Health Checks
builder.Services.AddHealthChecks().
    AddCheck<RedisHealthCheck>("redis_health_check", tags: new[] { "ready" }).
    AddCheck<CnbApiHealthCheck>("cnb_api_health_check", tags: new[] { "ready" });

// MediatR - register handlers from all assemblies
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CnbExchangeRateService).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Queries.GetExchangeRateQuery).Assembly);

    // Add validation behavior to pipeline
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// FluentValidation - register validators from all assemblies
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddValidatorsFromAssemblyContaining<ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Validators.BatchRateRequestValidator>();

// Register core services
builder.Services.AddSingleton<IClock>(SystemClock.Instance);
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IExchangeRateService, CnbExchangeRateService>();

// Register HttpClient for CNB with Polly policies
builder.Services.AddHttpClient<IExchangeRateDataSource, CnbExchangeRateDataSource>().
    AddPolicyHandler((services,
        request) =>
    {
        var options = services.GetRequiredService<IOptions<ExchangeRateServiceOptions>>().
            Value;
        return HttpPolicyExtensions.HandleTransientHttpError().
            WaitAndRetryAsync(
                options.RetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(options.RetryBaseDelaySeconds, retryAttempt)),
                (outcome,
                    timespan,
                    retryAttempt,
                    context) =>
                {
                    services.GetRequiredService<ILogger<CnbExchangeRateDataSource>>().
                        LogWarning("Retrying CNB API request {Attempt} after {Delay}ms",
                            retryAttempt,
                            timespan.TotalMilliseconds);
                });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

// Register global exception handler middleware
app.UseGlobalExceptionHandler();

// Health check endpoints
app.MapHealthChecks("/health"); // Basic health check
app.MapHealthChecks("/health/ready",
    new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("ready")
    }); // Readiness check (Redis, CNB API)

app.MapControllers();

Log.Information("Application starting up");
app.Run();