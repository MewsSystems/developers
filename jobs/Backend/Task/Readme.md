# Microservices Project with Monitoring, Logging, and Testing

## Overview
This project demonstrates a microservices architecture using .NET 6. The project includes:
- **Exchange Rate Retrieval Service**: Fetches exchange rates from an external API.
- **Exchange Rate Provider Service**: Consumes the data retrieval service and provides filtered exchange rates.
- **Unit Testing**: Tests for the services using xUnit and Moq.
- **Logging**: Logging setup with Serilog.
- **Monitoring**: Monitoring setup with Prometheus and Grafana.

## Prerequisites
- .NET 6 SDK
- Prometheus
- Serilog

## Setup and Configuration

### 1. Define URLs in `appsettings.json`
Ensure your `appsettings.json` includes the URLs for your services.

```json
{
  "Urls": {
    "ExchangeRetrievalService": "https://localhost:7172"
  }
}
```
### 2. Configure Services in `Program.cs`
Update Program.cs to read the configuration and inject it into your services.

```var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Urls"));
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddTransient<DataRetrievalClient>();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

public class AppSettings
{
    public string ExchangeRetrievalService { get; set; }
}
```
### 3. Add Unit Tests
Add unit tests for both services using xUnit and Moq.

### 4. Add Logger
Add a logger for both services and everywhere around the app using Serilog

### 5. Add Prometheus
Make sure you downloaded Prometheus and run the command from the same place you downloaded it,

and make sure the configuration file is the same as the file in the app `prometheus.yml`

Run Prometheus using this configuration:

```./prometheus --config.file=prometheus.yml```

and Run ```https://localhost:7172/metrics``` for checking





