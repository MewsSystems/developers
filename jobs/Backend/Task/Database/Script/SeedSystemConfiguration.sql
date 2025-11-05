-- =============================================
-- 1. Fetch Schedule Settings
-- =============================================
PRINT '';
PRINT '1. Fetch Schedule Settings...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'EnableAutoFetch')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('EnableAutoFetch', 'true', 'Automatically fetch rates on schedule', 'Bool');
    PRINT '  ✓ EnableAutoFetch';
END

-- =============================================
-- 2. Caching Settings
-- =============================================
PRINT '';
PRINT '2. Caching Settings...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'EnableCaching')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('EnableCaching', 'true', 'Enable in-memory caching of exchange rates', 'Bool');
    PRINT '  ✓ EnableCaching';
END

-- =============================================
-- 3. Retry & Resilience Settings
-- =============================================
PRINT '';
PRINT '3. Retry & Resilience Settings...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'MaxRetryAttempts')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('MaxRetryAttempts', '3', 'Number of retry attempts on fetch failure', 'Int');
    PRINT '  ✓ MaxRetryAttempts';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'RetryDelaySeconds')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('RetryDelaySeconds', '5', 'Delay between retry attempts (in seconds)', 'Int');
    PRINT '  ✓ RetryDelaySeconds';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'RequestTimeoutSeconds')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('RequestTimeoutSeconds', '30', 'HTTP request timeout (in seconds)', 'Int');
    PRINT '  ✓ RequestTimeoutSeconds';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'CircuitBreakerThreshold')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('CircuitBreakerThreshold', '5', 'Consecutive failures before circuit breaker opens', 'Int');
    PRINT '  ✓ CircuitBreakerThreshold';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'CircuitBreakerDurationSeconds')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('CircuitBreakerDurationSeconds', '60', 'Circuit breaker open duration (in seconds)', 'Int');
    PRINT '  ✓ CircuitBreakerDurationSeconds';
END

-- =============================================
-- 4. Provider Health Settings
-- =============================================
PRINT '';
PRINT '4. Provider Health Settings...';

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

-- =============================================
-- 5. Data Retention Settings
-- =============================================
PRINT '';
PRINT '5. Data Retention Settings...';

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

-- =============================================
-- 6. Logging Settings
-- =============================================
PRINT '';
PRINT '6. Logging Settings...';

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

-- =============================================
-- 7. API Settings
-- =============================================
PRINT '';
PRINT '7. API Settings...';

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

-- =============================================
-- 8. Notification Settings
-- =============================================
PRINT '';
PRINT '8. Notification Settings...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'EnableEmailNotifications')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('EnableEmailNotifications', 'false', 'Send email notifications for critical events', 'Bool');
    PRINT '  ✓ EnableEmailNotifications';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'NotifyOnProviderFailure')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('NotifyOnProviderFailure', 'true', 'Send notification when provider fails', 'Bool');
    PRINT '  ✓ NotifyOnProviderFailure';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'NotifyOnStaleData')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('NotifyOnStaleData', 'true', 'Send notification when data becomes stale', 'Bool');
    PRINT '  ✓ NotifyOnStaleData';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'AdminEmailAddress')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('AdminEmailAddress', 'admin@example.com', 'Email address for system notifications', 'String');
    PRINT '  ✓ AdminEmailAddress';
END

-- =============================================
-- 9. Feature Flags
-- =============================================
PRINT '';
PRINT '9. Feature Flags...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemConfiguration] WHERE [Key] = 'EnableHistoricalDataFetch')
BEGIN
    INSERT INTO [dbo].[SystemConfiguration] ([Key], [Value], [Description], [DataType])
    VALUES ('EnableHistoricalDataFetch', 'true', 'Allow fetching historical exchange rates', 'Bool');
    PRINT '  ✓ EnableHistoricalDataFetch';
END

-- =============================================
-- 10. System Information
-- =============================================
PRINT '';
PRINT '10. System Information...';

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