CREATE NONCLUSTERED INDEX IX_ExchangeRate_Lookup 
    ON [dbo].[ExchangeRate] ([TargetCurrencyId], [ValidDate] DESC) 
    INCLUDE ([Rate], [Multiplier], [BaseCurrencyId], [ProviderId]);

CREATE NONCLUSTERED INDEX IX_ExchangeRate_Provider 
    ON [dbo].[ExchangeRate] ([ProviderId], [ValidDate] DESC);

CREATE NONCLUSTERED INDEX IX_ExchangeRate_BaseCurrency 
    ON [dbo].[ExchangeRate] ([BaseCurrencyId], [ValidDate] DESC);

CREATE NONCLUSTERED INDEX IX_FetchLog_Provider_Date 
    ON [dbo].[ExchangeRateFetchLog] ([ProviderId], [FetchStarted] DESC);

CREATE NONCLUSTERED INDEX IX_ErrorLog_Timestamp 
    ON [dbo].[ErrorLog] ([Timestamp] DESC);

CREATE NONCLUSTERED INDEX IX_ErrorLog_Severity 
    ON [dbo].[ErrorLog] ([Severity], [Timestamp] DESC);

CREATE NONCLUSTERED INDEX IX_ExchangeRateProvider_Active 
    ON [dbo].[ExchangeRateProvider] ([IsActive]) 
    INCLUDE ([Code], [Name], [BaseCurrencyId], [Priority]);

CREATE NONCLUSTERED INDEX IX_ExchangeRateProvider_Health 
    ON [dbo].[ExchangeRateProvider] ([ConsecutiveFailures], [LastSuccessfulFetch]) 
    WHERE [IsActive] = 1;

CREATE NONCLUSTERED INDEX IX_ExchangeRateProvider_BaseCurrency 
    ON [dbo].[ExchangeRateProvider] ([BaseCurrencyId], [IsActive]) 
    INCLUDE ([Code], [Name], [Url], [Priority]);

CREATE NONCLUSTERED INDEX IX_ExchangeRateProvider_Auth 
    ON [dbo].[ExchangeRateProvider] ([RequiresAuthentication]) 
    WHERE [IsActive] = 1;

CREATE NONCLUSTERED INDEX IX_ExchangeRateProvider_Lookup 
    ON [dbo].[ExchangeRateProvider] ([Code]) 
    INCLUDE ([Id], [Name], [Url], [BaseCurrencyId], [IsActive]);

CREATE NONCLUSTERED INDEX IX_User_Email 
    ON [dbo].[User] ([Email]) 
    INCLUDE ([PasswordHash], [Role], [FirstName], [LastName]);

CREATE NONCLUSTERED INDEX IX_User_Role 
    ON [dbo].[User] ([Role]) 
    INCLUDE ([Email], [FirstName], [LastName]);
