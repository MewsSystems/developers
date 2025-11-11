using System.Text;
using ApplicationLayer;
using ApplicationLayer.Common.Interfaces;
using Common.Interfaces;
using ConfigurationLayer.Interface;
using ConfigurationLayer.Option;
using ConfigurationLayer.Service;
using CzechNationalBank;
using DataLayer;
using EuropeanCentralBank;
using gRPC.Streaming;
using Hangfire;
using Hangfire.Dashboard;
using InfrastructureLayer;
using InfrastructureLayer.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RomanianNationalBank;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// KESTREL CONFIGURATION FOR HTTP/2 (required for gRPC)
// ============================================================
builder.WebHost.ConfigureKestrel(options =>
{
    // HTTP/2 endpoint for gRPC (port 5001)
    options.ListenLocalhost(5001, o => o.Protocols = HttpProtocols.Http2);
});

// ============================================================
// CONFIGURATION OPTIONS
// ============================================================
builder.Services.Configure<SystemConfigurationOptions>(
    builder.Configuration.GetSection("SystemConfiguration"));

builder.Services.Configure<ExchangeRateProvidersOptions>(
    builder.Configuration.GetSection("ExchangeRateProviders"));

// ============================================================
// LAYER REGISTRATIONS (REUSE FROM REST)
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
// EXCHANGE RATE PROVIDERS (REUSE FROM REST)
// ============================================================
builder.Services.AddHttpClient();

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
// GRPC SERVICES
// ============================================================
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.MaxReceiveMessageSize = 16 * 1024 * 1024; // 16 MB
    options.MaxSendMessageSize = 16 * 1024 * 1024; // 16 MB
});

// gRPC reflection for development (allows tools like grpcurl to discover services)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddGrpcReflection();
}

// ============================================================
// STREAMING INFRASTRUCTURE (REPLACES SIGNALR!)
// ============================================================
// Singleton stream manager to track all active client connections
builder.Services.AddSingleton<IExchangeRatesStreamManager, ExchangeRatesStreamManager>();

// gRPC notification service (replaces SignalR version)
builder.Services.AddScoped<IExchangeRatesNotificationService, ExchangeRatesGrpcNotificationService>();

// ============================================================
// AUTHENTICATION & AUTHORIZATION
// ============================================================
// Clear default claim mappings to prevent JWT claims from being transformed
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Consumer", policy => policy.RequireRole("Consumer"));
});

// ============================================================
// BUILD APPLICATION
// ============================================================
var app = builder.Build();

// ============================================================
// DATABASE INITIALIZATION (In-Memory Mode)
// ============================================================
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

            var context = services.GetRequiredService<DataLayer.ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
            logger.LogInformation("Database schema created successfully.");

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
app.UseAuthentication();
app.UseAuthorization();

// ============================================================
// MAP GRPC SERVICES
// ============================================================
app.MapGrpcService<gRPC.Services.AuthenticationGrpcService>();
app.MapGrpcService<gRPC.Services.ExchangeRatesGrpcService>();
app.MapGrpcService<gRPC.Services.ProvidersGrpcService>();
app.MapGrpcService<gRPC.Services.CurrenciesGrpcService>();
app.MapGrpcService<gRPC.Services.UsersGrpcService>();
app.MapGrpcService<gRPC.Services.SystemHealthGrpcService>();

// gRPC reflection endpoint (for dev tools like grpcurl, Postman)
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

// Hangfire Dashboard (monitor background jobs)
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = Array.Empty<IDashboardAuthorizationFilter>() // Allow all in development
});

// ============================================================
// INITIALIZE BACKGROUND JOBS
// ============================================================
InfrastructureLayer.InfrastructureLayerServiceExtensions.UseInfrastructureLayerBackgroundJobs(app.Services, app.Configuration);

// ============================================================
// RUN APPLICATION
// ============================================================
var appLogger = app.Services.GetRequiredService<ILogger<Program>>();
appLogger.LogInformation("gRPC Server starting on http://localhost:5001");
appLogger.LogInformation("Hangfire Dashboard available at http://localhost:5001/hangfire");

app.Run();

// Make Program accessible to integration tests
public partial class Program { }
