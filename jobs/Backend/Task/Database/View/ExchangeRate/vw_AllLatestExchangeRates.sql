CREATE VIEW [dbo].[vw_AllLatestExchangeRates]
AS
-- Gets the latest exchange rate for each currency pair (across all providers)
-- Uses MAX(ValidDate) to find the most recent rate regardless of provider
WITH RankedRates AS (
    SELECT
        er.Id,
        er.ProviderId,
        er.BaseCurrencyId,
        er.TargetCurrencyId,
        er.Rate,
        er.Multiplier,
        er.ValidDate,
        er.Created,
        er.Modified,
        ROW_NUMBER() OVER (
            PARTITION BY er.BaseCurrencyId, er.TargetCurrencyId
            ORDER BY er.ValidDate DESC, er.Created DESC
        ) AS RowNum
    FROM [dbo].[ExchangeRate] er
)
SELECT
    rr.Id,
    rr.ProviderId,
    p.Code AS ProviderCode,
    p.Name AS ProviderName,
    bc.Code AS BaseCurrencyCode,
    rr.BaseCurrencyId,
    tc.Code AS TargetCurrencyCode,
    rr.TargetCurrencyId,
    rr.Rate,
    rr.Multiplier,
    CAST(rr.Rate AS DECIMAL(19,6)) / NULLIF(rr.Multiplier, 0) AS RatePerUnit,
    rr.ValidDate,
    rr.Created,
    rr.Modified,
    DATEDIFF(DAY, rr.ValidDate, CAST(GETDATE() AS DATE)) AS DaysOld,
    CASE
        WHEN rr.ValidDate = CAST(GETDATE() AS DATE) THEN 'Current'
        WHEN DATEDIFF(DAY, rr.ValidDate, CAST(GETDATE() AS DATE)) <= 1 THEN 'Recent'
        WHEN DATEDIFF(DAY, rr.ValidDate, CAST(GETDATE() AS DATE)) <= 7 THEN 'Week Old'
        ELSE 'Outdated'
    END AS FreshnessStatus
FROM RankedRates rr
INNER JOIN [dbo].[ExchangeRateProvider] p ON rr.ProviderId = p.Id
INNER JOIN [dbo].[Currency] bc ON rr.BaseCurrencyId = bc.Id
INNER JOIN [dbo].[Currency] tc ON rr.TargetCurrencyId = tc.Id
WHERE rr.RowNum = 1
  AND p.IsActive = 1;
GO
