using Asp.Versioning;
using ExchangeRateUpdater.Api.Extensions;
using ExchangeRateUpdater.Api.HealthChecks;
using ExchangeRateUpdater.Core.Extensions;
using ExchangeRateUpdater.Infrastructure.Extensions;
using ExchangeRateUpdater.Infrastructure.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

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
}

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