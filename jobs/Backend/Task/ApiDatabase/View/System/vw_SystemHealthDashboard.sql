CREATE VIEW [dbo].[vw_SystemHealthDashboard]
AS
SELECT 
    -- Provider metrics
    (SELECT COUNT(*) FROM [dbo].[ExchangeRateProvider] WHERE IsActive = 1) AS ActiveProviders,
    (SELECT COUNT(*) FROM [dbo].[ExchangeRateProvider] WHERE IsActive = 0) AS InactiveProviders,
    (SELECT COUNT(*) FROM [dbo].[vw_ProviderHealthStatus] WHERE HealthStatus = 'Critical') AS CriticalProviders,
    
    -- Currency metrics
    (SELECT COUNT(*) FROM [dbo].[Currency]) AS TotalCurrencies,
    
    -- Rate metrics
    (SELECT COUNT(*) FROM [dbo].[vw_CurrentExchangeRates]) AS CurrentRatesCount,
    (SELECT COUNT(DISTINCT TargetCurrencyCode) FROM [dbo].[vw_CurrentExchangeRates]) AS CurrenciesWithCurrentRates,
    
    -- Fetch metrics (last 24 hours)
    (SELECT COUNT(*) 
     FROM [dbo].[ExchangeRateFetchLog] 
     WHERE FetchStarted >= DATEADD(HOUR, -24, GETDATE())) AS Fetches24h,
    
    (SELECT COUNT(*) 
     FROM [dbo].[ExchangeRateFetchLog] 
     WHERE FetchStarted >= DATEADD(HOUR, -24, GETDATE())
       AND Status = 'Success') AS SuccessfulFetches24h,
    
    (SELECT COUNT(*) 
     FROM [dbo].[ExchangeRateFetchLog] 
     WHERE FetchStarted >= DATEADD(HOUR, -24, GETDATE())
       AND Status = 'Failed') AS FailedFetches24h,
    
    -- Error metrics (last 24 hours)
    (SELECT COUNT(*) 
     FROM [dbo].[ErrorLog] 
     WHERE Timestamp >= DATEADD(HOUR, -24, GETDATE())) AS Errors24h,
    
    (SELECT COUNT(*) 
     FROM [dbo].[ErrorLog] 
     WHERE Timestamp >= DATEADD(HOUR, -24, GETDATE())
       AND Severity = 'Critical') AS CriticalErrors24h,
    
    -- User metrics
    (SELECT COUNT(*) FROM [dbo].[User]) AS TotalUsers,
    
    -- Data freshness
    (SELECT MAX(ValidDate) FROM [dbo].[ExchangeRate]) AS LatestRateDate,
    CASE 
        WHEN (SELECT MAX(ValidDate) FROM [dbo].[ExchangeRate]) IS NOT NULL
        THEN DATEDIFF(HOUR, (SELECT MAX(ValidDate) FROM [dbo].[ExchangeRate]), CAST(GETDATE() AS DATE))
        ELSE NULL
    END AS HoursSinceLatestRate;
GO