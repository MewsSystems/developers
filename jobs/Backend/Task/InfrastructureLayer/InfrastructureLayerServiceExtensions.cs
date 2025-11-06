using ApplicationLayer.Common.Interfaces;
using ConfigurationLayer.Interface;
using ConfigurationLayer.Option;
using DomainLayer.Interfaces.Services;
using Hangfire;
using Hangfire.SqlServer;
using InfrastructureLayer.BackgroundJobs;
using InfrastructureLayer.BackgroundJobs.Jobs;
using InfrastructureLayer.ExternalServices;
using InfrastructureLayer.ExternalServices.Discovery;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfrastructureLayer;

/// <summary>
/// Extension methods for configuring InfrastructureLayer services.
/// </summary>
public static class InfrastructureLayerServiceExtensions
{
    /// <summary>
    /// Adds InfrastructureLayer services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Services
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        services.AddSingleton<IPasswordHasher, Services.BCryptPasswordHasher>();

        // Provider discovery
        services.AddSingleton<IProviderDiscoveryService, ProviderDiscoveryService>();

        // Note: IExchangeRateProvider implementations should be registered by the host application
        // from ExchangeRateProviderLayer (ECB, CNB, BNR providers)

        // Background jobs
        services.AddTransient<FetchHistoricalRatesJob>();
        services.AddTransient<FetchLatestRatesJob>();
        services.AddTransient<RetryFailedFetchesJob>();

        // Hangfire
        ConfigureHangfire(services, configuration);

        // Unit of Work - Bridge between DomainLayer and DataLayer
        // Note: DomainLayer.Interfaces.Persistence.IUnitOfWork is implemented by InfrastructureLayer.Persistence.DomainUnitOfWork
        // which adapts DataLayer.IUnitOfWork (repository adapters handle entity-aggregate mapping)
        services.AddScoped<DomainLayer.Interfaces.Persistence.IUnitOfWork, Persistence.DomainUnitOfWork>();

        return services;
    }

    /// <summary>
    /// Initializes and schedules background jobs.
    /// Call this after the application has started and database is ready.
    /// </summary>
    public static void UseInfrastructureLayerBackgroundJobs(IServiceProvider serviceProvider)
    {
        // Schedule historical fetch on startup (one-time)
        BackgroundJob.Enqueue<FetchHistoricalRatesJob>(job => job.ExecuteAsync(CancellationToken.None));

        // Schedule recurring jobs for each provider
        // In a real implementation, you would get provider configurations from database
        ScheduleProviderRecurringJobs(serviceProvider);
    }

    private static void ConfigureHangfire(IServiceCollection services, IConfiguration configuration)
    {
        // Get connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

        // Read worker count from appsettings.json
        // Note: ConfigurationService is not available during service registration
        // as it requires database access which may not be initialized yet
        var systemConfig = new SystemConfigurationOptions();
        configuration.GetSection("SystemConfiguration").Bind(systemConfig);
        int workerCount = systemConfig.BackgroundJobs.HangfireWorkerCount;

        // Configure Hangfire
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });
        });

        // Add Hangfire server with configured worker count
        services.AddHangfireServer(options =>
        {
            options.WorkerCount = workerCount;
        });
    }

    private static void ScheduleProviderRecurringJobs(IServiceProvider serviceProvider)
    {
        var providerConfigService = serviceProvider.GetRequiredService<IProviderConfigurationService>();
        var configService = serviceProvider.GetRequiredService<IConfigurationService>();
        var logger = serviceProvider.GetRequiredService<ILogger<FetchLatestRatesJob>>();

        // Get default configuration from ConfigurationService (checks cache -> db -> appsettings)
        var defaultCronExpression = configService.GetValueAsync("DefaultCronExpression", "0 16 * * *").GetAwaiter().GetResult();
        var defaultTimezone = configService.GetValueAsync("DefaultTimezone", "UTC").GetAwaiter().GetResult();

        // Get all active provider configurations
        var providers = providerConfigService.GetAllActiveProviderConfigurationsAsync().GetAwaiter().GetResult();

        foreach (var provider in providers)
        {
            if (!provider.IsActive)
            {
                logger.LogDebug("Skipping inactive provider {ProviderCode}", provider.Code);
                continue;
            }

            // Get provider-specific UpdateTime and TimeZone from Configuration dictionary
            var updateTime = provider.Configuration.TryGetValue("UpdateTime", out var time) ? time : "16:00";
            var timezone = provider.Configuration.TryGetValue("TimeZone", out var tz) ? tz : defaultTimezone;

            // Convert time (hh:mm) to cron expression (0 minute hour * * *)
            var cronExpression = ConvertTimeToCronExpression(updateTime, defaultCronExpression);

            RecurringJob.AddOrUpdate(
                recurringJobId: $"fetch-latest-{provider.Code}",
                methodCall: () => ExecuteFetchLatestJob(provider.Code),
                cronExpression: cronExpression,
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(ConvertTimeZoneId(timezone))
                });

            logger.LogInformation(
                "Scheduled recurring job for provider {ProviderCode} at {UpdateTime} ({Timezone}) - Cron: {CronExpression}",
                provider.Code,
                updateTime,
                timezone,
                cronExpression);
        }
    }

    /// <summary>
    /// Converts time in format "hh:mm" to cron expression "minute hour * * *" (daily schedule).
    /// </summary>
    private static string ConvertTimeToCronExpression(string time, string defaultCron)
    {
        if (string.IsNullOrWhiteSpace(time))
            return defaultCron;

        var parts = time.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[0], out var hour) || !int.TryParse(parts[1], out var minute))
        {
            return defaultCron;
        }

        if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
        {
            return defaultCron;
        }

        // Cron format: minute hour day month dayOfWeek
        // For daily schedule: minute hour * * *
        return $"{minute} {hour} * * *";
    }

    /// <summary>
    /// Converts common timezone abbreviations to .NET TimeZoneInfo IDs.
    /// </summary>
    private static string ConvertTimeZoneId(string timezone)
    {
        return timezone?.ToUpperInvariant() switch
        {
            "CET" => "Central European Standard Time",
            "CEST" => "Central European Standard Time",
            "EET" => "E. Europe Standard Time",
            "EEST" => "E. Europe Standard Time",
            "UTC" => "UTC",
            "GMT" => "GMT Standard Time",
            _ when timezone?.StartsWith("Europe/") == true => timezone, // IANA format (may not work on Windows)
            _ => "UTC" // Default fallback
        };
    }

    // Helper method for recurring job (needs to be public static for Hangfire)
    public static async Task ExecuteFetchLatestJob(string providerCode)
    {
        // This is a simplified version - in production, you'd use proper DI
        // Hangfire will handle the actual job execution with proper service provider
        await Task.CompletedTask;
    }
}
