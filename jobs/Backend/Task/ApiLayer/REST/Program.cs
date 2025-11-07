using ApplicationLayer;
using Common.Interfaces;
using ConfigurationLayer.Interface;
using ConfigurationLayer.Option;
using ConfigurationLayer.Service;
using CzechNationalBank;
using DataLayer;
using EuropeanCentralBank;
using Hangfire;
using Hangfire.Dashboard;
using InfrastructureLayer;
using Microsoft.Extensions.Options;
using RomanianNationalBank;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// CONFIGURATION OPTIONS
// ============================================================
// Register configuration options from appsettings.json
builder.Services.Configure<SystemConfigurationOptions>(
    builder.Configuration.GetSection("SystemConfiguration"));

builder.Services.Configure<ExchangeRateProvidersOptions>(
    builder.Configuration.GetSection("ExchangeRateProviders"));

// ============================================================
// LAYER REGISTRATIONS
// ============================================================
// 1. DataLayer - EF Core DbContext and repositories
builder.Services.AddDataLayer(builder.Configuration);

// 2. ConfigurationLayer - Configuration services with caching
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
builder.Services.AddSingleton<IProviderConfigurationService, ProviderConfigurationService>();

// 3. InfrastructureLayer - Infrastructure services, adapters, and background jobs
builder.Services.AddInfrastructureLayer(builder.Configuration);

// 4. ApplicationLayer - CQRS handlers, MediatR, and pipeline behaviors
builder.Services.AddApplicationLayer();

// ============================================================
// EXCHANGE RATE PROVIDERS
// ============================================================
// Register HttpClient for providers
builder.Services.AddHttpClient();

// Register provider implementations
builder.Services.AddSingleton<IExchangeRateProvider>(sp =>
{
    var providersOptions = sp.GetRequiredService<IOptions<ExchangeRateProvidersOptions>>().Value;
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("ECB");
    return new EuropeanCentralBankProvider(providersOptions.ECB, httpClient);
});

builder.Services.AddSingleton<IExchangeRateProvider>(sp =>
{
    var providersOptions = sp.GetRequiredService<IOptions<ExchangeRateProvidersOptions>>().Value;
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("CNB");
    return new CzechNationalBankProvider(providersOptions.CNB, httpClient);
});

builder.Services.AddSingleton<IExchangeRateProvider>(sp =>
{
    var providersOptions = sp.GetRequiredService<IOptions<ExchangeRateProvidersOptions>>().Value;
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("BNR");
    return new RomanianNationalBankProvider(providersOptions.BNR, httpClient);
});

// ============================================================
// ASP.NET CORE SERVICES
// ============================================================
builder.Services.AddControllers();

// OpenAPI/Swagger
builder.Services.AddOpenApi();

// ============================================================
// BUILD APPLICATION
// ============================================================
var app = builder.Build();

// ============================================================
// DATABASE INITIALIZATION (In-Memory Mode)
// ============================================================
// If using in-memory database, create schema and seed data
var useInMemoryDatabase = builder.Configuration.GetValue<bool>("Database:UseInMemoryDatabase");
if (useInMemoryDatabase)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Initializing in-memory database...");

            // Create database schema
            var context = services.GetRequiredService<DataLayer.ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
            logger.LogInformation("Database schema created successfully.");

            // Seed initial data
            var seeder = services.GetRequiredService<DataLayer.Seeding.DatabaseSeeder>();
            await seeder.SeedAllAsync();
            logger.LogInformation("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the in-memory database.");
            throw;
        }
    }
}

// ============================================================
// MIDDLEWARE PIPELINE
// ============================================================
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Hangfire Dashboard (monitor background jobs)
// Access at: /hangfire
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    // In production, add authentication:
    // Authorization = new[] { new HangfireAuthorizationFilter() }
    Authorization = Array.Empty<IDashboardAuthorizationFilter>() // Allow all in development
});

app.UseAuthorization();

app.MapControllers();

// ============================================================
// INITIALIZE BACKGROUND JOBS
// ============================================================
// Schedule background jobs after application is built
InfrastructureLayer.InfrastructureLayerServiceExtensions.UseInfrastructureLayerBackgroundJobs(app.Services);

// ============================================================
// RUN APPLICATION
// ============================================================
app.Run();

// Make Program accessible to integration tests
public partial class Program { }
