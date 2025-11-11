CREATE VIEW [dbo].[vw_SystemHealthDashboard]
AS
SELECT 'TotalProviders' AS Metric, CAST(COUNT(*) AS VARCHAR(50)) AS Value, 'Info' AS Status, NULL AS Details FROM [dbo].[ExchangeRateProvider]
UNION ALL
SELECT 'ActiveProviders', CAST(COUNT(*) AS VARCHAR(50)), 'Info', NULL FROM [dbo].[ExchangeRateProvider] WHERE IsActive = 1
UNION ALL
SELECT 'QuarantinedProviders', CAST(COUNT(*) AS VARCHAR(50)), 'Info', NULL FROM [dbo].[vw_ProviderHealthStatus] WHERE HealthStatus = 'Critical'
UNION ALL
SELECT 'TotalCurrencies', CAST(COUNT(*) AS VARCHAR(50)), 'Info', NULL FROM [dbo].[Currency]
UNION ALL
SELECT 'TotalExchangeRates', CAST(COUNT(*) AS VARCHAR(50)), 'Info', NULL FROM [dbo].[ExchangeRate]
UNION ALL
SELECT 'LatestRateDate', CAST(COALESCE(CAST(MAX(ValidDate) AS VARCHAR(50)), '') AS VARCHAR(50)), 'Info', NULL FROM [dbo].[ExchangeRate]
UNION ALL
SELECT 'OldestRateDate', CAST(COALESCE(CAST(MIN(ValidDate) AS VARCHAR(50)), '') AS VARCHAR(50)), 'Info', NULL FROM [dbo].[ExchangeRate]
UNION ALL
SELECT 'TotalFetchesToday', CAST(COUNT(*) AS VARCHAR(50)), 'Info', NULL FROM [dbo].[ExchangeRateFetchLog] WHERE CAST(FetchStarted AS DATE) = CAST(GETDATE() AS DATE)
UNION ALL
SELECT 'SuccessfulFetchesToday', CAST(COUNT(*) AS VARCHAR(50)), 'Info', NULL FROM [dbo].[ExchangeRateFetchLog] WHERE CAST(FetchStarted AS DATE) = CAST(GETDATE() AS DATE) AND Status = 'Success'
UNION ALL
SELECT 'FailedFetchesToday', CAST(COUNT(*) AS VARCHAR(50)), 'Info', NULL FROM [dbo].[ExchangeRateFetchLog] WHERE CAST(FetchStarted AS DATE) = CAST(GETDATE() AS DATE) AND Status = 'Failed'
UNION ALL
SELECT 'SuccessRateToday',
    CAST(
        CASE
            WHEN COUNT(*) = 0 THEN 0
            ELSE (100.0 * SUM(CASE WHEN Status = 'Success' THEN 1 ELSE 0 END) / COUNT(*))
        END AS VARCHAR(50)
    ),
    'Info',
    NULL
FROM [dbo].[ExchangeRateFetchLog]
WHERE CAST(FetchStarted AS DATE) = CAST(GETDATE() AS DATE);
GO