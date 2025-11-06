-- Index for ExchangeRate lookup by provider and date
-- Supports: GetRatesByProviderAndDateRangeAsync, GetCurrentRatesAsync
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ExchangeRate_Provider_Date' AND object_id = OBJECT_ID('[dbo].[ExchangeRate]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ExchangeRate_Provider_Date
        ON [dbo].[ExchangeRate] ([ProviderId], [ValidDate] DESC)
        INCLUDE ([BaseCurrencyId], [TargetCurrencyId], [Rate], [Multiplier]);
END
GO

-- Index for ExchangeRate lookup by currency pair
-- Supports: GetRatesByCurrencyPairAsync (WHERE BaseCurrencyId AND TargetCurrencyId ORDER BY ValidDate DESC)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ExchangeRate_CurrencyPair' AND object_id = OBJECT_ID('[dbo].[ExchangeRate]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ExchangeRate_CurrencyPair
        ON [dbo].[ExchangeRate] ([BaseCurrencyId], [TargetCurrencyId], [ValidDate] DESC)
        INCLUDE ([ProviderId], [Rate], [Multiplier]);
END
GO

-- Index for ExchangeRate lookup by date range
-- Supports: GetRatesByDateRangeAsync (WHERE ValidDate >= start AND ValidDate <= end)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ExchangeRate_ValidDate' AND object_id = OBJECT_ID('[dbo].[ExchangeRate]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ExchangeRate_ValidDate
        ON [dbo].[ExchangeRate] ([ValidDate] DESC)
        INCLUDE ([ProviderId], [BaseCurrencyId], [TargetCurrencyId], [Rate], [Multiplier]);
END
GO

-- Index for FetchLog lookup by provider and date
-- Supports: Monitoring queries for provider fetch history
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_FetchLog_Provider_Date' AND object_id = OBJECT_ID('[dbo].[ExchangeRateFetchLog]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_FetchLog_Provider_Date
        ON [dbo].[ExchangeRateFetchLog] ([ProviderId], [FetchStarted] DESC)
        INCLUDE ([Status], [RatesImported], [RatesUpdated], [FetchCompleted]);
END
GO

-- Index for FetchLog lookup by status
-- Supports: Finding running or failed fetch operations
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_FetchLog_Status' AND object_id = OBJECT_ID('[dbo].[ExchangeRateFetchLog]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_FetchLog_Status
        ON [dbo].[ExchangeRateFetchLog] ([Status], [FetchStarted] DESC)
        INCLUDE ([ProviderId], [RatesImported], [RatesUpdated]);
END
GO

-- Index for ErrorLog lookup by timestamp
-- Supports: Recent error queries, monitoring dashboards
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ErrorLog_Timestamp' AND object_id = OBJECT_ID('[dbo].[ErrorLog]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ErrorLog_Timestamp
        ON [dbo].[ErrorLog] ([Timestamp] DESC)
        INCLUDE ([Severity], [Source], [Message]);
END
GO

-- Index for ErrorLog lookup by severity and timestamp
-- Supports: Filtering errors by severity level
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ErrorLog_Severity_Timestamp' AND object_id = OBJECT_ID('[dbo].[ErrorLog]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ErrorLog_Severity_Timestamp
        ON [dbo].[ErrorLog] ([Severity], [Timestamp] DESC)
        INCLUDE ([Source], [Message], [UserId]);
END
GO

-- Index for provider lookup by code
-- Supports: GetByCodeAsync (WHERE Code) - Code already has UNIQUE constraint, but this covers query columns
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ExchangeRateProvider_Code' AND object_id = OBJECT_ID('[dbo].[ExchangeRateProvider]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ExchangeRateProvider_Code
        ON [dbo].[ExchangeRateProvider] ([Code])
        INCLUDE ([Id], [Name], [Url], [BaseCurrencyId], [IsActive], [RequiresAuthentication]);
END
GO

-- Index for active providers
-- Supports: GetActiveProvidersAsync (WHERE IsActive)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ExchangeRateProvider_IsActive' AND object_id = OBJECT_ID('[dbo].[ExchangeRateProvider]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ExchangeRateProvider_IsActive
        ON [dbo].[ExchangeRateProvider] ([IsActive])
        INCLUDE ([Id], [Code], [Name], [BaseCurrencyId], [Url], [LastSuccessfulFetch]);
END
GO

-- Index for provider health monitoring
-- Supports: Finding providers with consecutive failures for alerting
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ExchangeRateProvider_Health' AND object_id = OBJECT_ID('[dbo].[ExchangeRateProvider]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ExchangeRateProvider_Health
        ON [dbo].[ExchangeRateProvider] ([IsActive], [ConsecutiveFailures], [LastFailedFetch])
        INCLUDE ([Code], [Name], [LastSuccessfulFetch]);
END
GO

-- Index for user lookup by email
-- Supports: GetByEmailAsync (WHERE Email) - Email already has UNIQUE constraint, but this covers query columns
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_User_Email' AND object_id = OBJECT_ID('[dbo].[User]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_User_Email
        ON [dbo].[User] ([Email])
        INCLUDE ([Id], [PasswordHash], [Role], [FirstName], [LastName]);
END
GO

-- Index for user lookup by role
-- Supports: GetByRoleAsync (WHERE Role ORDER BY Email), user management queries
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_User_Role' AND object_id = OBJECT_ID('[dbo].[User]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_User_Role
        ON [dbo].[User] ([Role], [Email])
        INCLUDE ([Id], [FirstName], [LastName]);
END
GO

-- Index for Currency lookup by code
-- Supports: sp_BulkUpsertExchangeRates and GetByCodeAsync queries - Code already has UNIQUE constraint
-- Note: This index may be redundant with the UNIQUE constraint, but explicitly defined for clarity
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Currency_Code' AND object_id = OBJECT_ID('[dbo].[Currency]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Currency_Code
        ON [dbo].[Currency] ([Code])
        INCLUDE ([Id]);
END
GO
