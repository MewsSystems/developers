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
using REST.Middleware;
using RomanianNationalBank;
using Scalar.AspNetCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Diagnostics;

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

// Register notification service for real-time updates
builder.Services.AddScoped<ApplicationLayer.Common.Interfaces.IExchangeRatesNotificationService,
    REST.Services.ExchangeRatesNotificationService>();

// OpenAPI/Swagger with comprehensive documentation
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Exchange Rate Provider API",
            Version = "v1",
            Description = """
                RESTful API for managing exchange rate providers and exchange rates.

                ## Features
                - Exchange rate management (query, create, update)
                - Provider management (activate/deactivate, health monitoring)
                - Currency management
                - User authentication and authorization
                - System health monitoring

                ## Areas
                - **Exchange Rates**: Query exchange rates with filtering and conversion
                - **Providers**: Manage exchange rate data providers
                - **Currencies**: Manage supported currencies
                - **Users**: User management and authentication
                - **System Health**: Monitor system and provider health
                """,
            Contact = new()
            {
                Name = "API Support",
                Email = "support@example.com"
            }
        };
        return Task.CompletedTask;
    });
});

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
// Global exception handler (must be first to catch all exceptions)
app.UseGlobalExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Map OpenAPI specification endpoint
    app.MapOpenApi();

    // Scalar provides a beautiful API documentation UI
    // Access at: /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Exchange Rate Provider API")
            .WithTheme(ScalarTheme.Purple)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

//This made me lose a day and 100% of sanity.
//app.UseHttpsRedirection();

// Hangfire Dashboard (monitor background jobs)
// Access at: /hangfire
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    // In production, add authentication:
    // Authorization = new[] { new HangfireAuthorizationFilter() }
    Authorization = Array.Empty<IDashboardAuthorizationFilter>() // Allow all in development
});

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map SignalR hub for real-time exchange rate updates
// Access at: /hubs/exchange-rates
app.MapHub<REST.Hubs.ExchangeRatesHub>("/hubs/exchange-rates");

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
