using ExchangeRateProvider.Application;
using ExchangeRateProvider.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Prometheus;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    // Configure JSON serialization for better performance
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = false;
});

// API Versioning can be added later when needed

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Exchange Rate Provider API",
        Version = "v1",
        Description = "Exchange rate API with CNB data"
    });
});

// Health checks
builder.Services.AddHealthChecks()
    .AddUrlGroup(
        new Uri(builder.Configuration["ExchangeRateProvider:CnbHealthUrl"] 
                ?? "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"), 
        name: "cnb")
    .AddRedis(
        builder.Configuration["Redis:Configuration"], 
        name: "redis", 
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded, 
        tags: ["ready"]);

// JWT Auth (toggle via config)
var jwtEnabled = builder.Configuration.GetValue<bool>("Jwt:Enabled", false);
if (jwtEnabled)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.Audience = builder.Configuration["Jwt:Audience"];
            options.Authority = builder.Configuration["Jwt:Authority"];
            // Additional token validation parameters can be added here
        });
}

// Add logging with structured logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100, // 100 requests per window
                Window = TimeSpan.FromMinutes(1) // per minute
            }));
});

// Add application and infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Metrics
app.UseHttpMetrics();

// Rate limiting
app.UseRateLimiter();

// Request logging middleware
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var startTime = DateTime.UtcNow;

    logger.LogInformation(
        "Request: {Method} {Path} from {RemoteIp}",
        context.Request.Method,
        context.Request.Path,
        context.Connection.RemoteIpAddress);

    await next();

    var duration = DateTime.UtcNow - startTime;
    logger.LogInformation(
        "Response: {StatusCode} in {Duration}ms",
        context.Response.StatusCode,
        duration.TotalMilliseconds);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection(); // only for production
}

if (jwtEnabled)
{
    app.UseAuthentication();
}

app.UseAuthorization();

app.MapControllers();

// Health checks endpoint
app.MapHealthChecks("/health");

// Prometheus scrape endpoint
app.MapMetrics();

app.Run();

namespace ExchangeRateProvider.Api
{
    public partial class Program { }
}
