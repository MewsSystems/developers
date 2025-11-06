CREATE VIEW [dbo].[vw_ErrorSummary]
AS
SELECT
    el.Id,
    el.Timestamp,
    el.Severity,
    el.Source,
    el.Message,
    u.Email AS UserEmail,
    DATEDIFF(MINUTE, el.Timestamp, GETDATE()) AS MinutesAgo
FROM [dbo].[ErrorLog] el
LEFT JOIN [dbo].[User] u ON el.UserId = u.Id
WHERE el.Timestamp >= DATEADD(DAY, -30, GETDATE());
GO