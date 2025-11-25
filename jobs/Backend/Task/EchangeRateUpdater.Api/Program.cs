using System.Text.Json;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Read REDIS_CONNECTION env var into configuration (if present)
var redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION");
if (!string.IsNullOrWhiteSpace(redisConnection))
{
	builder.Configuration["Redis:Connection"] = redisConnection;
}

// Serilog
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddHealthChecks()
	.AddCheck("self", () => HealthCheckResult.Healthy())
	.AddRedis(builder.Configuration.GetValue<string>("Redis:Connection"), name: "redis", tags: new[] { "ready" });

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();