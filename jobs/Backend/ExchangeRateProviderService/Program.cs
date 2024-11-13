using ExchangeRateProviderService.Services;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Serilog;
using Prometheus;


// intiate the logger and configure it to write to the console and a file
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Create the builder and configure the services
var builder = WebApplication.CreateBuilder(args);

// Add Application Insights telemetry collection
builder.Services.AddApplicationInsightsTelemetry();

// Add Application Insights Telemetry Processor to filter out dependency telemetry
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Urls"));

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();

// Add services to the container.
// add the DataRetrievalClient to the services container
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<DataRetrievalClient>();
builder.Services.AddSwaggerGen();

// add the AppSettings to the services container

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Use Prometheus metrics middleware
app.UseMetricServer();
app.UseHttpMetrics();

app.UseHttpsRedirection();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapMetrics(); // Map Prometheus metrics endpoint
});

app.MapControllers();

app.Run();

/// <summary>
/// Represents the application settings.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Gets or sets the URL of the Exchange Retrieval Service.
    /// </summary>
    public string ExchangeRetrievalService { get; set; }
}