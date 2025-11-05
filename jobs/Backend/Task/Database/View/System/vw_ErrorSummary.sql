CREATE VIEW [dbo].[vw_ErrorSummary]
AS
SELECT 
    el.Source,
    el.Severity,
    CAST(el.Timestamp AS DATE) AS ErrorDate,
    COUNT(*) AS ErrorCount,
    MIN(el.Timestamp) AS FirstOccurrence,
    MAX(el.Timestamp) AS LastOccurrence,
    COUNT(DISTINCT el.UserId) AS AffectedUsers
FROM [dbo].[ErrorLog] el
WHERE el.Timestamp >= DATEADD(DAY, -30, GETDATE())
GROUP BY 
    el.Source,
    el.Severity,
    CAST(el.Timestamp AS DATE);
GO