using ApplicationLayer.Common.Interfaces;
using ConfigurationLayer.Interface;
using Hangfire;
using InfrastructureLayer.BackgroundJobs.Jobs;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer.BackgroundJobs.Services;

/// <summary>
/// Hangfire-based implementation of background job scheduler.
/// Manages scheduling and rescheduling of recurring jobs for providers.
/// </summary>
public class BackgroundJobScheduler : IBackgroundJobScheduler
{
    private readonly IProviderConfigurationService _providerConfigService;
    private readonly IConfigurationService _configService;
    private readonly ILogger<BackgroundJobScheduler> _logger;

    public BackgroundJobScheduler(
        IProviderConfigurationService providerConfigService,
        IConfigurationService configService,
        ILogger<BackgroundJobScheduler> logger)
    {
        _providerConfigService = providerConfigService;
        _configService = configService;
        _logger = logger;
    }

    public async Task RescheduleProviderJobAsync(string providerCode, string updateTime, string timeZone)
    {
        try
        {
            // Get default cron expression from configuration
            var defaultCronExpression = await _configService.GetValueAsync("DefaultCronExpression", "0 16 * * *");

            // Convert time (hh:mm) to cron expression
            var cronExpression = ConvertTimeToCronExpression(updateTime, defaultCronExpression);

            // Convert timezone abbreviation to .NET TimeZoneInfo ID
            var timeZoneId = ConvertTimeZoneId(timeZone);

            // Reschedule the recurring job
            RecurringJob.AddOrUpdate<FetchLatestRatesJob>(
                recurringJobId: $"fetch-latest-{providerCode}",
                methodCall: job => job.ExecuteAsync(providerCode, CancellationToken.None),
                cronExpression: cronExpression,
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)
                });

            _logger.LogInformation(
                "Rescheduled Hangfire job for provider {ProviderCode} - UpdateTime: {UpdateTime}, TimeZone: {TimeZone}, Cron: {CronExpression}",
                providerCode,
                updateTime,
                timeZone,
                cronExpression);

            // Refresh provider configuration cache so other services see the updated configuration
            await _providerConfigService.RefreshCacheAsync();

            _logger.LogInformation(
                "Refreshed provider configuration cache after rescheduling {ProviderCode}",
                providerCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error rescheduling job for provider {ProviderCode}",
                providerCode);
            throw;
        }
    }

    /// <summary>
    /// Converts time in format "hh:mm" to cron expression "minute hour * * *" (daily schedule).
    /// </summary>
    private string ConvertTimeToCronExpression(string time, string defaultCron)
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
    private string ConvertTimeZoneId(string timezone)
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
}
