CREATE VIEW [dbo].[vw_RecentFetchActivity]
AS
SELECT TOP 1000
    fl.Id,
    fl.ProviderId,
    p.Code AS ProviderCode,
    p.Name AS ProviderName,
    fl.FetchStarted,
    fl.FetchCompleted,
    fl.Status,
    fl.RatesImported,
    fl.RatesUpdated,
    fl.DurationMs,
    fl.ErrorMessage,
    u.Email AS RequestedByEmail
FROM [dbo].[ExchangeRateFetchLog] fl
INNER JOIN [dbo].[ExchangeRateProvider] p ON fl.ProviderId = p.Id
LEFT JOIN [dbo].[User] u ON fl.RequestedBy = u.Id
ORDER BY fl.FetchStarted DESC;
GO