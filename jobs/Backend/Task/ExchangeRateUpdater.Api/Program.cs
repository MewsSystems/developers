using ExchangeRateUpdater.API.Middleware;
using ExchangeRateUpdater.Application.DependencyInjection;
using ExchangeRateUpdater.Application.Settings;
using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Read Configuration
var configuration = builder.Configuration;

// Register Settings
builder.Services.Configure<CacheSettings>(configuration.GetSection(nameof(CacheSettings)));
builder.Services.Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)));
builder.Services.Configure<HttpClientSettings>(configuration.GetSection(nameof(HttpClientSettings)));

// Register Application and Infrastructure Services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration.GetSection(nameof(HttpClientSettings)).Get<HttpClientSettings>() ?? new());

// Register Middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// Register Controllers
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Exchange Rate API",
        Version = "v1",
        Description = "API for fetching exchange rates from the Czech National Bank",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact { Name = "Antonio Esposito" }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Register MediatR Services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

var app = builder.Build();

// Add Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rate API v1");
});

// Enable Security Middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Serilog Request Logging
app.UseSerilogRequestLogging(opts =>
{
    opts.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestPath", httpContext.Request.Path);
        diagnosticContext.Set("StatusCode", httpContext.Response.StatusCode);
        diagnosticContext.Set("Method", httpContext.Request.Method);
    };
});

await app.RunAsync();
