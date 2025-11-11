using System.Text;
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
using InfrastructureLayer.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RomanianNationalBank;
using SoapCore;
using Microsoft.IdentityModel.JsonWebTokens;

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
// AUTHENTICATION & AUTHORIZATION
// ============================================================
// Clear default claim mappings to prevent JWT claims from being transformed
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT settings not found in configuration");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero, // Remove default 5-minute clock skew
        RoleClaimType = "role", // Match the simple "role" claim in the token
        NameClaimType = "email"  // Match the "email" claim in the token
    };

    // IMPORTANT: Configure claim types for the authentication handler
    options.MapInboundClaims = false; // Prevent claim type mapping

    // Allow SignalR to receive JWT token from query string
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            // If the request is for our SignalR hub...
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/exchange-rates"))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Consumer", policy => policy.RequireRole("Consumer"));
});

// ============================================================
// SIGNALR
// ============================================================
builder.Services.AddSignalR();

// Register SignalR notification service for real-time updates
builder.Services.AddScoped<ApplicationLayer.Common.Interfaces.IExchangeRatesNotificationService,
    SOAP.Services.SignalRExchangeRatesNotificationService>();

// ============================================================
// SOAP SERVICES
// ============================================================
// Register SOAP service implementations
builder.Services.AddScoped<SOAP.Services.IExchangeRateService, SOAP.Services.ExchangeRateService>();
builder.Services.AddScoped<SOAP.Services.IAuthenticationService, SOAP.Services.AuthenticationService>();
builder.Services.AddScoped<SOAP.Services.ICurrencyService, SOAP.Services.CurrencyService>();
builder.Services.AddScoped<SOAP.Services.IProviderService, SOAP.Services.ProviderService>();
builder.Services.AddScoped<SOAP.Services.IUserService, SOAP.Services.UserService>();
builder.Services.AddScoped<SOAP.Services.ISystemHealthService, SOAP.Services.SystemHealthService>();

builder.Services.AddSoapCore();

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
app.UseRouting();

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Hangfire Dashboard (monitor background jobs)
// Access at: /hangfire
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    // In production, add authentication:
    // Authorization = new[] { new HangfireAuthorizationFilter() }
    Authorization = Array.Empty<IDashboardAuthorizationFilter>() // Allow all in development
});

// ============================================================
// SOAP ENDPOINTS
// ============================================================
// Map SOAP endpoints with WSDL generation
app.UseEndpoints(endpoints =>
{
    var soapEncoderOptions = new SoapEncoderOptions
    {
        MessageVersion = System.ServiceModel.Channels.MessageVersion.Soap11,
        WriteEncoding = System.Text.Encoding.UTF8
    };

    // Exchange Rate Service
    // Access WSDL at: http://localhost:5002/ExchangeRateService.asmx?wsdl
    endpoints.UseSoapEndpoint<SOAP.Services.IExchangeRateService>(
        "/ExchangeRateService.asmx",
        soapEncoderOptions,
        SoapSerializer.DataContractSerializer,
        caseInsensitivePath: true)
        .RequireAuthorization();

    // Authentication Service
    // Access WSDL at: http://localhost:5002/AuthenticationService.asmx?wsdl
    endpoints.UseSoapEndpoint<SOAP.Services.IAuthenticationService>(
        "/AuthenticationService.asmx",
        soapEncoderOptions,
        SoapSerializer.DataContractSerializer,
        caseInsensitivePath: true);

    // Currency Service
    // Access WSDL at: http://localhost:5002/CurrencyService.asmx?wsdl
    endpoints.UseSoapEndpoint<SOAP.Services.ICurrencyService>(
        "/CurrencyService.asmx",
        soapEncoderOptions,
        SoapSerializer.DataContractSerializer,
        caseInsensitivePath: true)
        .RequireAuthorization();

    // Provider Service
    // Access WSDL at: http://localhost:5002/ProviderService.asmx?wsdl
    endpoints.UseSoapEndpoint<SOAP.Services.IProviderService>(
        "/ProviderService.asmx",
        soapEncoderOptions,
        SoapSerializer.DataContractSerializer,
        caseInsensitivePath: true)
        .RequireAuthorization();

    // User Service
    // Access WSDL at: http://localhost:5002/UserService.asmx?wsdl
    endpoints.UseSoapEndpoint<SOAP.Services.IUserService>(
        "/UserService.asmx",
        soapEncoderOptions,
        SoapSerializer.DataContractSerializer,
        caseInsensitivePath: true)
        .RequireAuthorization();

    // System Health Service
    // Access WSDL at: http://localhost:5002/SystemHealthService.asmx?wsdl
    endpoints.UseSoapEndpoint<SOAP.Services.ISystemHealthService>(
        "/SystemHealthService.asmx",
        soapEncoderOptions,
        SoapSerializer.DataContractSerializer,
        caseInsensitivePath: true)
        .RequireAuthorization();

    // Map SignalR hub for real-time exchange rate updates
    // Access at: /hubs/exchange-rates
    endpoints.MapHub<SOAP.Hubs.ExchangeRatesHub>("/hubs/exchange-rates");
});

// ============================================================
// INITIALIZE BACKGROUND JOBS
// ============================================================
// Schedule background jobs after application is built
InfrastructureLayer.InfrastructureLayerServiceExtensions.UseInfrastructureLayerBackgroundJobs(app.Services, app.Configuration);

// ============================================================
// RUN APPLICATION
// ============================================================
app.Run();

// Make Program accessible to integration tests
public partial class Program { }
