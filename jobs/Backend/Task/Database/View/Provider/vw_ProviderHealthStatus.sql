CREATE VIEW [dbo].[vw_ProviderHealthStatus]
AS
SELECT 
    p.Id,
    p.Code,
    p.Name,
    p.IsActive,
    p.BaseCurrencyId,
    bc.Code AS BaseCurrencyCode,
    p.RequiresAuthentication,
    p.LastSuccessfulFetch,
    p.LastFailedFetch,
    p.ConsecutiveFailures,
    
    -- Calculate uptime metrics with NULL handling
    CASE 
        WHEN p.LastSuccessfulFetch IS NOT NULL 
        THEN DATEDIFF(HOUR, p.LastSuccessfulFetch, GETDATE())
        ELSE NULL
    END AS HoursSinceLastSuccess,
    
    -- Recent fetch statistics (last 30 days)
    (SELECT COUNT(*) 
     FROM [dbo].[ExchangeRateFetchLog] fl
     WHERE fl.ProviderId = p.Id 
       AND fl.FetchStarted >= DATEADD(DAY, -30, GETDATE())) AS TotalFetches30Days,
    
    (SELECT COUNT(*) 
     FROM [dbo].[ExchangeRateFetchLog] fl
     WHERE fl.ProviderId = p.Id 
       AND fl.Status = 'Success'
       AND fl.FetchStarted >= DATEADD(DAY, -30, GETDATE())) AS SuccessfulFetches30Days,
    
    (SELECT COUNT(*) 
     FROM [dbo].[ExchangeRateFetchLog] fl
     WHERE fl.ProviderId = p.Id 
       AND fl.Status = 'Failed'
       AND fl.FetchStarted >= DATEADD(DAY, -30, GETDATE())) AS FailedFetches30Days,
    
    (SELECT AVG(CAST(fl.DurationMs AS BIGINT))
     FROM [dbo].[ExchangeRateFetchLog] fl
     WHERE fl.ProviderId = p.Id 
       AND fl.Status = 'Success'
       AND fl.DurationMs IS NOT NULL
       AND fl.FetchStarted >= DATEADD(DAY, -30, GETDATE())) AS AvgFetchDurationMs,
    
    -- Health status calculation
    CASE 
        WHEN p.IsActive = 0 THEN 'Disabled'
        WHEN p.ConsecutiveFailures >= 5 THEN 'Critical'
        WHEN p.ConsecutiveFailures >= 3 THEN 'Degraded'
        WHEN p.LastSuccessfulFetch IS NULL THEN 'Never Fetched'
        WHEN DATEDIFF(HOUR, p.LastSuccessfulFetch, GETDATE()) > 24 THEN 'Stale'
        WHEN DATEDIFF(HOUR, p.LastSuccessfulFetch, GETDATE()) > 2 THEN 'Warning'
        ELSE 'Healthy'
    END AS HealthStatus
FROM [dbo].[ExchangeRateProvider] p
INNER JOIN [dbo].[Currency] bc ON p.BaseCurrencyId = bc.Id;
GO