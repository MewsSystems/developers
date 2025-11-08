using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLayer.Option
{
    public class SystemConfigurationOptions
    {
        public ProviderHealthOptions ProviderHealth { get; set; } = new();
        public DataRetentionOptions DataRetention { get; set; } = new();
        public LoggingOptions Logging { get; set; } = new();
        public ApiOptions Api { get; set; } = new();
        public SystemOptions System { get; set; } = new();
        public BackgroundJobOptions BackgroundJobs { get; set; } = new();
    }

    public class ProviderHealthOptions
    {
        public int AutoDisableAfterFailures { get; set; } = 10;
        public int HealthCheckIntervalMinutes { get; set; } = 15;
        public int StaleDataThresholdHours { get; set; } = 48;
    }

    public class DataRetentionOptions
    {
        public int RetainExchangeRatesDays { get; set; } = 730;
        public int RetainFetchLogsDays { get; set; } = 90;
        public int RetainErrorLogsDays { get; set; } = 30;
        public bool EnableAutoCleanup { get; set; } = true;
    }

    public class LoggingOptions
    {
        public string LogLevel { get; set; } = "Information";
        public bool EnableDetailedLogging { get; set; } = false;
        public bool LogSuccessfulFetches { get; set; } = true;
    }

    public class ApiOptions
    {
        public int RateLimitPerMinute { get; set; } = 60;
        public bool EnableApiKeyAuthentication { get; set; } = false;
        public int MaxResultsPerPage { get; set; } = 100;
    }

    public class SystemOptions
    {
        public string Version { get; set; } = "1.0.0";
        public bool MaintenanceMode { get; set; } = false;
        public string MaintenanceMessage { get; set; } = "System is under maintenance. Please try again later.";
    }

    public class BackgroundJobOptions
    {
        public bool FetchHistoricalOnStartup { get; set; } = true;
        public int HistoricalDataDays { get; set; } = 90;
        public int DefaultRetryDelayMinutes { get; set; } = 30;
        public int MaxRetries { get; set; } = 3;
        public int HealthCheckIntervalMinutes { get; set; } = 5;
        public int RecentDataThresholdHours { get; set; } = 2;
        public int HangfireWorkerCount { get; set; } = 5;
        public string DefaultCronExpression { get; set; } = "0 16 * * *";
        public string DefaultTimezone { get; set; } = "UTC";
    }
}
