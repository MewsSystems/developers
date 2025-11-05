CREATE VIEW [dbo].[vw_RecentFetchActivity]
AS
SELECT TOP 1000
    fl.Id,
    p.Code AS ProviderCode,
    p.Name AS ProviderName,
    fl.FetchStarted,
    fl.FetchCompleted,
    fl.Status,
    fl.RatesImported,
    fl.RatesUpdated,
    fl.DurationMs,
    fl.ErrorMessage,
    u.Email AS RequestedByEmail,
    CASE 
        WHEN fl.Status = 'Success' THEN 'Success'
        WHEN fl.Status = 'Failed' THEN 'Failed'
        WHEN fl.Status = 'PartialSuccess' THEN 'Partial'
        ELSE 'Running'
    END AS StatusIcon,
    CASE 
        WHEN fl.DurationMs IS NULL THEN NULL
        WHEN fl.DurationMs < 1000 THEN 'Fast'
        WHEN fl.DurationMs < 5000 THEN 'Normal'
        WHEN fl.DurationMs < 10000 THEN 'Slow'
        ELSE 'Very Slow'
    END AS PerformanceCategory
FROM [dbo].[ExchangeRateFetchLog] fl
INNER JOIN [dbo].[ExchangeRateProvider] p ON fl.ProviderId = p.Id
LEFT JOIN [dbo].[User] u ON fl.RequestedBy = u.Id
ORDER BY fl.FetchStarted DESC;
GO