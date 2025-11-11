IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'AutoDisableProviderAfterFailures')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('AutoDisableProviderAfterFailures', '10', 'Auto-disable provider after N consecutive failures (0 = never)', 'Int');
    PRINT '  ✓ AutoDisableProviderAfterFailures';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'ProviderHealthCheckIntervalMinutes')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('ProviderHealthCheckIntervalMinutes', '15', 'How often to check provider health (in minutes)', 'Int');
    PRINT '  ✓ ProviderHealthCheckIntervalMinutes';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'StaleDataThresholdHours')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('StaleDataThresholdHours', '48', 'Consider data stale after N hours without update', 'Int');
    PRINT '  ✓ StaleDataThresholdHours';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'RetainExchangeRatesDays')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('RetainExchangeRatesDays', '730', 'Keep exchange rates for N days (730 = 2 years)', 'Int');
    PRINT '  ✓ RetainExchangeRatesDays';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'RetainFetchLogsDays')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('RetainFetchLogsDays', '90', 'Keep fetch logs for N days (90 = 3 months)', 'Int');
    PRINT '  ✓ RetainFetchLogsDays';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'RetainErrorLogsDays')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('RetainErrorLogsDays', '30', 'Keep error logs for N days (30 = 1 month)', 'Int');
    PRINT '  ✓ RetainErrorLogsDays';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'EnableAutoCleanup')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('EnableAutoCleanup', 'true', 'Automatically cleanup old data based on retention settings', 'Bool');
    PRINT '  ✓ EnableAutoCleanup';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'LogLevel')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('LogLevel', 'Information', 'Minimum log level: Debug, Information, Warning, Error, Critical', 'String');
    PRINT '  ✓ LogLevel';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'EnableDetailedLogging')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('EnableDetailedLogging', 'false', 'Enable detailed request/response logging (verbose)', 'Bool');
    PRINT '  ✓ EnableDetailedLogging';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'LogSuccessfulFetches')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('LogSuccessfulFetches', 'true', 'Log successful fetch operations', 'Bool');
    PRINT '  ✓ LogSuccessfulFetches';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'ApiRateLimitPerMinute')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('ApiRateLimitPerMinute', '60', 'Max API requests per minute per user', 'Int');
    PRINT '  ✓ ApiRateLimitPerMinute';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'EnableApiKeyAuthentication')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('EnableApiKeyAuthentication', 'false', 'Require API key for public endpoints', 'Bool');
    PRINT '  ✓ EnableApiKeyAuthentication';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'MaxResultsPerPage')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('MaxResultsPerPage', '100', 'Maximum results per page in API responses', 'Int');
    PRINT '  ✓ MaxResultsPerPage';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'SystemVersion')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('SystemVersion', '1.0.0', 'Current system version', 'String');
    PRINT '  ✓ SystemVersion';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'MaintenanceMode')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('MaintenanceMode', 'false', 'Enable maintenance mode (blocks API access)', 'Bool');
    PRINT '  ✓ MaintenanceMode';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'MaintenanceMessage')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('MaintenanceMessage', 'System is under maintenance. Please try again later.', 'Message to display during maintenance', 'String');
    PRINT '  ✓ MaintenanceMessage';
END

-- Background Jobs Configuration
IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'HistoricalDataDays')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('HistoricalDataDays', '90', 'Number of days of historical data to fetch on startup', 'Int');
    PRINT '  ✓ HistoricalDataDays';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'DefaultRetryDelayMinutes')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('DefaultRetryDelayMinutes', '30', 'Default delay (in minutes) before retrying a failed fetch', 'Int');
    PRINT '  ✓ DefaultRetryDelayMinutes';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'MaxRetries')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('MaxRetries', '3', 'Maximum number of retry attempts for a failed fetch', 'Int');
    PRINT '  ✓ MaxRetries';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'HealthCheckIntervalMinutes')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('HealthCheckIntervalMinutes', '5', 'Interval (in minutes) between provider health checks', 'Int');
    PRINT '  ✓ HealthCheckIntervalMinutes';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'RecentDataThresholdHours')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('RecentDataThresholdHours', '2', 'Time (in hours) to consider existing data as recent and schedule a retry', 'Int');
    PRINT '  ✓ RecentDataThresholdHours';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'HangfireWorkerCount')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('HangfireWorkerCount', '5', 'Number of concurrent Hangfire background job workers', 'Int');
    PRINT '  ✓ HangfireWorkerCount';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'DefaultCronExpression')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('DefaultCronExpression', '0 16 * * *', 'Default cron expression for scheduled fetches (daily at 4 PM UTC)', 'String');
    PRINT '  ✓ DefaultCronExpression';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'DefaultTimezone')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('DefaultTimezone', 'UTC', 'Default timezone for cron expressions', 'String');
    PRINT '  ✓ DefaultTimezone';
END